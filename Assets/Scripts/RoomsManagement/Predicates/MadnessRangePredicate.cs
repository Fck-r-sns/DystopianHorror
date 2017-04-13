using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadnessRangePredicate : Predicate
{

    [SerializeField]
    private int lowerBound = WorldState.MIN_MADNESS;

    [SerializeField]
    private int upperBound = WorldState.MIN_MADNESS;

    public override bool Check(WorldState world)
    {
        return (lowerBound <= world.madness) && (world.madness <= upperBound);
    }

}
