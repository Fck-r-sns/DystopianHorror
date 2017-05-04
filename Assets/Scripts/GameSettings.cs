using UnityEngine;
using UnityStandardAssets.CinematicEffects;

public class GameSettings : MonoBehaviour
{
    [SerializeField]
    private AmbientOcclusion[] ambientOcclusions;

    [SerializeField]
    private FirstPersonController controller;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _brightness = 0.25f;

    [SerializeField]
    private bool _ambientOcclusionEnabled = true;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _volume = 1.0f;

    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float _mouseSensitivity = 2.0f;

    public float brightness {
        get {
            return _brightness;
        }
        set {
            _brightness = value;
            RenderSettings.ambientLight = new Color(_brightness, _brightness, _brightness);
            PlayerPrefs.SetFloat("brightness", _brightness);
        }
    }

    public bool ambientOcclusionEnabled {
        get {
            return _ambientOcclusionEnabled;
        }
        set {
            _ambientOcclusionEnabled = value;
            foreach (AmbientOcclusion ao in ambientOcclusions)
            {
                ao.enabled = _ambientOcclusionEnabled;
            }
            PlayerPrefs.SetInt("ao", _ambientOcclusionEnabled ? 1 : 0);
        }
    }

    public float volume {
        get {
            return _volume;
        }
        set {
            _volume = value;
            AudioListener.volume = _volume;
            PlayerPrefs.SetFloat("volume", _volume);
        }
    }

    public float mouseSensitivity {
        get {
            return _mouseSensitivity;
        }
        set {
            _mouseSensitivity = value;
            controller.SetMouseSensitivity(_mouseSensitivity);
            PlayerPrefs.SetFloat("sensitivity", _mouseSensitivity);
        }
    }

    public void Save()
    {
        PlayerPrefs.Save();
    }

    private void Start()
    {
        brightness = PlayerPrefs.GetFloat("brightness", brightness);
        ambientOcclusionEnabled = PlayerPrefs.GetInt("ao", ambientOcclusionEnabled ? 1 : 0) > 0;
        volume = PlayerPrefs.GetFloat("volume", volume);
        mouseSensitivity = PlayerPrefs.GetFloat("sensitivity", mouseSensitivity);
    }

}
