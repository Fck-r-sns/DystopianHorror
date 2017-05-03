using UnityEngine;

using EventBus;
using System.Collections;

public class Trigger_EnableMonster : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private GameObject monster;

    [SerializeField]
    private SchoolBellsManager schoolBellsManager;

    private const int ROOMS_VISITED_BEFORE_MONSTER_APPEARS = 1;

    private static bool isTriggered;
    private int address = AddressProvider.GetFreeAddress();
    private int roomsVisited = 0;

    public void OnReceived(EBEvent e)
    {
        if (e.type == EBEventType.WorldStateChanged)
        {
            WorldStateChangedEvent wsce = e as WorldStateChangedEvent;
            roomsVisited = wsce.worldState.roomsVisited;
        }
    }

    private void Start()
    {
        Dispatcher.Subscribe(EBEventType.WorldStateChanged, address, gameObject);
    }

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.WorldStateChanged, address);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && roomsVisited >= ROOMS_VISITED_BEFORE_MONSTER_APPEARS)
        {
            isTriggered = true;
            StartCoroutine(ReleaseTheKraken(other.transform));
        }
    }

    private IEnumerator ReleaseTheKraken(Transform player)
    {
        Dispatcher.Unsubscribe(EBEventType.WorldStateChanged, address);

        RoomsManager roomsManager = RoomsManager.GetManager();
        roomsManager.LockAllDoors();

        schoolBellsManager.StartRinging();

        yield return new WaitForSeconds(3.0f);

        monster.GetComponent<MonsterBehaviour>().MoveToFarthestWaypointFrom(player.position);
        monster.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        roomsManager.UnlockAllDoors();
    }
}
