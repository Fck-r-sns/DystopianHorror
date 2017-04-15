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

    private int address = AddressProvider.GetFreeAddress();

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
        Dispatcher.Subscribe(EBEventType.ChangeStateToNormalRequest, address, gameObject);
        Dispatcher.Subscribe(EBEventType.ChangeStateToMadRequest, address, gameObject);
    }

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.ChangeStateToNormalRequest, address);
        Dispatcher.Unsubscribe(EBEventType.ChangeStateToMadRequest, address);
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
