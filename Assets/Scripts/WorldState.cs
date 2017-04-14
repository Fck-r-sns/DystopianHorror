using UnityEngine;

using EventBus;

public class WorldState : MonoBehaviour, IEventSubscriber
{

    public const int MIN_MADNESS = 0;
    public const int MAX_MADNESS = 100;
    public const int MADNESS_PER_ROOM_VISIT = +5;
    public const int MADNESS_PER_BOOK_COLLECTED = -10;
    public const int MADNESS_PER_MONSTER_CAUGHT = +50;
    public const float BOOK_SPAWN_INITIAL_CHANCE = 0.1f;

    private int address = AddressProvider.GetFreeAddress();
    private int roomsVisited_ = 0;
    private int madness_ = 0;
    private int collectiblesFound_ = 0;
    private bool keyFound_ = false;
    private int timesCaughtByMonster_ = 0;

    public int roomsVisited {
        get {
            return roomsVisited_;
        }
    }

    public int madness {
        get {
            return madness_;
        }
    }

    public int collectiblesFound {
        get {
            return collectiblesFound_;
        }
    }

    public bool keyFound {
        get {
            return keyFound_;
        }
    }

    public int timeCaughtByMonster {
        get {
            return timesCaughtByMonster_;
        }
    }

    public void OnReceived(EBEvent e)
    {
        switch (e.type)
        {
            case EBEventType.ItemCollected:
                ProcessItemCollectedEvent(e as ItemCollectedEvent);
                break;

            case EBEventType.HallMovingTriggerEntered:
                ProcessHallMoveinTriggerEnteredEvent(e as HallMovingTriggerEnteredEvent);
                break;
        }
    }

    private void Start()
    {
        // subscribe
        Dispatcher.Subscribe(EBEventType.ItemCollected, address, gameObject);
        Dispatcher.Subscribe(EBEventType.HallMovingTriggerEntered, address, gameObject);
        Dispatcher.Subscribe(EBEventType.CaughtByMonster, address, gameObject);
    }

    private void OnDestroy()
    {
        // unsubscribe
        Dispatcher.Unsubscribe(EBEventType.ItemCollected, address);
        Dispatcher.Unsubscribe(EBEventType.HallMovingTriggerEntered, address);
        Dispatcher.Unsubscribe(EBEventType.CaughtByMonster, address);
    }

    private void ProcessItemCollectedEvent(ItemCollectedEvent e)
    {
        switch (e.item.GetItemType())
        {
            case CollectibleItem.Type.Book:
                ++collectiblesFound_;
                madness_ += MADNESS_PER_BOOK_COLLECTED;
                break;
            case CollectibleItem.Type.Key:
                keyFound_ = true;
                break;
        }
    }

    private void ProcessHallMoveinTriggerEnteredEvent(HallMovingTriggerEnteredEvent e)
    {
        ++roomsVisited_;
        madness_ += MADNESS_PER_ROOM_VISIT;
    }

    private void ProcessCaughtByMonsterEvent(EBEvent e)
    {
        madness_ += MADNESS_PER_MONSTER_CAUGHT;
    }
}
