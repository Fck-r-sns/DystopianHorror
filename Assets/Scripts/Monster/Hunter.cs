using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;
using System;

public class Hunter : MonoBehaviour, IEventSubscriber {

    [SerializeField]
    private float movePeriod_s = 2.0f;

    private int address = AddressProvider.GetFreeAddress();
    private Queue<Waypoint> waypointsQueue = new Queue<Waypoint>();

    private float lastMoveTime = 0.0f;

    public void OnReceived(EBEvent e)
    {
        if (e.type == EBEventType.NewWaypointCreated)
        {
            waypointsQueue.Enqueue((e as NewWaypointEvent).waypoint);
        }
    }

    void Start () {
        Dispatcher.Subscribe(EBEventType.NewWaypointCreated, address, gameObject);
        lastMoveTime = Time.time;
	}

    void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.NewWaypointCreated, address);
    }

    void Update () {
		if (Time.time - lastMoveTime > movePeriod_s)
        {
            MoveToNextWaypoint();
            lastMoveTime += movePeriod_s;
        }
	}

    private void MoveToNextWaypoint()
    {
        if (waypointsQueue.Count != 0)
        {
            transform.position = waypointsQueue.Dequeue().position;
        }
    }
}
