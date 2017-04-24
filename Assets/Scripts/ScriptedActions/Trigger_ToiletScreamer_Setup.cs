using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_ToiletScreamer_Setup : MonoBehaviour
{

    [SerializeField]
    private Door door;

    [SerializeField]
    private AudioSource metalAudioSource;

    [SerializeField]
    private AudioSource waterAudioSource;

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            isTriggered = true;
            metalAudioSource.Play();
            waterAudioSource.Play();
            door.SetOpened();
        }
    }

}
