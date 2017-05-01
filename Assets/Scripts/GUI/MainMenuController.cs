using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    [SerializeField]
    private FirstPersonController controller;

    [SerializeField]
    private GameObject mainMenu;

    private GameObject newGameButton;
    private GameObject continueGameButton;
    private bool isPaused = false;

    private void Start()
    {
        newGameButton = transform.Find("MainMenu/NewGame").gameObject;
        continueGameButton = transform.Find("MainMenu/ContinueGame").gameObject;
        continueGameButton.SetActive(false);
        SetMenuVisible(false);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Main");
        newGameButton.SetActive(false);
        continueGameButton.SetActive(true);
    }

    public void ContinueGame()
    {
        SetPause(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPause(!isPaused);
            SetMenuVisible(isPaused);
        }
    }

    private void SetMenuVisible(bool isVisible)
    {
        mainMenu.SetActive(isVisible);
    }

    private void SetPause(bool isPaused)
    {
        this.isPaused = isPaused;
        if (isPaused)
        {
            Time.timeScale = 0.0f;
            controller.SetMouseLookEnabled(false);
            controller.SetHeadBobEnabled(false);
            controller.SetCursorLock(false);
            controller.enabled = false;
        }
        else
        {
            Time.timeScale = 1.0f;
            controller.SetMouseLookEnabled(true);
            controller.SetHeadBobEnabled(true);
            controller.SetCursorLock(true);
            controller.enabled = true;
        }
    }
}
