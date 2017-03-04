using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class Prey : MonoBehaviour {

    [SerializeField]
    private float waypointSpawnPeriod_s = 1.0f;

    private float lastSpawnTime = 0.0f;

	void Start () {
        lastSpawnTime = Time.time;
	}
	
	void Update () {
		if (Time.time - lastSpawnTime > waypointSpawnPeriod_s)
        {
            CreateWaypoint();
            lastSpawnTime += waypointSpawnPeriod_s;
        }
	}

    private void CreateWaypoint()
    {
        Dispatcher.SendEvent(new NewWaypointEvent(new Waypoint(transform.position, Time.time)));
    }
}
