using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimesCaughtByMonsterPredicate : Predicate
{
    [SerializeField]
    private int lowerBound = 0;

    [SerializeField]
    private int upperBound = int.MaxValue;

    public override bool Check(WorldState world)
    {
        return (lowerBound <= world.timesCaughtByMonster) && (world.timesCaughtByMonster <= upperBound);
    }
}
