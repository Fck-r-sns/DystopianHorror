using UnityEngine;
using UnityEngine.UI;

using EventBus;

public class MainMenuController : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private GameFlowManager gameFlowManager;

    [SerializeField]
    private GameSettings gameSettings;

    private GameObject menu;
    private GameObject mainMenu;
    private GameObject settingsMenu;
    private GameObject newGameButton;
    private GameObject continueGameButton;
    private GameObject backFromSettingsButton;

    private Slider brightnessSlider;
    private Text brightnessText;
    private Toggle ambientOcclusionToggle;
    private Slider volumeSlider;
    private Text volumeText;
    private Slider mouseSensitivitySlider;
    private Text mouseSensitivityText;

    private int address = AddressProvider.GetFreeAddress();
    private bool isBlocked = false;

    private void Start()
    {
        menu = transform.Find("Canvas/Menu").gameObject;
        mainMenu = transform.Find("Canvas/Menu/Main").gameObject;
        settingsMenu = transform.Find("Canvas/Menu/Settings").gameObject;
        newGameButton = transform.Find("Canvas/Menu/Main/NewGame").gameObject;
        continueGameButton = transform.Find("Canvas/Menu/Main/ContinueGame").gameObject;

        brightnessSlider = transform.Find("Canvas/Menu/Settings/BrightnessValue").gameObject.GetComponent<Slider>();
        brightnessText = transform.Find("Canvas/Menu/Settings/BrightnessValueText").gameObject.GetComponent<Text>();
        ambientOcclusionToggle = transform.Find("Canvas/Menu/Settings/AmbientOcclusionValue").gameObject.GetComponent<Toggle>();
        volumeSlider = transform.Find("Canvas/Menu/Settings/VolumeValue").gameObject.GetComponent<Slider>();
        volumeText = transform.Find("Canvas/Menu/Settings/VolumeValueText").gameObject.GetComponent<Text>();
        mouseSensitivitySlider = transform.Find("Canvas/Menu/Settings/MouseSensitivityValue").gameObject.GetComponent<Slider>();
        mouseSensitivityText = transform.Find("Canvas/Menu/Settings/MouseSensitivityValueText").gameObject.GetComponent<Text>();

        InitSettingsGUI();

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

    public void OpenSettings()
    {
        if (!isBlocked)
        {
            mainMenu.SetActive(false);
            settingsMenu.SetActive(true);
            UpdateSettingsGUI();
        }
    }

    public void Exit()
    {
        if (!isBlocked)
        {
            gameFlowManager.QuitGame();
        }
    }

    public void BackFromSettings()
    {
        if (!isBlocked)
        {
            settingsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }

    private void SetMenuVisible(bool isVisible)
    {
        menu.SetActive(isVisible);
        isBlocked = !isVisible;
    }

    private void InitSettingsGUI()
    {
        brightnessSlider.onValueChanged.AddListener(value =>
        {
            gameSettings.brightness = value;
            brightnessText.text = value.ToString("0.00");
        });
        ambientOcclusionToggle.onValueChanged.AddListener(value =>
        {
            gameSettings.ambientOcclusionEnabled = value;
        });
        volumeSlider.onValueChanged.AddListener(value =>
        {
            gameSettings.volume = value;
            volumeText.text = value.ToString("0.00");
        });
        mouseSensitivitySlider.onValueChanged.AddListener(value =>
        {
            gameSettings.mouseSensitivity = value;
            mouseSensitivityText.text = value.ToString("0.00");
        });
    }

    private void UpdateSettingsGUI()
    {
        brightnessSlider.value = gameSettings.brightness;
        brightnessText.text = gameSettings.brightness.ToString("0.00");
        ambientOcclusionToggle.isOn = gameSettings.ambientOcclusionEnabled;
        volumeSlider.value = gameSettings.volume;
        volumeText.text = gameSettings.volume.ToString("0.00");
        mouseSensitivitySlider.value = gameSettings.mouseSensitivity;
        mouseSensitivityText.text = gameSettings.mouseSensitivity.ToString("0.00");
    }
}
