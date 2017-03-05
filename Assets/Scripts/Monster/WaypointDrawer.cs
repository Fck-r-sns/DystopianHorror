using UnityEngine;

using EventBus;

public class WaypointDrawer : MonoBehaviour, IEventSubscriber
{

    private int address = AddressProvider.GetFreeAddress();

    public void OnReceived(EBEvent e)
    {
        if (e.type == EBEventType.NewWaypointCreated)
        {
            DrawWaypoint((e as NewWaypointEvent).waypoint.position);
        }
    }

    private void Awake()
    {
        Dispatcher.Subscribe(EBEventType.NewWaypointCreated, address, gameObject);
    }

    private void DrawWaypoint(Vector3 position)
    {
        Debug.DrawRay(position, Vector3.up, Color.black, 10.0f);
    }

}
