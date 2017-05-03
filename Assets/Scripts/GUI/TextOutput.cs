using UnityEngine;
using UnityEngine.UI;

public class TextOutput : MonoBehaviour
{

    private GameObject textOutput;
    private Text text;
    private Image background;

    public void ShowText(string text)
    {
        this.text.text = text;
        textOutput.SetActive(true);
    }

    public bool IsActive()
    {
        return textOutput.activeSelf;
    }

    private void Start()
    {
        textOutput = transform.Find("Canvas/TextOutput").gameObject;
        text = transform.Find("Canvas/TextOutput/Text").gameObject.GetComponent<Text>();
        background = transform.Find("Canvas/TextOutput/Background").gameObject.GetComponent<Image>();

        int width = Screen.width / 2;
        int height = Screen.height / 2;
        text.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        background.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width + 40, height + 40);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            textOutput.SetActive(false);
        }
    }
}
