using UnityEngine;

using EventBus;

public class TestPublisher : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Dispatcher.GetInstance().SendEvent(
            new EBEvent
            {
                type = EBEventType.TestEvent,
                address = Defines.BROADCAST_ADDRESS
            }
            );
    }
}