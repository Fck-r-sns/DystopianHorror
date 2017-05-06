using UnityEngine;

using EventBus;

public class MonsterCatch : MonoBehaviour, IEventSubscriber
{

    private Dispatcher dispatcher;
    private int address;

    public void OnReceived(EBEvent e)
    {
        switch (e.type)
        {
            case EBEventType.CaughtByMonster:
                this.enabled = false;
                break;

            case EBEventType.ApplyMadnessAfterMonsterCaught:
                this.enabled = true;
                break;
        }
    }

    private void Start()
    {
        dispatcher = Dispatcher.GetInstance();
        address = dispatcher.GetFreeAddress();
        dispatcher.Subscribe(EBEventType.CaughtByMonster, address, gameObject);
        dispatcher.Subscribe(EBEventType.ApplyMadnessAfterMonsterCaught, address, gameObject);
    }

    private void OnDestroy()
    {
        dispatcher.Unsubscribe(EBEventType.CaughtByMonster, address);
        dispatcher.Unsubscribe(EBEventType.ApplyMadnessAfterMonsterCaught, address);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            dispatcher.SendEvent(new EBEvent() { type = EBEventType.CaughtByMonster });
        }
    }
}
