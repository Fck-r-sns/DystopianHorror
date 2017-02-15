using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventBus
{
    public enum EventType
    {
        TestEvent
    }

    public class Event
    {
        public EventType type;
        public int address;

        public override string ToString()
        {
            return "Event(type=" + type + ";address=" + address + ")";
        }
    }
}
