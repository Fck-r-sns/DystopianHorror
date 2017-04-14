using EventBus;

public class ItemCollectedEvent : EBEvent
{
    public readonly CollectibleItem item;

    public ItemCollectedEvent(CollectibleItem item)
    {
        this.type = EBEventType.ItemCollected;
        this.item = item;
    }
}
