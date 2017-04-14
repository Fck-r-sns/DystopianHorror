using EventBus;

public class WorldStateChangedEvent : EBEvent
{
    public readonly WorldState worldState;

    public WorldStateChangedEvent(WorldState worldState)
    {
        this.type = EBEventType.WorldStateChanged;
        this.worldState = worldState;
    }
}
