using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;
using System;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour, IEventSubscriber
{

    // 2 sources for crossfade
    [SerializeField]
    private AudioSource source1;

    [SerializeField]
    private AudioSource source2;

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


    private AudioSource[] sources = new AudioSource[2];
    private int address = AddressProvider.GetFreeAddress();
    private int currentSourceIndex = 0;

    public void OnReceived(EBEvent e)
    {
        
    }

    void Start () {
        source1.volume = maxVolume;
        source2.volume = maxVolume;
        sources[0] = source1;
        sources[1] = source2;
        changeAudio(prologueMusic);
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
}
