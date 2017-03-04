using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class NewWaypointEvent : EBEvent {

    public readonly Waypoint waypoint;
    
    public NewWaypointEvent(Vector3 position, float time)
    {
        this.type = EBEventType.NewWaypointCreated;
        this.waypoint = new Waypoint(position, time);
    }

    public NewWaypointEvent(Waypoint waypoint)
    {
        this.type = EBEventType.NewWaypointCreated;
        this.waypoint = waypoint;
    }

}
