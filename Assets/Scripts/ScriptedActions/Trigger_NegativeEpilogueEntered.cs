using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class Trigger_NegativeEpilogueEntered : MonoBehaviour
{

    [SerializeField]
    private Door door;

    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            triggered = true;
            door.Close();
            door.Lock();
            Dispatcher.SendEvent(new EBEvent() { type = EBEventType.NegativeEpilogueEntered });
        }
    }
}
