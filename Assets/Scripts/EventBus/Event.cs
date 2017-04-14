using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventBus
{
    public enum EBEventType
    {
        TestEvent,
        TestDoorTriggerEntered,
        TestDoorTriggerExited,
        NewWaypointCreated,
        RoomSpawningTrigger,
        HallMovingTriggerEntered,
        DoorClosingTrigger,
        MonsterInFrustum,
        MonsterOutOfFrustum,
        MonsterInPlainSight,
        MonsterOutOfPlainSight,
        ItemCollected,
        CaughtByMonster
    }

    public enum TriggerAction
    {
        Enter,
        Exit
    }

    public class EBEvent
    {
        public EBEventType type;
        public int address = Defines.BROADCAST_ADDRESS;

        public override string ToString()
        {
            return "Event(type=" + type + ";address=" + address + ")";
        }
    }
}
