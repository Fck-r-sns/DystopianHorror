using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using EventBus;
using System;

public class DoorTrigger_Receiver : MonoBehaviour, IEventSubscriber {

    private Door door;
    private int address = AddressProvider.GetFreeAddress();

    public void OnReceived(EBEvent e)
    {
        switch (e.type)
        {
            case EBEventType.TestDoorTriggerEntered:
                door.open();
                break;
            case EBEventType.TestDoorTriggerExited:
                door.close();
                break;
        }
    }

    // Use this for initialization
    void Start () {
        door = GetComponent<Door>();
        Dispatcher.Subscribe(EBEventType.TestDoorTriggerEntered, address, gameObject);
        Dispatcher.Subscribe(EBEventType.TestDoorTriggerExited, address, gameObject);
    }
}
