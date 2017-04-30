using UnityEngine;

using EventBus;

public class SchoolBellsManager : MonoBehaviour, IEventSubscriber
{

    private int address = AddressProvider.GetFreeAddress();

    public void StartRinging()
    {
        foreach (Transform child in transform)
        {
            SchoolBell bell = child.GetComponent<SchoolBell>();
            if (bell != null)
            {
                bell.StartRinging();
            }
        }
    }

    public void StopRinging()
    {
        foreach (Transform child in transform)
        {
            SchoolBell bell = child.GetComponent<SchoolBell>();
            if (bell != null)
            {
                bell.StopRinging();
            }
        }
    }

    public void OnReceived(EBEvent e)
    {
        switch (e.type)
        {
            case EBEventType.ChangeStateToNormalRequest:
                ChangeState(SchoolBell.State.Normal);
                break;

            case EBEventType.ChangeStateToMadRequest:
                ChangeState(SchoolBell.State.Mad);
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

    private void ChangeState(SchoolBell.State state)
    {
        foreach (Transform child in transform)
        {
            SchoolBell bell = child.GetComponent<SchoolBell>();
            if (bell != null)
            {
                bell.SetState(state);
            }
        }
    }

}
