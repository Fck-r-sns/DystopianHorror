using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomSpawningTrigger : MonoBehaviour
{

    [SerializeField]
    private string roomsManagerId = "default";

    [SerializeField]
    private int id;

    [SerializeField]
    private Vector3 rootOffset;

    [SerializeField]
    private Vector3 rootRotation;

    private Scene lastScene;
    private RoomsManager roomsManager;

    public void MoveScene(Scene scene)
    {
        GameObject root = roomsManager.GetRoot(scene);
        root.transform.position = rootOffset;
        root.transform.eulerAngles = rootRotation;
        root.SetActive(true);
    }

    public Vector3 GetPosition()
    {
        return rootOffset;
    }

    public Vector3 GetRotation()
    {
        return rootRotation;
    }

    private void Start()
    {
        roomsManager = RoomsManager.GetManager(roomsManagerId);
        roomsManager.RegisterDoor(id, this);
    }

    private void OnTriggerEnter(Collider other)
    {
        LoadRoom();
        EventBus.Dispatcher.SendEvent(new RoomSpawningTriggerEnteredEvent(roomsManagerId, id));
    }

    private void LoadRoom()
    {
        Scene scene = roomsManager.GetRandomRoom();
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

}
