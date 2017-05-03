using EventBus;

public class RoomSpawningTriggerEvent : EBEvent
{

    public readonly int roomEntryId;
    public readonly TriggerAction action;

    public RoomSpawningTriggerEvent(int roomEntryId, TriggerAction action)
    {
        this.type = EBEventType.RoomSpawningTrigger;
        this.roomEntryId = roomEntryId;
        this.action = action;
    }

}
