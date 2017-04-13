using UnityEngine;

using EventBus;

public class WorldState : MonoBehaviour, IEventSubscriber
{
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
