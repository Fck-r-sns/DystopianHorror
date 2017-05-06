using UnityEngine;

using EventBus;
using System.Collections;

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
    public const int ENDING_MADNESS_THRESHOLD = 81;
    public const int ENDING_COLLECTIBLES_FOUND_THRESHOLD = 5;
    public const int MADNESS_PER_ROOM_VISIT = +20;
    public const int MADNESS_PER_BOOK_COLLECTED = -30;
    public const int MADNESS_PER_MONSTER_CAUGHT = +60;
    public const float BOOK_SPAWN_TURNS = 7;
    public const float KEY_SPAWN_TURNS = 6;

    private Dispatcher dispatcher;
    private int address;
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

    public int timesCaughtByMonster {
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

            case EBEventType.ApplyMadnessAfterMonsterCaught:
                ProcessApplyMadnessAfterMonsterEvent(e);
                break;

            case EBEventType.PrologueEntered:
                location_ = Location.Prologue;
                break;

            case EBEventType.HallEntered:
                location_ = Location.Hall;
                break;

            case EBEventType.PositiveEpilogueEntered:
                location_ = Location.PositiveEpilogue;
                StartCoroutine(ProcessEnding());
                break;

            case EBEventType.NegativeEpilogueEntered:
                location_ = Location.NegativeEpilogue;
                StartCoroutine(ProcessEnding());
                break;
        }

        dispatcher.SendEvent(new WorldStateChangedEvent(this));
    }

    private void Start()
    {
        dispatcher = Dispatcher.GetInstance();
        address = dispatcher.GetFreeAddress();
        // subscribe
        dispatcher.Subscribe(EBEventType.ItemCollected, address, gameObject);
        dispatcher.Subscribe(EBEventType.HallMovingTriggerEntered, address, gameObject);
        dispatcher.Subscribe(EBEventType.CaughtByMonster, address, gameObject);
        dispatcher.Subscribe(EBEventType.ApplyMadnessAfterMonsterCaught, address, gameObject);
        dispatcher.Subscribe(EBEventType.PrologueEntered, address, gameObject);
        dispatcher.Subscribe(EBEventType.HallEntered, address, gameObject);
        dispatcher.Subscribe(EBEventType.PositiveEpilogueEntered, address, gameObject);
        dispatcher.Subscribe(EBEventType.NegativeEpilogueEntered, address, gameObject);
    }

    private void OnDestroy()
    {
        // unsubscribe
        dispatcher.Unsubscribe(EBEventType.ItemCollected, address);
        dispatcher.Unsubscribe(EBEventType.HallMovingTriggerEntered, address);
        dispatcher.Unsubscribe(EBEventType.CaughtByMonster, address);
        dispatcher.Unsubscribe(EBEventType.ApplyMadnessAfterMonsterCaught, address);
        dispatcher.Unsubscribe(EBEventType.PrologueEntered, address);
        dispatcher.Unsubscribe(EBEventType.HallEntered, address);
        dispatcher.Unsubscribe(EBEventType.PositiveEpilogueEntered, address);
        dispatcher.Unsubscribe(EBEventType.NegativeEpilogueEntered, address);
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
        if (!keyFound_)
        {
            AddMadness(MADNESS_PER_ROOM_VISIT);
        }
    }

    private void ProcessCaughtByMonsterEvent(EBEvent e)
    {
        ++timesCaughtByMonster_;
    }

    private void ProcessApplyMadnessAfterMonsterEvent(EBEvent e)
    {
        AddMadness(MADNESS_PER_MONSTER_CAUGHT);
    }

    private void AddMadness(int value)
    {
        madness_ += value;
        madness_ = Mathf.Clamp(madness_, MIN_MADNESS, MAX_MADNESS);
    }

    private IEnumerator ProcessEnding()
    {
        yield return null;
        dispatcher.Unsubscribe(EBEventType.ItemCollected, address);
        dispatcher.Unsubscribe(EBEventType.HallMovingTriggerEntered, address);
        dispatcher.Unsubscribe(EBEventType.CaughtByMonster, address);
        dispatcher.Unsubscribe(EBEventType.ApplyMadnessAfterMonsterCaught, address);
        dispatcher.Unsubscribe(EBEventType.PrologueEntered, address);
        dispatcher.Unsubscribe(EBEventType.HallEntered, address);
        dispatcher.Unsubscribe(EBEventType.PositiveEpilogueEntered, address);
        dispatcher.Unsubscribe(EBEventType.NegativeEpilogueEntered, address);
    }
}
