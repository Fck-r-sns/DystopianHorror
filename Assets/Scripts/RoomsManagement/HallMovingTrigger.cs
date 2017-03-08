using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class HallMovingTrigger : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private string roomsManagerId = "default";

    private Collider collider;
    private RoomsManager roomsManager;
    private int address = AddressProvider.GetFreeAddress();

    public void OnReceived(EBEvent e)
    {
        if (e.type == EBEventType.RoomSpawningTriggerEntered)
        {
            collider.enabled = true; // reset trigger
        }
    }

    private void Start()
    {
        collider = GetComponent<Collider>();
        roomsManager = RoomsManager.GetManager(roomsManagerId);
        Dispatcher.Subscribe(EBEventType.RoomSpawningTriggerEntered, address, gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        collider.enabled = false;
        RoomEntry door = roomsManager.GetRandomRoomEntry();
        GameObject root = roomsManager.GetRoot(gameObject.scene);
        Vector3 oldPosition = root.transform.position;
        Vector3 oldRotation = root.transform.eulerAngles;
        door.CloseDoor();
        door.AttachRoom(gameObject.scene);
        EventBus.Dispatcher.SendEvent(new HallMovingTriggerEnteredEvent(
            oldPosition,
            oldRotation,
            door.GetRootOffset(),
            door.GetRootRotation()
            ));
    }
}
