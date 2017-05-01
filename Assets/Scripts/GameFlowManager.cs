using System.Collections;
using UnityEngine;

using EventBus;

public class GameFlowManager : MonoBehaviour
{

    [SerializeField]
    private GameObject mainMenuCameraHolder;

    [SerializeField]
    private FirstPersonController controller;

    private bool isPaused;

    public void StartNewGame()
    {
        StartCoroutine(StartGame_impl());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        isPaused = true;
        ApplyPause();
    }

    public void ResumeGame()
    {
        isPaused = false;
        ApplyPause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        ApplyPause();
    }

    private void Start()
    {
        PauseGame();
        Time.timeScale = 1.0f;
        FadingManager.GetInstance().FadeToNormal(6.0f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void ApplyPause()
    {
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

        Dispatcher.SendEvent(new EBEvent() { type = (isPaused ? EBEventType.GamePaused : EBEventType.GameResumed) });
    }

    private IEnumerator StartGame_impl()
    {
        FadingManager.GetInstance().FadeToBlack(2.0f);
        yield return new WaitForSeconds(2.0f);

        mainMenuCameraHolder.SetActive(false);
        controller.gameObject.SetActive(true);
        controller.SetCursorLock(true);

        Dispatcher.SendEvent(new EBEvent() { type = EBEventType.GameStarted });

        FadingManager.GetInstance().FadeToNormal(5.0f);
        yield return new WaitForSeconds(1.0f);

        ResumeGame();
    }
}
