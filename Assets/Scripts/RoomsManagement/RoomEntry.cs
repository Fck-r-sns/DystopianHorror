using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventBus;

public class RoomEntry : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private string roomsManagerId = "default";

    [SerializeField]
    private int id;

    [SerializeField]
    private Vector3 rootOffset;

    [SerializeField]
    private Vector3 rootRotation;

    private int address = AddressProvider.GetFreeAddress();
    private Scene lastScene;
    private RoomsManager roomsManager;
    private Door door;
    private bool spawningEnabled = true;

    public string GetRoomsManagerId()
    {
        return roomsManagerId;
    }

    public int GetId()
    {
        return id;
    }

    public Vector3 GetRootOffset()
    {
        return rootOffset;
    }

    public Vector3 GetRootRotation()
    {
        return rootRotation;
    }

    public bool IsSpawningEnabled()
    {
        return spawningEnabled;
    }

    public void SetSpawningEnabled(bool enabled)
    {
        spawningEnabled = enabled;
    }

    public void OnReceived(EBEvent e)
    {
        switch (e.type)
        {
            case EBEventType.RoomSpawningTrigger:
                RoomSpawningTriggerEvent rstee = (e as RoomSpawningTriggerEvent);
                if (rstee.roomsManagerId.Equals(roomsManagerId) && (rstee.action == TriggerAction.Enter))
                {
                    if ((rstee.roomEntryId == id) && spawningEnabled)
                    {
                        Scene room = roomsManager.GetRandomRoom();
                        AttachRoom(room);
                    }

                    if (rstee.roomEntryId != id)
                    {
                        SetSpawningEnabled(true);
                    }
                }
                break;

            case EBEventType.DoorClosingTrigger:
                DoorClosingTriggerEvent dcte = (e as DoorClosingTriggerEvent);
                if (dcte.roomsManagerId.Equals(roomsManagerId) && (dcte.roomEntryId == id) && (dcte.action == TriggerAction.Exit))
                {
                    CloseDoor();
                }
                break;

            case EBEventType.HallMovingTriggerEntered:
                CloseDoor();
                break;
        }
    }

    void Start()
    {
        roomsManager = RoomsManager.GetManager(roomsManagerId);
        roomsManager.RegisterDoor(id, this);
        door = GetComponentInChildren<Door>();
        Dispatcher.Subscribe(EBEventType.RoomSpawningTrigger, address, gameObject);
        Dispatcher.Subscribe(EBEventType.DoorClosingTrigger, address, gameObject);
        Dispatcher.Subscribe(EBEventType.HallMovingTriggerEntered, address, gameObject);
    }

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.RoomSpawningTrigger, address);
        Dispatcher.Unsubscribe(EBEventType.DoorClosingTrigger, address);
        Dispatcher.Unsubscribe(EBEventType.HallMovingTriggerEntered, address);
    }

    public void CloseDoor()
    {
        //door.setClosed();
        door.close();
    }

    public void AttachRoom(Scene scene)
    {
        if (lastScene.IsValid())
        {
            if (lastScene.name.Equals(scene.name))
            {
                MoveScene(scene);
                return;
            }
            else
            {
                roomsManager.DisableRoom(lastScene);
            }
        }
        MoveScene(scene);
        lastScene = scene;
    }

    private void MoveScene(Scene scene)
    {
        GameObject root = roomsManager.GetRoot(scene);
        root.transform.position = rootOffset;
        root.transform.eulerAngles = rootRotation;
        root.SetActive(true);
    }

}
