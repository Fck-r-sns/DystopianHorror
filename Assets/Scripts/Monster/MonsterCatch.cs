using UnityEngine;

using EventBus;

public class MonsterCatch : MonoBehaviour, IEventSubscriber
{

    private int address = AddressProvider.GetFreeAddress();

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
        Dispatcher.Subscribe(EBEventType.CaughtByMonster, address, gameObject);
        Dispatcher.Subscribe(EBEventType.ApplyMadnessAfterMonsterCaught, address, gameObject);
    }

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.CaughtByMonster, address);
        Dispatcher.Unsubscribe(EBEventType.ApplyMadnessAfterMonsterCaught, address);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Dispatcher.SendEvent(new EBEvent() { type = EBEventType.CaughtByMonster });
        }
    }
}
