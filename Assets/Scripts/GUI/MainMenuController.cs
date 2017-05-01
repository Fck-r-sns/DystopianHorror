using UnityEngine;

using EventBus;

public class MainMenuController : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private GameFlowManager gameFlowManager;

    private GameObject mainMenu;
    private GameObject newGameButton;
    private GameObject continueGameButton;
    private int address = AddressProvider.GetFreeAddress();
    private bool isBlocked = false;

    private void Start()
    {
        mainMenu = transform.Find("Canvas/MainMenu").gameObject;
        newGameButton = transform.Find("Canvas/MainMenu/NewGame").gameObject;
        continueGameButton = transform.Find("Canvas/MainMenu/ContinueGame").gameObject;
        continueGameButton.SetActive(false);
        SetMenuVisible(false);

        Dispatcher.Subscribe(EBEventType.GameStarted, address, gameObject);
        Dispatcher.Subscribe(EBEventType.GamePaused, address, gameObject);
        Dispatcher.Subscribe(EBEventType.GameResumed, address, gameObject);
    }

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.GameStarted, address);
        Dispatcher.Unsubscribe(EBEventType.GamePaused, address);
        Dispatcher.Unsubscribe(EBEventType.GameResumed, address);
    }

    public void OnReceived(EBEvent e)
    {
        switch (e.type)
        {
            case EBEventType.GameStarted:
                SetMenuVisible(false);
                newGameButton.SetActive(false);
                continueGameButton.SetActive(true);
                break;

            case EBEventType.GamePaused:
                SetMenuVisible(true);
                break;

            case EBEventType.GameResumed:
                SetMenuVisible(false);
                break;
        }
    }

    public void NewGame()
    {
        if (!isBlocked)
        {
            isBlocked = true;
            gameFlowManager.StartNewGame();
        }
    }

    public void ContinueGame()
    {
        if (!isBlocked)
        {
            gameFlowManager.ResumeGame();
        }
    }

    public void Exit()
    {
        if (!isBlocked)
        {
            gameFlowManager.QuitGame();
        }
    }

    private void SetMenuVisible(bool isVisible)
    {
        mainMenu.SetActive(isVisible);
        isBlocked = !isVisible;
    }
}
