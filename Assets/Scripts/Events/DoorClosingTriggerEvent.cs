using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class DoorClosingTriggerEvent : EBEvent
{

    public readonly int roomEntryId;
    public readonly TriggerAction action;

    public DoorClosingTriggerEvent(int roomEntryId, TriggerAction action)
    {
        this.type = EBEventType.DoorClosingTrigger;
        this.roomEntryId = roomEntryId;
        this.action = action;
    }

}
