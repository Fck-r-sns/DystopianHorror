using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class ChasedPrey : MonoBehaviour {

    [SerializeField]
    private float waypointSpawnPeriod = 1.0f;

    private float lastSpawnTime = 0.0f;

	void Start () {
        lastSpawnTime = Time.time;
	}
	
	void Update () {
		if (Time.time - lastSpawnTime > waypointSpawnPeriod)
        {
            CreateWaypoint();
            lastSpawnTime += waypointSpawnPeriod;
        }
	}

    private void CreateWaypoint()
    {
        Dispatcher.GetInstance().SendEvent(new NewWaypointEvent(new Waypoint(transform.position, Time.time)));
    }
}
