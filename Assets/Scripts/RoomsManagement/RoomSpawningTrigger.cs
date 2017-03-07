using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomSpawningTrigger : MonoBehaviour
{

    [SerializeField]
    private string roomsManagerId = "default";

    [SerializeField]
    private Vector3 rootOffset;

    [SerializeField]
    private Vector3 rootRotation;

    private Scene lastScene;
    private bool loaded;
    private RoomsManager roomsManager;

    private void Start()
    {
        roomsManager = RoomsManager.GetManager(roomsManagerId);
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(LoadRoom());
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (lastSceneName != null)
    //    {
    //        roomsManager.UnloadRoom(lastSceneName);
    //        lastSceneName = null;
    //    }
    //}

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
