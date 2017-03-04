using UnityEngine;

public class Waypoint {

    public readonly Vector3 position;
    public readonly float time;

    public Waypoint(Vector3 position, float time)
    {
        this.position = position;
        this.time = time;
    }
}
