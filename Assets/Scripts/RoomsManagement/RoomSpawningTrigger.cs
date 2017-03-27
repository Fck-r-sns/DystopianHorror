using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomSpawningTrigger : MonoBehaviour
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
        EventBus.Dispatcher.SendEvent(new RoomSpawningTriggerEvent(roomsManagerId, id, RoomSpawningTriggerEvent.Action.Enter));
    }

    private void OnTriggerExit(Collider other)
    {
        EventBus.Dispatcher.SendEvent(new RoomSpawningTriggerEvent(roomsManagerId, id, RoomSpawningTriggerEvent.Action.Exit));
    }

}
