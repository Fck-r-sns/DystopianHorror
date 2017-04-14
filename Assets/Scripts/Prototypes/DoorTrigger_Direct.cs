using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger_Direct : MonoBehaviour {

    [SerializeField]
    Door door;

    void OnTriggerEnter(Collider other)
    {
        door.Open();
    }

    void OnTriggerExit(Collider other)
    {
        door.Close();
    }
}
