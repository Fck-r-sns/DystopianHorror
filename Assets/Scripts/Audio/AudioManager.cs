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
    private float crossFadeRate = 1.5f;

    [SerializeField]
    private AudioClip prologueMusic;

    [SerializeField]
    private AudioClip coreLowMadnessMusic;

    [SerializeField]
    private AudioClip coreMediumMadnessMusic;

    [SerializeField]
    private AudioClip coreHighMadnessMusic;

    [SerializeField]
    private AudioClip badEpilogueMusic;

    [SerializeField]
    private AudioClip goodEpilogueMusic;

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
        }
    }

    void Start () {
        musicSource1.volume = maxVolume;
        musicSource2.volume = maxVolume;
        sources[0] = musicSource1;
        sources[1] = musicSource2;

        Dispatcher.Subscribe(EBEventType.ItemCollected, address, gameObject);

        //changeAudio(prologueMusic);
    }

    void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.ItemCollected, address);
    }

    void Update () {
		
	}

    void changeAudio(AudioClip clip)
    {
        if (sources[currentSourceIndex].isPlaying)
        {
            StartCoroutine(changeAudioWithFading(clip));
        }
        else
        {
            currentSourceIndex = 1 - currentSourceIndex;
            AudioSource s = sources[currentSourceIndex];
            s.clip = clip;
            s.Play();
        }
    }

    IEnumerator changeAudioWithFading(AudioClip clip)
    {
        AudioSource activeSource = sources[currentSourceIndex];
        AudioSource inactiveSource = sources[1 - currentSourceIndex];

        inactiveSource.volume = 0;
        inactiveSource.clip = clip;
        inactiveSource.Play();

        float scaledRate = crossFadeRate * maxVolume;
        while (activeSource.volume > 0)
        {
            float delta = scaledRate * Time.deltaTime;
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
}
