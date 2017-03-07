using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class HallMovingTriggerEnteredEvent : EBEvent
{

    public Vector3 oldPosition;
    public Vector3 oldRotation;
    public Vector3 newPosition;
    public Vector3 newRotation;

    public HallMovingTriggerEnteredEvent(Vector3 oldPosition, Vector3 oldRotation, Vector3 newPosition, Vector3 newRotation)
    {
        this.type = EBEventType.HallMovingTriggerEntered;
        this.oldPosition = oldPosition;
        this.oldRotation = oldRotation;
        this.newPosition = newPosition;
        this.newRotation = newRotation;
    }
}
