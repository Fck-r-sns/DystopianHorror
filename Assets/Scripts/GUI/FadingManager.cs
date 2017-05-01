using UnityEngine;
using UnityEngine.UI;

public class FadingManager : MonoBehaviour
{

    public enum State
    {
        Normal,
        FadeIn,
        Faded,
        FadeOut
    }

    [SerializeField]
    private RawImage blackFading;

    [SerializeField]
    private RawImage whiteFading;

    private static FadingManager instance;
    private State state = State.Normal;
    private RawImage currentFading;
    private float alpha = 1.0f;
    private float fadingTime = 1.0f;

    public static FadingManager GetInstance()
    {
        return instance;
    }

    public State GetState()
    {
        return state;
    }

    public void FadeToBlack(float fadingTime)
    {
        this.fadingTime = fadingTime;
        currentFading = blackFading;
        state = State.FadeIn;
    }

    public void FadeToWhite(float fadingTime)
    {
        this.fadingTime = fadingTime;
        currentFading = whiteFading;
        state = State.FadeIn;
    }

    public void FadeToNormal(float fadingTime)
    {
        this.fadingTime = fadingTime;
        state = State.FadeOut;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentFading = blackFading;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Normal:
                break;

            case State.FadeIn:
                {
                    alpha += Time.deltaTime / fadingTime;
                    alpha = Mathf.Clamp01(alpha);
                    Color color = currentFading.color;
                    color.a = alpha;
                    currentFading.color = color;
                    if (alpha >= 1.0f)
                    {
                        state = State.Faded;
                    }
                }
                break;

            case State.Faded:
                break;

            case State.FadeOut:
                {
                    alpha -= Time.deltaTime / fadingTime;
                    alpha = Mathf.Clamp01(alpha);
                    Color color = currentFading.color;
                    color.a = alpha;
                    currentFading.color = color;
                    if (alpha <= 0.0f)
                    {
                        state = State.Normal;
                    }
                }
                break;
        }
    }

}
