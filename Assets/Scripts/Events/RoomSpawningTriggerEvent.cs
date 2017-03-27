using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class RoomSpawningTriggerEvent : EBEvent
{

    public enum Action
    {
        Enter,
        Exit
    }

    public readonly string roomsManagerId;
    public readonly int roomEntryId;
    public readonly Action action;

    public RoomSpawningTriggerEvent(string roomsManagerId, int roomEntryId, Action action)
    {
        this.type = EBEventType.RoomSpawningTrigger;
        this.roomsManagerId = roomsManagerId;
        this.roomEntryId = roomEntryId;
        this.action = action;
    }

}
