using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class TestPublisher : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        EventBus.Dispatcher.SendEvent(
            new EventBus.Event
            {
                type = EventBus.EventType.TestEvent,
                address = Defines.BROADCAST_ADDRESS
            }
            );
    }
}