using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class DoorClosingTriggerEvent : EBEvent
{

    public readonly string roomsManagerId;
    public readonly int roomEntryId;
    public readonly TriggerAction action;

    public DoorClosingTriggerEvent(string roomsManagerId, int roomEntryId, TriggerAction action)
    {
        this.type = EBEventType.DoorClosingTrigger;
        this.roomsManagerId = roomsManagerId;
        this.roomEntryId = roomEntryId;
        this.action = action;
    }

}
