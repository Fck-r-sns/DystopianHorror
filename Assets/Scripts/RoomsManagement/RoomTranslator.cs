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

    public void OnReceived(EBEvent e)
    {
        if (e.type == EBEventType.RoomSpawningTrigger)
        {
            translationEnabled = true;
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
    }

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.RoomSpawningTrigger, address);
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
