using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EventBus
{

    public class Dispatcher : MonoBehaviour
    {
        private static Dictionary<EventType, Dictionary<int, GameObject>> subscribers = new Dictionary<EventType, Dictionary<int, GameObject>>();

        public static void Subscribe(EventType eventType, int address, GameObject subscriber)
        {
            if (!subscribers.ContainsKey(eventType))
            {
                subscribers.Add(eventType, new Dictionary<int, GameObject>());
            }
            subscribers[eventType].Add(address, subscriber);
        }

        public static void Unsubscribe(EventType eventType, int address)
        {
            if (subscribers.ContainsKey(eventType))
            {
                subscribers[eventType].Remove(address);
            }
        }

        public static void SendEvent(Event e)
        {
            Dictionary<int, GameObject> typeSubscribers = subscribers[e.type];
            if (typeSubscribers == null)
            {
                return;
            }
            if (e.address == Defines.BROADCAST_ADDRESS)
            {
                foreach (var entry in typeSubscribers)
                {
                    ExecuteEvents.Execute<IEventSubscriber>(entry.Value, null, (handler, data) => handler.OnReceived(e));
                }
            }
            else
            {
                GameObject target = typeSubscribers[e.address];
                if (target != null)
                {
                    ExecuteEvents.Execute<IEventSubscriber>(target, null, (handler, data) => handler.OnReceived(e));
                }
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}