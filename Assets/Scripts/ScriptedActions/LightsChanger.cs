using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class LightsChanger : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private DaylightLamp.State normalState = DaylightLamp.State.On;

    [SerializeField]
    private DaylightLamp.State madState = DaylightLamp.State.BrokenOn;

    private Dispatcher dispatcher;
    private int address;

    public void OnReceived(EBEvent e)
    {
        switch (e.type)
        {
            case EBEventType.ChangeStateToNormalRequest:
                ChangeLampsStates(normalState);
                break;

            case EBEventType.ChangeStateToMadRequest:
                ChangeLampsStates(madState);
                break;
        }
    }

    private void Start()
    {
        dispatcher = Dispatcher.GetInstance();
        address = dispatcher.GetFreeAddress();
        dispatcher.Subscribe(EBEventType.ChangeStateToNormalRequest, address, gameObject);
        dispatcher.Subscribe(EBEventType.ChangeStateToMadRequest, address, gameObject);
    }

    private void OnDestroy()
    {
        dispatcher.Unsubscribe(EBEventType.ChangeStateToNormalRequest, address);
        dispatcher.Unsubscribe(EBEventType.ChangeStateToMadRequest, address);
    }

    private void ChangeLampsStates(DaylightLamp.State state)
    {
        foreach (Transform child in transform)
        {
            DaylightLamp lamp = child.GetComponent<DaylightLamp>();
            if (lamp != null)
            {
                lamp.SetState(state);
            }
        }
    }
}
