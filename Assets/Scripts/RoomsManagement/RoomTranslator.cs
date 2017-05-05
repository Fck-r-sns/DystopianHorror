using UnityEngine;

using EventBus;
using System.Collections;

public class RoomTranslator : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private RoomScene roomScene;

    private RoomsManager roomsManager;
    private RoomEntry attachedDoor;
    private int address = AddressProvider.GetFreeAddress();
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
        Dispatcher.Subscribe(EBEventType.RoomSpawningTrigger, address, gameObject);
        Dispatcher.Subscribe(EBEventType.ItemCollected, address, gameObject);
    }

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.RoomSpawningTrigger, address);
        Dispatcher.Unsubscribe(EBEventType.ItemCollected, address);
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
        Dispatcher.SendEvent(new HallMovingTriggerEnteredEvent(
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
