using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_ToiletScreamer_Punchline : MonoBehaviour
{

    [SerializeField]
    private Door door;

    [SerializeField]
    private AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        door.Close();
        audioSource.Stop();
    }

}
