using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFading : MonoBehaviour
{

    public enum State
    {
        Normal,
        FadeIn,
        Faded,
        FadeOut
    }

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private Texture2D blackFading;

    [SerializeField]
    private Texture2D whiteFading;

    private State state = State.Normal;
    private Texture2D currentTexture;
    private float alpha = 0.0f;
    private float fadingTime = 1.0f;

    public State GetState()
    {
        return state;
    }

    public void FadeToBlack(float fadingTime)
    {
        this.fadingTime = fadingTime;
        currentTexture = blackFading;
        state = State.FadeIn;
    }

    public void FadeToWhite(float fadingTime)
    {
        this.fadingTime = fadingTime;
        currentTexture = whiteFading;
        state = State.FadeIn;
    }

    public void FadeToNormal(float fadingTime)
    {
        this.fadingTime = fadingTime;
        state = State.FadeOut;
    }

    private void OnGUI()
    {
        switch (state)
        {
            case State.Normal:
                break;

            case State.FadeIn:
                {
                    alpha += Time.deltaTime / fadingTime;
                    alpha = Mathf.Clamp01(alpha);
                    Color color = GUI.color;
                    color.a = alpha;
                    GUI.color = color;
                    GUI.DrawTexture(camera.pixelRect, currentTexture);
                    if (alpha >= 1.0f)
                    {
                        state = State.Faded;
                    }
                }
                break;

            case State.Faded:
                GUI.DrawTexture(camera.pixelRect, currentTexture);
                break;

            case State.FadeOut:
                {
                    alpha -= Time.deltaTime / fadingTime;
                    alpha = Mathf.Clamp01(alpha);
                    Color color = GUI.color;
                    color.a = alpha;
                    GUI.color = color;
                    GUI.DrawTexture(camera.pixelRect, currentTexture);
                    if (alpha <= 0.0f)
                    {
                        state = State.Normal;
                    }
                }
                break;
        }
    }

}
