using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class HallMovingTriggerEnteredEvent : EBEvent
{

    public readonly int frameNumber;
    public readonly Vector3 oldPosition;
    public readonly Vector3 oldRotation;
    public readonly Vector3 newPosition;
    public readonly Vector3 newRotation;

    public HallMovingTriggerEnteredEvent(int frameNumber, Vector3 oldPosition, Vector3 oldRotation, Vector3 newPosition, Vector3 newRotation)
    {
        this.type = EBEventType.HallMovingTriggerEntered;
        this.frameNumber = frameNumber;
        this.oldPosition = oldPosition;
        this.oldRotation = oldRotation;
        this.newPosition = newPosition;
        this.newRotation = newRotation;
    }
}
