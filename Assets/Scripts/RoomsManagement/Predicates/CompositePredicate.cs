using System;
using UnityEngine;

[Serializable]
public class CompositePredicate
{
    [SerializeField]
    private Predicate[] predicates;

    public bool Check(WorldState world)
    {
        if ((predicates == null) || (predicates.Length == 0))
        {
            return false;
        }
        bool res = true;
        foreach (Predicate p in predicates)
        {
            res = res && p.Check(world);
        }
        return res;
    }
}
