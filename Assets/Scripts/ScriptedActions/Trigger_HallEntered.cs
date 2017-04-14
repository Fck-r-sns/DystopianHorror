using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class Trigger_HallEntered : MonoBehaviour, IEventSubscriber
{
    public enum Mode
    {
        Primary,
        Secondary
    }

    [SerializeField]
    private Mode mode = Mode.Primary;

    [SerializeField]
    private Door door;

    [SerializeField]
    private RoomEntry roomEntry;

    private int address = AddressProvider.GetFreeAddress();
    private static bool triggered = false;
    
    public void OnReceived(EBEvent e)
    {
        if (e.type == EBEventType.RoomSpawningTrigger)
        {
            RoomSpawningTriggerEvent rste = e as RoomSpawningTriggerEvent;
            if (rste.roomEntryId != roomEntry.GetId())
            {
                door.Unlock();
                roomEntry.SetSpawningEnabled(true);
                string roomsManagerId = roomEntry.GetRoomsManagerId();
                RoomsManager manager = RoomsManager.GetManager(roomsManagerId);
                manager.UnloadPrologue();
                StartCoroutine(UnsubscribeOnNextUpdate());
            }
        }
    }

    private void Start()
    {
        if (mode == Mode.Primary)
        {
            Dispatcher.Subscribe(EBEventType.RoomSpawningTrigger, address, gameObject);
        }
    }

    private void OnDestroy()
    {
        if (mode == Mode.Primary)
        {
            Dispatcher.Unsubscribe(EBEventType.RoomSpawningTrigger, address);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            triggered = true;
            Dispatcher.SendEvent(new EBEvent() { type = EBEventType.HallEntered });
            door.Close();
            door.Lock();
        }
    }

    private IEnumerator UnsubscribeOnNextUpdate()
    {
        yield return null;
        Dispatcher.Unsubscribe(EBEventType.RoomSpawningTrigger, address);
    }

}
