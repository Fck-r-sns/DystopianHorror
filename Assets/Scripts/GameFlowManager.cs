using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using EventBus;

public class GameFlowManager : MonoBehaviour
{

    [SerializeField]
    private GameObject mainMenuCameraHolder;

    [SerializeField]
    private FirstPersonController controller;

    private bool isPauseAllowed = false;
    private bool keepMouseLock = false;
    private bool isPaused;

    private bool isMouseLookEnabled = true;
    private bool isHeadBobEnabled = true;
    private bool isCursorLocked = true;
    private bool isControllerEnabled = true;

    // hack
    public void SetPauseAllowed(bool value)
    {
        isPauseAllowed = value;
    }

    // hack
    public void SetKeepMouseLock(bool value)
    {
        keepMouseLock = value;
    }

    public void StartNewGame()
    {
        StartCoroutine(StartGame_impl());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame(bool sendEvent = true)
    {
        isPaused = true;
        ApplyPause();
        if (sendEvent)
        {
            Dispatcher.GetInstance().SendEvent(new EBEvent() { type = EBEventType.GamePaused });
        }
    }

    public void ResumeGame(bool sendEvent = true)
    {
        isPaused = false;
        ApplyPause();
        if (sendEvent)
        {
            Dispatcher.GetInstance().SendEvent(new EBEvent() { type = EBEventType.GameResumed });
        }
    }

    public void TogglePause(bool sendEvent = true)
    {
        isPaused = !isPaused;
        ApplyPause();
        if (sendEvent)
        {
            Dispatcher.GetInstance().SendEvent(new EBEvent() { type = (isPaused ? EBEventType.GamePaused : EBEventType.GameResumed) });
        }
    }

    private void Start()
    {
        StartCoroutine(GameLoading_impl());
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
            if (!keepMouseLock)
            {
                controller.SetCursorLock(false);
            }
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
    }

    private IEnumerator GameLoading_impl()
    {
        FadingManager.GetInstance().SetFadedToBlack();
        PauseGame();
        Time.timeScale = 1.0f;
        yield return new WaitUntil(() => SceneManager.GetSceneByName("Prologue").isLoaded && SceneManager.GetSceneByName("Hall").isLoaded);
        Dispatcher.GetInstance().SendEvent(new EBEvent() { type = EBEventType.GameLoaded });
        FadingManager.GetInstance().FadeToNormal(4.0f);
    }

    private IEnumerator StartGame_impl()
    {
        FadingManager.GetInstance().FadeToBlack(2.0f);
        yield return new WaitForSeconds(2.0f);

        mainMenuCameraHolder.SetActive(false);
        controller.gameObject.SetActive(true);
        controller.SetCursorLock(true);

        Dispatcher.GetInstance().SendEvent(new EBEvent() { type = EBEventType.GameStarted });

        TextOutput textOutput = TextOutput.GetInstance();
        textOutput.ShowText(TextManager.GetIntroText(), TextOutput.TextAreaSize.Big);
        yield return new WaitWhile(() => textOutput.IsActive());

        FadingManager.GetInstance().FadeToNormal(3.0f);
        yield return new WaitForSeconds(1.0f);

        ResumeGame();
        isPauseAllowed = true;
    }
}
