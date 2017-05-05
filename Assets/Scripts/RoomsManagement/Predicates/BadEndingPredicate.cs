using UnityEngine;

public class BadEndingPredicate : Predicate
{
    [SerializeField]
    private EndingPredicate endingPredicate;

    public override bool Check(WorldState world)
    {
        return (world.keyFound == true) && !endingPredicate.Check(world);
    }
}
