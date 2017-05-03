using UnityEngine;
using UnityEngine.UI;

public class TextOutput : MonoBehaviour
{

    private static TextOutput instance;
    private GameObject textOutput;
    private Text text;
    private Image background;
    private Text tip;

    public static TextOutput GetInstance()
    {
        return instance;
    }

    public void ShowText(string text)
    {
        this.text.text = text;
        textOutput.SetActive(true);
    }

    public bool IsActive()
    {
        return textOutput.activeSelf;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        textOutput = transform.Find("Canvas/TextOutput").gameObject;
        text = transform.Find("Canvas/TextOutput/Text").gameObject.GetComponent<Text>();
        background = transform.Find("Canvas/TextOutput/Background").gameObject.GetComponent<Image>();
        tip = transform.Find("Canvas/TextOutput/Tip").gameObject.GetComponent<Text>();

        int width = Screen.width / 2;
        int height = Screen.height / 2;
        
        background.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width + 40, height + 40);

        RectTransform textRect = text.gameObject.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(width, height - 50);
        textRect.localPosition = new Vector3(0, 20, 0);

        RectTransform tipRect = tip.gameObject.GetComponent<RectTransform>();
        tipRect.sizeDelta = new Vector2(width, 50);
        tipRect.localPosition = new Vector3(0, -height / 2 + 10, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            textOutput.SetActive(false);
        }
    }
}
