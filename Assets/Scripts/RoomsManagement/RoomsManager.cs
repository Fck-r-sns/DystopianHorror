using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomsManager : MonoBehaviour
{

    [SerializeField]
    private string id;

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
        int index = Random.Range(0, roomEntries.Count);
        return roomEntries[index];
    }

    public RoomScene GetRandomRoomScene()
    {
        int index = Random.Range(0, roomScenes.Count);
        return roomScenes[index];
    }

    public void EnableRoom(RoomScene scene)
    {
        scene.SetEnabled(true);
    }

    public void DisableRoom(RoomScene scene)
    {
        scene.SetEnabled(false);
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
        RoomScene roomScene = root.GetComponent<RoomScene>();
        DisableRoom(roomScene);
        roomScene.SetScene(scene);
        roomScenes.Add(roomScene);
    }

}
