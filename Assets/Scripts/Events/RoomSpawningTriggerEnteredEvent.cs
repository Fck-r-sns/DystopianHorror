using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class RoomSpawningTriggerEnteredEvent : EBEvent
{

    public readonly string roomsManagerId;
    public readonly Vector3 rootOffset;
    public readonly Vector3 rootRotation;

    public RoomSpawningTriggerEnteredEvent(string roomsManagerId, Vector3 rootOffset, Vector3 rootRotation)
    {
        this.type = EBEventType.RoomSpawningTriggerEntered;
        this.roomsManagerId = roomsManagerId;
        this.rootOffset = rootOffset;
        this.rootRotation = rootRotation;
    }

}
