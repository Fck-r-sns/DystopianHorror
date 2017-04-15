using UnityEngine;

using EventBus;

public class MonsterCatch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Dispatcher.SendEvent(new EBEvent() { type = EBEventType.CaughtByMonster });
        }
    }
}
