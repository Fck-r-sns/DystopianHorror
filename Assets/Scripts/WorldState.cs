using UnityEngine;

using EventBus;

public class WorldState : MonoBehaviour, IEventSubscriber
{

    public enum Location
    {
        Prologue,
        Hall,
        PositiveEpilogue,
        NegativeEpilogue
    }

    public const int MIN_MADNESS = 0;
    public const int MAX_MADNESS = 100;
    public const int MEDIUM_MADNESS_THRESHOLD = 30;
    public const int HIGH_MADNESS_THRESHOLD = 70;
    public const int NORMAL_TO_MAD_BOUNDARY = 50;
    public const int MADNESS_PER_ROOM_VISIT = +10;
    public const int MADNESS_PER_BOOK_COLLECTED = -20;
    public const int MADNESS_PER_MONSTER_CAUGHT = +30;
    public const float BOOK_SPAWN_INITIAL_CHANCE = 0.1f;

    private int address = AddressProvider.GetFreeAddress();
    private Location location_;
    private int roomsVisited_ = 0;
    private int madness_ = 0;
    private int collectiblesFound_ = 0;
    private bool keyFound_ = false;
    private int timesCaughtByMonster_ = 0;

    public Location location {
        get {
            return location_;
        }
    }

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

            case EBEventType.CaughtByMonster:
                ProcessCaughtByMonsterEvent(e);
                break;

            case EBEventType.PrologueEntered:
                location_ = Location.Prologue;
                break;

            case EBEventType.HallEntered:
                location_ = Location.Hall;
                break;

            case EBEventType.PositiveEpilogueEntered:
                location_ = Location.PositiveEpilogue;
                break;

            case EBEventType.NegativeEpilogueEntered:
                location_ = Location.NegativeEpilogue;
                break;
        }

        Dispatcher.SendEvent(new WorldStateChangedEvent(this));
    }

    private void Start()
    {
        // subscribe
        Dispatcher.Subscribe(EBEventType.ItemCollected, address, gameObject);
        Dispatcher.Subscribe(EBEventType.HallMovingTriggerEntered, address, gameObject);
        Dispatcher.Subscribe(EBEventType.CaughtByMonster, address, gameObject);
        Dispatcher.Subscribe(EBEventType.PrologueEntered, address, gameObject);
        Dispatcher.Subscribe(EBEventType.HallEntered, address, gameObject);
        Dispatcher.Subscribe(EBEventType.PositiveEpilogueEntered, address, gameObject);
        Dispatcher.Subscribe(EBEventType.NegativeEpilogueEntered, address, gameObject);
    }

    private void OnDestroy()
    {
        // unsubscribe
        Dispatcher.Unsubscribe(EBEventType.ItemCollected, address);
        Dispatcher.Unsubscribe(EBEventType.HallMovingTriggerEntered, address);
        Dispatcher.Unsubscribe(EBEventType.CaughtByMonster, address);
        Dispatcher.Unsubscribe(EBEventType.PrologueEntered, address);
        Dispatcher.Unsubscribe(EBEventType.HallEntered, address);
        Dispatcher.Unsubscribe(EBEventType.PositiveEpilogueEntered, address);
        Dispatcher.Unsubscribe(EBEventType.NegativeEpilogueEntered, address);
    }

    private void ProcessItemCollectedEvent(ItemCollectedEvent e)
    {
        switch (e.item.GetItemType())
        {
            case CollectibleItem.Type.Book:
                ++collectiblesFound_;
                AddMadness(MADNESS_PER_BOOK_COLLECTED);
                break;

            case CollectibleItem.Type.Key:
                keyFound_ = true;
                break;
        }
    }

    private void ProcessHallMoveinTriggerEnteredEvent(HallMovingTriggerEnteredEvent e)
    {
        ++roomsVisited_;
        AddMadness(MADNESS_PER_ROOM_VISIT);
    }

    private void ProcessCaughtByMonsterEvent(EBEvent e)
    {
        AddMadness(MADNESS_PER_MONSTER_CAUGHT);
    }

    private void AddMadness(int value)
    {
        madness_ += value;
        madness_ = Mathf.Clamp(madness_, MIN_MADNESS, MAX_MADNESS);
    }
}
