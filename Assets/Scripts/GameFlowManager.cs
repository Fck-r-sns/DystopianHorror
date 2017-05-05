using System.Collections;
using UnityEngine;

using EventBus;

public class GameFlowManager : MonoBehaviour
{

    [SerializeField]
    private GameObject mainMenuCameraHolder;

    [SerializeField]
    private FirstPersonController controller;

    private bool isPauseAllowed = false;
    private bool isPaused;

    private bool isMouseLookEnabled = true;
    private bool isHeadBobEnabled = true;
    private bool isCursorLocked = true;
    private bool isControllerEnabled = true;

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
        FadingManager.GetInstance().FadeToNormal(4.0f);
    }

    private void Update()
    {
        if (isPauseAllowed && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void ApplyPause()
    {
        if (isPaused)
        {
            Time.timeScale = 0.0f;

            isMouseLookEnabled = controller.IsMouseLookEnabled();
            isHeadBobEnabled = controller.IsHeadbobEnabled();
            isCursorLocked = controller.IsCursorLocked();
            isControllerEnabled = controller.enabled;

            controller.SetMouseLookEnabled(false);
            controller.SetHeadBobEnabled(false);
            controller.SetCursorLock(false);
            controller.enabled = false;
        }
        else
        {
            Time.timeScale = 1.0f;
            controller.SetMouseLookEnabled(isMouseLookEnabled);
            controller.SetHeadBobEnabled(isHeadBobEnabled);
            controller.SetCursorLock(isCursorLocked);
            controller.enabled = isControllerEnabled;
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

        TextOutput textOutput = TextOutput.GetInstance();
        textOutput.ShowText(TextManager.GetIntroText(), TextOutput.TextAreaSize.Big);
        yield return new WaitWhile(() => textOutput.IsActive());

        FadingManager.GetInstance().FadeToNormal(3.0f);
        yield return new WaitForSeconds(1.0f);

        ResumeGame();
        isPauseAllowed = true;
    }
}
