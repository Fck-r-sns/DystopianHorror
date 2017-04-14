﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;
using System;

public class GoodEndingAnimation : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private Door door;

    [SerializeField]
    private CameraDirector director;

    [SerializeField]
    private Transform rotationTarget;

    [SerializeField]
    private Transform movementTarget;

    private int address = AddressProvider.GetFreeAddress();
    private Camera camera;
    private FirstPersonController controller;

    public void OnReceived(EBEvent e)
    {
        if (e.type == EBEventType.InteractionWithDoor)
        {
            DoorInteractionEvent die = e as DoorInteractionEvent;
            if (die.door == door)
            {
                controller.SetMouseLookEnabled(false);
                controller.SetHeadBobEnabled(false);
                controller.enabled = false;
                director.StartAnimating(camera, rotationTarget, movementTarget);
            }
        }
    }

    // Use this for initialization
    private void Start () {
        Dispatcher.Subscribe(EBEventType.InteractionWithDoor, address, gameObject);
	}

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.InteractionWithDoor, address);
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            camera = other.gameObject.GetComponentInChildren<Camera>();
            controller = other.gameObject.GetComponentInChildren<FirstPersonController>();
        }
    }

}