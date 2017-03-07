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

    private string lastSceneName;
    private RoomsManager roomsManager;

    private void Start()
    {
        roomsManager = RoomsManager.GetManager(roomsManagerId);
    }

    private void OnTriggerEnter(Collider other)
    {
        Scene room = roomsManager.GetRandomRoom();
        StartCoroutine(MoveScene(room));
        lastSceneName = room.name;
    }

    private void OnTriggerExit(Collider other)
    {
        if (lastSceneName != null)
        {
            roomsManager.UnloadRoom(lastSceneName);
            lastSceneName = null;
        }
    }

    private IEnumerator MoveScene(Scene scene)
    {
        while (!scene.isLoaded)
        {
            yield return null;
        }
        GameObject root = scene.GetRootGameObjects()[0];
        root.transform.position = rootOffset;
        root.SetActive(true);
    }

}
