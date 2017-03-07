using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomsManager : MonoBehaviour
{

    [SerializeField]
    private string id;

    [SerializeField]
    private string[] rooms;

    private static Dictionary<string, RoomsManager> managers = new Dictionary<string, RoomsManager>();
    private Dictionary<int, RoomSpawningTrigger> doors = new Dictionary<int, RoomSpawningTrigger>();

    public static RoomsManager GetManager(string sceneName)
    {
        return managers[sceneName];
    }

    void Awake()
    {
        managers.Add(id, this);
        foreach(string room in rooms) 
        {
            SceneManager.LoadSceneAsync(room, LoadSceneMode.Additive);
            StartCoroutine(WaitForLoadingAndDisableScene(room));
        }
    }

    public void RegisterDoor(int id, RoomSpawningTrigger door)
    {
        doors.Add(id, door);
    }

    public RoomSpawningTrigger GetRandomDoor()
    {
        int index = Random.Range(0, doors.Count);
        foreach (var door in doors.Values)
        {
            if (index == 0)
            {
                return door;
            }
            --index;
        }
        return null;
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
