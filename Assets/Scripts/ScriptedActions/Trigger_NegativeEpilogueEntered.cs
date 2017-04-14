using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class Trigger_NegativeEpilogueEntered : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Dispatcher.SendEvent(new EBEvent() { type = EBEventType.NegativeEpilogueEntered });
    }
}
