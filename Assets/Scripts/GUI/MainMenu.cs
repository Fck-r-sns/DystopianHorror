using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene("Prologue", LoadSceneMode.Additive);
        SceneManager.LoadScene("Hall", LoadSceneMode.Additive);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
