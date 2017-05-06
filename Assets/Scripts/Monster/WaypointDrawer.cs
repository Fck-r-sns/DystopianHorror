using UnityEngine;

using EventBus;

public class WaypointDrawer : MonoBehaviour, IEventSubscriber
{

    private Dispatcher dispatcher;
    private int address;

    public void OnReceived(EBEvent e)
    {
        if (e.type == EBEventType.NewWaypointCreated)
        {
            DrawWaypoint((e as NewWaypointEvent).waypoint.position);
        }
    }

    private void Start()
    {
        dispatcher = Dispatcher.GetInstance();
        address = dispatcher.GetFreeAddress();
        dispatcher.Subscribe(EBEventType.NewWaypointCreated, address, gameObject);
    }

    private void OnDestroy()
    {
        dispatcher.Unsubscribe(EBEventType.NewWaypointCreated, address);
    }

    private void DrawWaypoint(Vector3 position)
    {
        Debug.DrawRay(position, Vector3.up, Color.black, 10.0f);
    }

}
