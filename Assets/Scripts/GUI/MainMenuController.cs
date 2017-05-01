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
    private bool isPaused = false;

    private void Start()
    {
        mainMenu = transform.Find("MainMenu").gameObject;
        newGameButton = transform.Find("MainMenu/NewGame").gameObject;
        continueGameButton = transform.Find("MainMenu/ContinueGame").gameObject;
        continueGameButton.SetActive(false);
        SetMenuVisible(false);

        Dispatcher.Subscribe(EBEventType.GamePaused, address, gameObject);
        Dispatcher.Subscribe(EBEventType.GameResumed, address, gameObject);
    }

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.GamePaused, address);
        Dispatcher.Unsubscribe(EBEventType.GameResumed, address);
    }

    public void OnReceived(EBEvent e)
    {
        switch (e.type)
        {
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
        gameFlowManager.StartNewGame();
        newGameButton.SetActive(false);
        continueGameButton.SetActive(true);
    }

    public void ContinueGame()
    {
        gameFlowManager.ResumeGame();
    }

    public void Exit()
    {
        gameFlowManager.QuitGame();
    }

    private void SetMenuVisible(bool isVisible)
    {
        mainMenu.SetActive(isVisible);
    }
}
