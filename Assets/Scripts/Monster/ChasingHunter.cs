using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class ChasingHunter : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private float movePeriod_s = 2.0f;

    [SerializeField]
    private bool ignoreStanding = true;

    [SerializeField]
    private float standingDistance = 0.2f;

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

    void Start()
    {
        Dispatcher.Subscribe(EBEventType.NewWaypointCreated, address, gameObject);
        lastMoveTime = Time.time;
    }

    void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.NewWaypointCreated, address);
    }

    void Update()
    {
        if (Time.time - lastMoveTime > movePeriod_s)
        {
            Waypoint wp = GetNextWaypoint();
            if (wp != null)
            {
                MoveToNextWaypoint(wp);
            }
            lastMoveTime += movePeriod_s;
        }
    }

    private void MoveToNextWaypoint(Waypoint waypoint)
    {
        transform.position = waypoint.position;
    }

    private Waypoint GetNextWaypoint()
    {
        Waypoint res = null;
        while (res == null)
        {
            if (waypointsQueue.Count == 0)
            {
                break;
            }
            Waypoint wp = waypointsQueue.Dequeue();
            if (Vector3.Distance(transform.position, wp.position) > standingDistance)
            {
                res = wp;
            }
        }
        return res;
    }
}
