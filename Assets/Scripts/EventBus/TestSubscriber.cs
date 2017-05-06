using UnityEngine;

using EventBus;

public class TestSubscriber : MonoBehaviour, IEventSubscriber
{

    private int address;

    public void OnReceived(EBEvent e)
    {
        Debug.Log("Event received: " + e);
    }

    // Use this for initialization
    void Start()
    {
        address = Dispatcher.GetInstance().GetFreeAddress();
        Dispatcher.GetInstance().Subscribe(EBEventType.TestEvent, address, gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}