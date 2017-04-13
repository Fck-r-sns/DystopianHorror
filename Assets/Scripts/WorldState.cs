using UnityEngine;

using EventBus;

public class WorldState : MonoBehaviour, IEventSubscriber
{

    public const int MIN_MADNESS = 0;
    public const int MAX_MADNESS = 100;
    public const float BOOK_SPAWN_INITIAL_CHANCE = 0.1f;

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
        // process events
    }

    private void Start()
    {
        // subscribe
    }

    private void OnDestroy()
    {
        // unsubscribe
    }
}
