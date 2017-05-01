using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class GameFlowManager : MonoBehaviour
{

    [SerializeField]
    private FirstPersonController controller;

    private bool isPaused;

    public void StartNewGame()
    {
        ResumeGame();
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
}
