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

    private void Start()
    {
        roomsManager = RoomsManager.GetManager(roomsManagerId);
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(LoadRoom());
        EventBus.Dispatcher.SendEvent(new RoomSpawningTriggerEnteredEvent(roomsManagerId, id));
    }

    private IEnumerator LoadRoom()
    {
        Scene scene = roomsManager.GetRandomRoom();
        if (lastScene.IsValid())
        {
            if (lastScene.name.Equals(scene.name))
            {
                MoveScene(scene);
                yield break;
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
