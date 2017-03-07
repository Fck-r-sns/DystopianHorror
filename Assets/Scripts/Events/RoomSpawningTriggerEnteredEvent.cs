using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class RoomSpawningTriggerEnteredEvent : EBEvent
{

    public readonly string roomsManagerId;
    public readonly int triggerId;

    public RoomSpawningTriggerEnteredEvent(string roomsManagerId, int triggerId)
    {
        this.type = EBEventType.RoomSpawningTriggerEntered;
        this.roomsManagerId = roomsManagerId;
        this.triggerId = triggerId;
    }

}
