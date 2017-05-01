using UnityEngine;
using UnityStandardAssets.CinematicEffects;

public class GameSettings : MonoBehaviour
{
    [SerializeField]
    private AmbientOcclusion[] ambientOcclusions;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _brightness = 0.25f;

    [SerializeField]
    private bool _ambientOcclusionEnabled = true;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _volume = 1.0f;

    public float brightness {
        get {
            return _brightness;
        }
        set {
            _brightness = value;
            RenderSettings.ambientLight = new Color(_brightness, _brightness, _brightness);
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
        }
    }

    public float volume {
        get {
            return _volume;
        }
        set {
            _volume = value;
            AudioListener.volume = _volume;
        }
    }

    private void Start()
    {
        brightness = brightness;
        ambientOcclusionEnabled = ambientOcclusionEnabled;
        volume = volume;
    }

}
