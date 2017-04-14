using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class DoorInteractionEvent : EBEvent
{

    public readonly Door door;
    public readonly Immersive.DoorControl doorController;

    public DoorInteractionEvent(Door door, Immersive.DoorControl doorController)
    {
        this.type = EBEventType.InteractionWithDoor;
        this.door = door;
        this.doorController = doorController;
    }
}
