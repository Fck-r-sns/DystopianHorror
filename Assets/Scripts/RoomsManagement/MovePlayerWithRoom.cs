using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class MovePlayerWithRoom : MonoBehaviour, IEventSubscriber
{

    private int address = AddressProvider.GetFreeAddress();

    public void OnReceived(EBEvent e)
    {
        if (e.type == EBEventType.HallMovingTriggerEntered)
        {
            HallMovingTriggerEnteredEvent hmtee = (e as HallMovingTriggerEnteredEvent);
            transform.Translate(hmtee.newPosition - hmtee.oldPosition, Space.World);
            transform.RotateAround(hmtee.newPosition, Vector3.up, hmtee.newRotation.y - hmtee.oldRotation.y);
        }
    }

    private void Awake()
    {
        Dispatcher.Subscribe(EBEventType.HallMovingTriggerEntered, address, gameObject);
    }
}
