using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;
using System;

public class Hunter : MonoBehaviour, IEventSubscriber {

    private int address = AddressProvider.GetFreeAddress();
    private Queue<Waypoint> waypointsQueue = new Queue<Waypoint>();

    public void OnReceived(EBEvent e)
    {
        if (e.type == EBEventType.NewWaypointCreated)
        {
            waypointsQueue.Enqueue((e as NewWaypointEvent).waypoint);
        }
    }

    void Start () {
        Dispatcher.Subscribe(EBEventType.NewWaypointCreated, address, gameObject);
	}

    void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.NewWaypointCreated, address);
    }

    void Update () {
		
	}
}
