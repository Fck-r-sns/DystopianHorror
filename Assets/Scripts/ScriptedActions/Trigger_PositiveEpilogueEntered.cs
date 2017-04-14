using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class Trigger_PositiveEpilogueEntered : MonoBehaviour
{
    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            triggered = true;
            Dispatcher.SendEvent(new EBEvent() { type = EBEventType.PositiveEpilogueEntered });
        }
    }
}
