using UnityEngine;

using EventBus;

public class TestSubscriber : MonoBehaviour, IEventSubscriber
{

    private Dispatcher dispatcher;
    private int address;

    public void OnReceived(EBEvent e)
    {
        Debug.Log("Event received: " + e);
    }

    // Use this for initialization
    void Start()
    {
        dispatcher = Dispatcher.GetInstance();
        address = dispatcher.GetFreeAddress();
        dispatcher.Subscribe(EBEventType.TestEvent, address, gameObject);
    }

    private void OnDestroy()
    {
        dispatcher.Unsubscribe(EBEventType.TestEvent, address);
    }

}