
public class EndingPredicate : Predicate
{
    public override bool Check(WorldState world)
    {
        // true for good ending, false for bad
        return (world.timesCaughtByMonster == 0)
            && ((world.madness < WorldState.ENDING_MADNESS_THRESHOLD) || (world.collectiblesFound >= WorldState.ENDING_COLLECTIBLES_FOUND_THRESHOLD));
    }
}
