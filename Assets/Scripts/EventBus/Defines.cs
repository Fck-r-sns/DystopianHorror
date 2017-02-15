using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventBus
{
    public static class Defines
    {
        public const int BROADCAST_ADDRESS = -1;

        // reserved addresses
        public const int PLAYER_ADDRESS = 1;
        public const int MONSTER_ADDRESS = 2;
        public const int TEST_ADDRESS = 42;

        public const int FIRST_FREE_ADDRESS = 100;
    }
}