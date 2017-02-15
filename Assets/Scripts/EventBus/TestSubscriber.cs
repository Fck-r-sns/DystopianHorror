using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;

public class TestSubscriber : MonoBehaviour, EventBus.IEventSubscriber {

    public void OnReceived(EventBus.Event e)
    {
        Debug.Log("Event received: " + e);
    }

    // Use this for initialization
    void Start()
    {
        EventBus.Dispatcher.Subscribe(EventBus.EventType.TestEvent, 42, gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
