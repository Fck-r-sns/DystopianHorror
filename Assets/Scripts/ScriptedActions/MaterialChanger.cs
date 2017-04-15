﻿using UnityEngine;
using EventBus;

public class MaterialChanger : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private Material normalMaterial;

    [SerializeField]
    private Material madMaterial;

    private int address = AddressProvider.GetFreeAddress();

    public void OnReceived(EBEvent e)
    {
        switch (e.type)
        {
            case EBEventType.ChangeStateToNormalRequest:
                GetComponent<Renderer>().material = normalMaterial;
                break;

            case EBEventType.ChangeStateToMadRequest:
                GetComponent<Renderer>().material = madMaterial;
                break;
        }
    }

    private void Start () {
        Dispatcher.Subscribe(EBEventType.ChangeStateToNormalRequest, address, gameObject);
        Dispatcher.Subscribe(EBEventType.ChangeStateToMadRequest, address, gameObject);
    }

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.ChangeStateToNormalRequest, address);
        Dispatcher.Unsubscribe(EBEventType.ChangeStateToMadRequest, address);
    }

}
