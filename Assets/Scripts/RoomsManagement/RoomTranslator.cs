using UnityEngine;

using EventBus;

public class RoomTranslator : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private string roomsManagerId = "default";

    [SerializeField]
    private RoomScene roomScene;

    private RoomsManager roomsManager;
    private int address = AddressProvider.GetFreeAddress();
    private bool translationEnabled = true;
    private bool needTranslateRoom = false;

    public void OnReceived(EBEvent e)
    {
        switch (e.type)
        {
            case EBEventType.RoomSpawningTrigger:
                translationEnabled = true;
                break;

            case EBEventType.ItemCollected:
                ItemCollectedEvent ice = e as ItemCollectedEvent;
                if ((ice.item.GetItemType() == CollectibleItem.Type.Key) && (ice.item.GetRoomScene() == roomScene))
                {
                    needTranslateRoom = true;
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
        roomsManager = RoomsManager.GetManager(roomsManagerId);
        Dispatcher.Subscribe(EBEventType.RoomSpawningTrigger, address, gameObject);
        Dispatcher.Subscribe(EBEventType.ItemCollected, address, gameObject);
    }

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.RoomSpawningTrigger, address);
        Dispatcher.Unsubscribe(EBEventType.ItemCollected, address);
    }

    private void Update()
    {
        if (needTranslateRoom)
        {
            needTranslateRoom = false;
            TranslateRoom();
        }
    }

    private void TranslateRoom()
    {
        RoomEntry door = roomsManager.GetRandomRoomEntry();
        door.SetSpawningEnabled(false);
        GameObject root = roomScene.GetRoot();
        Vector3 oldPosition = root.transform.position;
        Vector3 oldRotation = root.transform.eulerAngles;
        door.AttachRoom(roomScene);
        Dispatcher.SendEvent(new HallMovingTriggerEnteredEvent(
            oldPosition,
            oldRotation,
            door.GetRootOffset(),
            door.GetRootRotation()
            ));
    }
}
