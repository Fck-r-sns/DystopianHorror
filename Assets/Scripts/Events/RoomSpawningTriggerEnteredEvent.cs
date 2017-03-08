using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class RoomSpawningTriggerEnteredEvent : EBEvent
{

    public readonly string roomsManagerId;
    public readonly int roomEntryId;

    public RoomSpawningTriggerEnteredEvent(string roomsManagerId, int roomEntryId)
    {
        this.type = EBEventType.RoomSpawningTriggerEntered;
        this.roomsManagerId = roomsManagerId;
        this.roomEntryId = roomEntryId;
    }

}
