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
            StartCoroutine(MoveOnNextUpdate(hmtee));
        }
    }

    private void Start()
    {
        Dispatcher.Subscribe(EBEventType.HallMovingTriggerEntered, address, gameObject);
    }

    private IEnumerator MoveOnNextUpdate(HallMovingTriggerEnteredEvent hmtee)
    {
        yield return new WaitWhile(() => Time.frameCount < hmtee.frameNumber);
        transform.Translate(hmtee.newPosition - hmtee.oldPosition, Space.World);
        transform.RotateAround(hmtee.newPosition, Vector3.up, hmtee.newRotation.y - hmtee.oldRotation.y);
    }
}
