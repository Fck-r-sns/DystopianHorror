using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class DoorTrigger_Sender : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        Dispatcher.SendEvent(new EBEvent { type = EBEventType.TestDoorTriggerEntered });
    }

    void OnTriggerExit(Collider other)
    {
        Dispatcher.SendEvent(new EBEvent { type = EBEventType.TestDoorTriggerExited });
    }
}
