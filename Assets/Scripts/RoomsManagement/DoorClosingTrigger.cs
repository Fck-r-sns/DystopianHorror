using UnityEngine;
using EventBus;

public class DoorClosingTrigger : MonoBehaviour
{

    private int id;

    private void Start()
    {
        RoomEntry entry = transform.parent.gameObject.GetComponent<RoomEntry>();
        id = entry.GetId();
    }

    private void OnTriggerEnter(Collider other)
    {
        Dispatcher.SendEvent(new DoorClosingTriggerEvent(id, TriggerAction.Enter));
    }

    private void OnTriggerExit(Collider other)
    {
        Dispatcher.SendEvent(new DoorClosingTriggerEvent(id, TriggerAction.Exit));
    }

}
