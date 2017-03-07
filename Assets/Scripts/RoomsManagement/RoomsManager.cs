using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomsManager : MonoBehaviour
{

    private static Dictionary<string, RoomsManager> managers = new Dictionary<string, RoomsManager>();

    [SerializeField]
    private string id;

    [SerializeField]
    private string[] rooms;

    public static RoomsManager GetManager(string sceneName)
    {
        return managers[sceneName];
    }

    private void Awake()
    {
        managers.Add(id, this);
        foreach(string room in rooms) 
        {
            SceneManager.LoadSceneAsync(room, LoadSceneMode.Additive);
            StartCoroutine(WaitForLoadingAndDisableScene(room));
        }
    }

    public Scene GetRandomRoom()
    {
        int index = Random.Range(0, rooms.Length);
        string sceneName = rooms[index];
        return SceneManager.GetSceneByName(sceneName);
    }

    public GameObject GetRoot(Scene scene)
    {
        return scene.GetRootGameObjects()[0];
    }

    public void EnableRoom(Scene scene)
    {
        GetRoot(scene).SetActive(true);
    }

    public void DisableRoom(Scene scene)
    {
        GetRoot(scene).SetActive(false);
    }

    private IEnumerator WaitForLoadingAndDisableScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (!scene.IsValid())
        {
            yield break;
        }
        yield return new WaitUntil(() => scene.isLoaded);
        GameObject root = scene.GetRootGameObjects()[0];
        root.SetActive(false);
    }

}
