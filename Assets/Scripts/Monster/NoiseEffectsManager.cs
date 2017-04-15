using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VideoGlitches;
using EventBus;
using System;

[RequireComponent(typeof(VideoGlitchShift))]
[RequireComponent(typeof(VideoGlitchNoiseDigital))]
[RequireComponent(typeof(VideoGlitchSpectrumOffset))]
public class NoiseEffectsManager : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private Transform monster;

    [SerializeField]
    private bool enableShift = true;

    [SerializeField]
    private AnimationCurve shiftIntensityToDistance = new AnimationCurve(new Keyframe(70.0f, 0.25f), new Keyframe(0.0f, 1.0f));

    [SerializeField]
    private float shiftMaxIntensity = 0.1f;

    [SerializeField]
    private bool enableNoise = true;

    [SerializeField]
    private AnimationCurve noiseIntensityToDistance = new AnimationCurve(new Keyframe(70.0f, 0.25f), new Keyframe(0.0f, 1.0f));

    [SerializeField]
    private float noiseMaxIntensity = 0.1f;

    [SerializeField]
    private bool enableSpectrumOffset = true;

    [SerializeField]
    private AnimationCurve spectrumOffsetIntensityToDistance = new AnimationCurve(new Keyframe(70.0f, 0.25f), new Keyframe(0.0f, 1.0f));

    [SerializeField]
    private float spectrumOffsetMaxIntensity = 0.1f;

    private VideoGlitchShift shift;
    private VideoGlitchNoiseDigital noise;
    private VideoGlitchSpectrumOffset spectrumOffset;
    private int address = AddressProvider.GetFreeAddress();
    private bool monsterInFrustum = false;
    private bool monsterInPlainSight = false;

    public void SetMonster(Transform monster)
    {
        this.monster = monster;
    }

    public void OnReceived(EBEvent e)
    {
        switch (e.type)
        {
            case EBEventType.MonsterInFrustum:
                monsterInFrustum = true;
                UpdateEffects();
                break;

            case EBEventType.MonsterOutOfFrustum:
                monsterInFrustum = false;
                UpdateEffects();
                break;

            case EBEventType.MonsterInPlainSight:
                monsterInPlainSight = true;
                UpdateEffects();
                break;

            case EBEventType.MonsterOutOfPlainSight:
                monsterInPlainSight = false;
                UpdateEffects();
                break;
        }
    }

    void Start()
    {
        shift = GetComponent<VideoGlitchShift>();
        noise = GetComponent<VideoGlitchNoiseDigital>();
        spectrumOffset = GetComponent<VideoGlitchSpectrumOffset>();

        UpdateEffects();

        Dispatcher.Subscribe(EBEventType.MonsterInFrustum, address, gameObject);
        Dispatcher.Subscribe(EBEventType.MonsterOutOfFrustum, address, gameObject);
        Dispatcher.Subscribe(EBEventType.MonsterInPlainSight, address, gameObject);
        Dispatcher.Subscribe(EBEventType.MonsterOutOfPlainSight, address, gameObject);
    }

    void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.MonsterInFrustum, address);
        Dispatcher.Unsubscribe(EBEventType.MonsterOutOfFrustum, address);
        Dispatcher.Unsubscribe(EBEventType.MonsterInPlainSight, address);
        Dispatcher.Unsubscribe(EBEventType.MonsterOutOfPlainSight, address);
    }

    void Update()
    {
        float dst = Mathf.Abs(transform.position.x - monster.position.x) + Mathf.Abs(transform.position.z - monster.position.z); // manhattan distance
        shift.amplitude = shiftIntensityToDistance.Evaluate(dst) * shiftMaxIntensity;
        noise.threshold = noiseIntensityToDistance.Evaluate(dst) * noiseMaxIntensity;
        spectrumOffset.strength = spectrumOffsetIntensityToDistance.Evaluate(dst) * spectrumOffsetMaxIntensity;
    }

    private void UpdateEffects()
    {
        shift.enabled = enableShift && monsterInFrustum;
        noise.enabled = enableNoise && monsterInPlainSight;
        spectrumOffset.enabled = enableSpectrumOffset && monsterInFrustum && monsterInPlainSight;
    }

}
