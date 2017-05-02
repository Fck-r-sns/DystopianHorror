
public class GoodEndingPredicate : Predicate
{
    public override bool Check(WorldState world)
    {
        return (world.keyFound == true) && (world.timesCaughtByMonster == 0) && (world.madness < WorldState.ENDING_MADNESS_THRESHOLD);
    }
}
