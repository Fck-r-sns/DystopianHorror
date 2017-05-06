using System.Collections;
using UnityEngine;

using EventBus;

public class MovePlayerWithRoom : MonoBehaviour, IEventSubscriber
{

    private Dispatcher dispatcher;
    private int address;

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
        dispatcher = Dispatcher.GetInstance();
        address = dispatcher.GetFreeAddress();
        dispatcher.Subscribe(EBEventType.HallMovingTriggerEntered, address, gameObject);
    }

    private void OnDestroy()
    {
        dispatcher.Unsubscribe(EBEventType.HallMovingTriggerEntered, address);
    }

    private IEnumerator MoveOnNextUpdate(HallMovingTriggerEnteredEvent hmtee)
    {
        yield return new WaitWhile(() => Time.frameCount < hmtee.frameNumber);
        transform.Translate(hmtee.newPosition - hmtee.oldPosition, Space.World);
        transform.RotateAround(hmtee.newPosition, Vector3.up, hmtee.newRotation.y - hmtee.oldRotation.y);
    }
}
