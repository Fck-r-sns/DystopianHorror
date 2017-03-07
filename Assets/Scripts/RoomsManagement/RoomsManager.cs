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
    }

    public Scene GetRandomRoom()
    {
        int index = Random.Range(0, rooms.Length);
        string sceneName = rooms[index];
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        return SceneManager.GetSceneByName(sceneName);
    }

    public void UnloadRoom(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }

}
