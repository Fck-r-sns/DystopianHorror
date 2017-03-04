using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class NewWaypointEvent : EBEvent {

    public readonly Vector3 position;
    public readonly float time;
    
    public NewWaypointEvent(Vector3 position, float time)
    {
        this.type = EBEventType.NewWaypointCreated;
        this.position = position;
        this.time = time;
    }

}
