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

    private VideoGlitchShift shift;
    private VideoGlitchNoiseDigital noise;
    private VideoGlitchSpectrumOffset spectrumOffset;
    private int address = AddressProvider.GetFreeAddress();
    private bool monsterInFrustum = false;
    private bool monsterInPlainSight = false;

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

    // Use this for initialization
    void Start()
    {
        shift = GetComponent<VideoGlitchShift>();
        noise = GetComponent<VideoGlitchNoiseDigital>();
        spectrumOffset = GetComponent<VideoGlitchSpectrumOffset>();

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

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateEffects()
    {
        shift.enabled = monsterInFrustum;
        noise.enabled = monsterInPlainSight;
        spectrumOffset.enabled = monsterInFrustum && monsterInPlainSight;
    }

}
