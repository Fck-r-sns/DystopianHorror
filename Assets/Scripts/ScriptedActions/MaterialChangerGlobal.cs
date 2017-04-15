using UnityEngine;
using EventBus;

public class MaterialChangerGlobal : MonoBehaviour, IEventSubscriber
{

    private int address = AddressProvider.GetFreeAddress();
    private bool isMad = false;

    public void OnReceived(EBEvent e)
    {
        if (e.type == EBEventType.WorldStateChanged)
        {
            WorldStateChangedEvent wsce = e as WorldStateChangedEvent;
            if (wsce.worldState.madness >= WorldState.NORMAL_TO_MAD_BOUNDARY)
            {
                if (!isMad)
                {
                    isMad = true;
                    Dispatcher.SendEvent(new EBEvent() { type = EBEventType.ChangeWallsToMadRequest });
                }
            }
            else
            {
                if (isMad)
                {
                    isMad = false;
                    Dispatcher.SendEvent(new EBEvent() { type = EBEventType.ChangeWallsToNormalRequest });
                }
            }
        }
    }

    private void Start()
    {
        Dispatcher.Subscribe(EBEventType.WorldStateChanged, address, gameObject);
    }

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.WorldStateChanged, address);
    }
}
