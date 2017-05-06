using UnityEngine;
using EventBus;

public class MaterialChangerGlobal : MonoBehaviour, IEventSubscriber
{

    private Dispatcher dispatcher;
    private int address;
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
                    dispatcher.SendEvent(new EBEvent() { type = EBEventType.ChangeStateToMadRequest });
                }
            }
            else
            {
                if (isMad)
                {
                    isMad = false;
                    dispatcher.SendEvent(new EBEvent() { type = EBEventType.ChangeStateToNormalRequest });
                }
            }
        }
    }

    private void Start()
    {
        dispatcher = Dispatcher.GetInstance();
        address = dispatcher.GetFreeAddress();
        dispatcher.Subscribe(EBEventType.WorldStateChanged, address, gameObject);
    }

    private void OnDestroy()
    {
        dispatcher.Unsubscribe(EBEventType.WorldStateChanged, address);
    }
}
