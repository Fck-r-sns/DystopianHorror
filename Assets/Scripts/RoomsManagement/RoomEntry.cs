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

    public void OnReceived(EBEvent e)
    {
        if (e.type == EBEventType.RoomSpawningTriggerEntered)
        {
            RoomSpawningTriggerEnteredEvent rstee = (e as RoomSpawningTriggerEnteredEvent);
            if (rstee.roomsManagerId.Equals(roomsManagerId) && rstee.roomEntryId == id)
            {
                Scene room = roomsManager.GetRandomRoom();
                AttachRoom(room);
            }
        }
    }

    void Start()
    {
        roomsManager = RoomsManager.GetManager(roomsManagerId);
        roomsManager.RegisterDoor(id, this);
        Dispatcher.Subscribe(EBEventType.RoomSpawningTriggerEntered, address, gameObject);
    }

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.RoomSpawningTriggerEntered, address);
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
