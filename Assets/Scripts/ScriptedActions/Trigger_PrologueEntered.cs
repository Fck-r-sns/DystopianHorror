using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class Trigger_PrologueEntered : MonoBehaviour
{
    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            triggered = true;
            Dispatcher.GetInstance().SendEvent(new EBEvent() { type = EBEventType.PrologueEntered });
        }
    }
}
