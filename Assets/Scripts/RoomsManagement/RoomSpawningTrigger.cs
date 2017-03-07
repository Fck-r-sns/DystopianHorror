using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomSpawningTrigger : MonoBehaviour
{

    private string lastSceneName;

    private void OnTriggerEnter(Collider other)
    {
        Scene room = RoomsManager.GetInstance().GetRandomRoom();
        StartCoroutine(MoveScene(room));
        lastSceneName = room.name;
    }

    private void OnTriggerExit(Collider other)
    {
        if (lastSceneName != null)
        {
            RoomsManager.GetInstance().UnloadRoom(lastSceneName);
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
        root.transform.position = new Vector3(0, 0, 10);
        root.SetActive(true);
    }

}
