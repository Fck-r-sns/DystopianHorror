using EventBus;
using UnityEngine;

public class TestSubscriber : MonoBehaviour, IEventSubscriber
{

    private int address = AddressProvider.GetFreeAddress();

    public void OnReceived(EventBus.Event e)
    {
        Debug.Log("Event received: " + e);
    }

    // Use this for initialization
    void Start()
    {
        Dispatcher.Subscribe(EventBus.EventType.TestEvent, address, gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}