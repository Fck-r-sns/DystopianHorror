using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomsManager : MonoBehaviour
{

    [SerializeField]
    private string id;

    [SerializeField]
    private WorldState worldState;

    [SerializeField]
    private string[] roomNames;

    private static Dictionary<string, RoomsManager> managers = new Dictionary<string, RoomsManager>();
    private List<RoomEntry> roomEntries = new List<RoomEntry>();
    private List<RoomScene> roomScenes = new List<RoomScene>();

    public static RoomsManager GetManager(string sceneName)
    {
        return managers[sceneName];
    }

    void Awake()
    {
        managers.Add(id, this);
        foreach(string room in roomNames) 
        {
            SceneManager.LoadSceneAsync(room, LoadSceneMode.Additive);
            StartCoroutine(WaitForLoadingAndInitScene(room));
        }
    }

    public void RegisterRoomEntry(RoomEntry door)
    {
        roomEntries.Add(door);
    }

    public RoomEntry GetRandomRoomEntry()
    {
        List<RoomEntry> filtered = new List<RoomEntry>(roomEntries.Count);
        foreach (RoomEntry re in roomEntries)
        {
            if (re.CheckPredicate(worldState))
            {
                filtered.Add(re);
            }
        }
        int index = Random.Range(0, filtered.Count);
        return filtered[index];
    }

    public RoomScene GetRandomRoomScene()
    {
        List<RoomScene> filtered = new List<RoomScene>(roomScenes.Count);
        foreach (RoomScene rs in roomScenes)
        {
            if (rs.CheckPredicate(worldState))
            {
                filtered.Add(rs);
            }
        }
        int index = Random.Range(0, filtered.Count);
        return filtered[index];
    }

    private IEnumerator WaitForLoadingAndInitScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (!scene.IsValid())
        {
            yield break;
        }
        yield return new WaitUntil(() => scene.isLoaded);
        GameObject root = scene.GetRootGameObjects()[0];
        root.transform.position = new Vector3(-100, 0, -100);
        RoomScene roomScene = root.GetComponent<RoomScene>();
        roomScene.SetScene(scene);
        roomScenes.Add(roomScene);

        // wait two frames (to complete start routines of rooms)
        yield return null;
        yield return null;
        roomScene.SetEnabled(false);
    }

}
