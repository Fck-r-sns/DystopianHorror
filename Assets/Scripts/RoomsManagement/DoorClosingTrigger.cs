using UnityEngine;
using EventBus;

public class DoorClosingTrigger : MonoBehaviour
{

    private string roomsManagerId = "default";
    private int id;

    private void Start()
    {
        RoomEntry entry = transform.parent.gameObject.GetComponent<RoomEntry>();
        roomsManagerId = entry.GetRoomsManagerId();
        id = entry.GetId();
    }

    private void OnTriggerEnter(Collider other)
    {
        Dispatcher.SendEvent(new DoorClosingTriggerEvent(roomsManagerId, id, TriggerAction.Enter));
    }

    private void OnTriggerExit(Collider other)
    {
        Dispatcher.SendEvent(new DoorClosingTriggerEvent(roomsManagerId, id, TriggerAction.Exit));
    }

}
