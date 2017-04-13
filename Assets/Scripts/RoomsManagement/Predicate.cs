using UnityEngine;

public class Predicate : MonoBehaviour
{
    public virtual bool Check(WorldState world)
    {
        return false;
    }
}
