using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFading : MonoBehaviour
{

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private Texture2D blackFading;

    [SerializeField]
    private Texture2D whiteFading;

    [SerializeField]
    private float fadingTime = 1.0f;

    private enum State
    {
        Normal,
        FadeIn,
        FadeOut
    }

    private State state = State.Normal;
    private Texture2D currentTexture;
    private float alpha = 0.0f;

    public void FadeToBlack()
    {
        currentTexture = blackFading;
        state = State.FadeIn;
    }

    public void FadeToWhite()
    {
        currentTexture = whiteFading;
        state = State.FadeIn;
    }

    public void FadeToNormal()
    {
        state = State.FadeOut;
    }

    private void OnGUI()
    {
        switch (state)
        {
            case State.Normal:
                break;

            case State.FadeIn:
                alpha += Time.deltaTime / fadingTime;
                alpha = Mathf.Clamp01(alpha);
                Color color = GUI.color;
                color.a = alpha;
                GUI.color = color;
                GUI.DrawTexture(camera.pixelRect, currentTexture);
                break;

            case State.FadeOut:
                break;
        }
    }

}
