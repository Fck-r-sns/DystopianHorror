using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFoundPredicate : Predicate
{
    [SerializeField]
    private bool keyFound = false;

    public override bool Check(WorldState world)
    {
        return (world.keyFound == keyFound);
    }
}
