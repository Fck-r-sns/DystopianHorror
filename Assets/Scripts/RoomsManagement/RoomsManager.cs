using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomsManager
{

    private static RoomsManager instance;
    private List<string> rooms = new List<string>();

    private RoomsManager()
    {
        RegisterRoom("Room1");
        RegisterRoom("Room2");
        RegisterRoom("Room3");
    }

    public static RoomsManager GetInstance()
    {
        if (instance == null)
        {
            instance = new RoomsManager();
        }
        return instance;
    }

    public void RegisterRoom(string sceneName)
    {
        rooms.Add(sceneName);
    }

    public Scene GetRandomRoom()
    {
        int index = Random.Range(0, rooms.Count);
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
