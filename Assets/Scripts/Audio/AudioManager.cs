using System.Collections;
using UnityEngine;

using EventBus;
using System;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour, IEventSubscriber
{

    // 2 music sources for crossfade
    [SerializeField]
    private AudioSource musicSource1;

    [SerializeField]
    private AudioSource musicSource2;

    [SerializeField]
    private AudioSource interfaceSoundSource;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float maxVolume;

    [SerializeField]
    private float crossFadeTime = 2.0f;

    [SerializeField]
    private AudioClip prologueMusic;

    [SerializeField]
    private AudioClip coreLowMadnessMusic;

    [SerializeField]
    private AudioClip coreMediumMadnessMusic;

    [SerializeField]
    private AudioClip coreHighMadnessMusic;

    [SerializeField]
    private AudioClip positiveEpilogueMusic;

    [SerializeField]
    private AudioClip negativeEpilogueMusic;

    [SerializeField]
    private AudioClip grabBookSound;

    [SerializeField]
    private AudioClip grabKeySound;

    private AudioSource[] sources = new AudioSource[2];
    private int address = AddressProvider.GetFreeAddress();
    private int currentSourceIndex = 0;

    public void OnReceived(EBEvent e)
    {
        switch (e.type)
        {
            case EBEventType.ItemCollected:
                ProcessItemCollectedEvent(e as ItemCollectedEvent);
                break;

            case EBEventType.WorldStateChanged:
                ProcessWorldStateChangedEvent(e as WorldStateChangedEvent);
                break;
        }
    }

    void Start()
    {
        musicSource1.volume = maxVolume;
        musicSource2.volume = maxVolume;
        sources[0] = musicSource1;
        sources[1] = musicSource2;

        Dispatcher.Subscribe(EBEventType.ItemCollected, address, gameObject);
        Dispatcher.Subscribe(EBEventType.WorldStateChanged, address, gameObject);
    }

    void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.ItemCollected, address);
        Dispatcher.Unsubscribe(EBEventType.WorldStateChanged, address);
    }

    void Update()
    {

    }

    void ChangeAudio(AudioClip clip)
    {
        if (sources[currentSourceIndex].isPlaying)
        {
            StartCoroutine(ChangeAudioWithFading(clip));
        }
        else
        {
            currentSourceIndex = 1 - currentSourceIndex;
            AudioSource s = sources[currentSourceIndex];
            s.clip = clip;
            s.Play();
        }
    }

    IEnumerator ChangeAudioWithFading(AudioClip clip)
    {
        AudioSource activeSource = sources[currentSourceIndex];
        AudioSource inactiveSource = sources[1 - currentSourceIndex];

        inactiveSource.volume = 0;
        inactiveSource.clip = clip;
        inactiveSource.Play();

        float crossFadeRate = maxVolume / crossFadeTime;
        while (activeSource.volume > 0)
        {
            float delta = crossFadeRate * Time.deltaTime;
            activeSource.volume -= delta;
            inactiveSource.volume += delta;
            inactiveSource.volume = Mathf.Min(inactiveSource.volume, maxVolume);
            yield return null;
        }

        currentSourceIndex = 1 - currentSourceIndex;
    }

    private void ProcessItemCollectedEvent(ItemCollectedEvent e)
    {
        switch (e.item.GetItemType())
        {
            case CollectibleItem.Type.Book:
                interfaceSoundSource.PlayOneShot(grabBookSound);
                break;

            case CollectibleItem.Type.Key:
                interfaceSoundSource.PlayOneShot(grabKeySound);
                break;
        }
    }

    private void ProcessWorldStateChangedEvent(WorldStateChangedEvent e)
    {
        WorldState world = e.worldState;

        if ((world.location == WorldState.Location.Prologue)
            && (sources[currentSourceIndex].clip != prologueMusic))
        {
            ChangeAudio(prologueMusic);
            return;
        }

        if ((world.location == WorldState.Location.Hall)
            && (world.madness < WorldState.MEDIUM_MADNESS_THRESHOLD)
            && (sources[currentSourceIndex].clip != coreLowMadnessMusic))
        {
            ChangeAudio(coreLowMadnessMusic);
            return;
        }

        if ((world.location == WorldState.Location.Hall)
            && ((WorldState.MEDIUM_MADNESS_THRESHOLD <= world.madness) && (world.madness < WorldState.HIGH_MADNESS_THRESHOLD))
            && (sources[currentSourceIndex].clip != coreMediumMadnessMusic))
        {
            ChangeAudio(coreMediumMadnessMusic);
            return;
        }

        if ((world.location == WorldState.Location.Hall)
            && (WorldState.HIGH_MADNESS_THRESHOLD <= world.madness)
            && (sources[currentSourceIndex].clip != coreHighMadnessMusic))
        {
            ChangeAudio(coreHighMadnessMusic);
            return;
        }

        if ((world.location == WorldState.Location.PositiveEpilogue)
            && (sources[currentSourceIndex].clip != positiveEpilogueMusic))
        {
            ChangeAudio(positiveEpilogueMusic);
            return;
        }

        if ((world.location == WorldState.Location.NegativeEpilogue)
            && (sources[currentSourceIndex].clip != negativeEpilogueMusic))
        {
            ChangeAudio(negativeEpilogueMusic);
            return;
        }

    }
}
