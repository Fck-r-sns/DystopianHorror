using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventBus
{
    public enum EBEventType
    {
        TestEvent
    }

    public class EBEvent
    {
        public EBEventType type;
        public int address;

        public override string ToString()
        {
            return "Event(type=" + type + ";address=" + address + ")";
        }
    }
}
