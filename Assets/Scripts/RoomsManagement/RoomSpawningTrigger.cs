using UnityEngine;
using EventBus;

public class RoomSpawningTrigger : MonoBehaviour
{

    private int id;

    private void Start()
    {
        RoomEntry entry = transform.parent.gameObject.GetComponent<RoomEntry>();
        id = entry.GetId();
    }

    private void OnTriggerEnter(Collider other)
    {
        Dispatcher.GetInstance().SendEvent(new RoomSpawningTriggerEvent(id, TriggerAction.Enter));
    }

    private void OnTriggerExit(Collider other)
    {
        Dispatcher.GetInstance().SendEvent(new RoomSpawningTriggerEvent(id, TriggerAction.Exit));
    }

}
