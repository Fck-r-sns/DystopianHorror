using UnityEngine;

using EventBus;
using System.Collections;

public class RoomTranslator : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private RoomScene roomScene;

    private RoomsManager roomsManager;
    private RoomEntry attachedDoor;
    private Dispatcher dispatcher;
    private int address;
    private bool translationEnabled = true;
    private int translationFrame = 0;

    public void OnReceived(EBEvent e)
    {
        switch (e.type)
        {
            case EBEventType.RoomSpawningTrigger:
                if (attachedDoor == null || (e as RoomSpawningTriggerEvent).roomEntryId != attachedDoor.GetId())
                {
                    translationEnabled = true;
                }
                break;

            case EBEventType.ItemCollected:
                ItemCollectedEvent ice = e as ItemCollectedEvent;
                if ((ice.item.GetItemType() == CollectibleItem.Type.Key) && (ice.item.GetRoomScene() == roomScene))
                {
                    TranslateRoom(); // translate to ending scene
                }
                break;
        }
    }

    public void OnTranslationTriggerEnter()
    {
        if (translationEnabled)
        {
            translationEnabled = false;
            TranslateRoom();
        }
    }

    private void Start()
    {
        roomsManager = RoomsManager.GetManager();
        dispatcher = Dispatcher.GetInstance();
        address = dispatcher.GetFreeAddress();
        dispatcher.Subscribe(EBEventType.RoomSpawningTrigger, address, gameObject);
        dispatcher.Subscribe(EBEventType.ItemCollected, address, gameObject);
    }

    private void OnDestroy()
    {
        dispatcher.Unsubscribe(EBEventType.RoomSpawningTrigger, address);
        dispatcher.Unsubscribe(EBEventType.ItemCollected, address);
    }

    private void TranslateRoom()
    {
        attachedDoor = roomsManager.GetRandomRoomEntry();
        attachedDoor.SetSpawningEnabled(false);
        GameObject root = roomScene.GetRoot();
        Vector3 oldPosition = root.transform.position;
        Vector3 oldRotation = root.transform.eulerAngles;
        translationFrame = Time.frameCount + 2;
        StartCoroutine(TranslateRoomOnNextUpdate(attachedDoor));
        dispatcher.SendEvent(new HallMovingTriggerEnteredEvent(
            translationFrame,
            oldPosition,
            oldRotation,
            attachedDoor.GetRootOffset(),
            attachedDoor.GetRootRotation()
            ));
    }

    private IEnumerator TranslateRoomOnNextUpdate(RoomEntry door)
    {
        yield return new WaitWhile(() => Time.frameCount < translationFrame);
        door.AttachRoom(roomScene);
    }
}
