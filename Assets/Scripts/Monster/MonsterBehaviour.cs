using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CameraVisibilityChecker))]
public class MonsterBehaviour : MonoBehaviour
{

    private enum Direction
    {
        Forward = +1,
        Backward = -1
    }

    private enum State
    {
        Patrol,
        Chase,
        Attack
    }

    [SerializeField]
    private Transform[] waypoints;

    [SerializeField]
    private Transform mainTarget;

    [SerializeField]
    private float patrolSpeed = 5;

    [SerializeField]
    private float chaseSpeed = 7;

    [SerializeField]
    private float attackSpeed = 10;

    [SerializeField]
    private LayerMask layerMask = -1;

    private static float WAYPOINT_PASS_DISTANCE = 1.0f;

    private NavMeshAgent navMeshAgent;
    private CameraVisibilityChecker visibilityChecker;
    private Direction direction = Direction.Forward;
    private State state = State.Patrol;
    private Transform currentTarget;
    private int currentWaypointIndex = -1;

    // Use this for initialization
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        visibilityChecker = GetComponent<CameraVisibilityChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, mainTarget.position - transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, layerMask) && (hit.transform == mainTarget))
        {
            if (visibilityChecker.isVisible())
            {
                Attack();
                state = State.Attack;
            }
            else
            {
                Chase();
                state = State.Chase;
            }
        }
        else
        {
            Patrol();
            state = State.Patrol;
        }
    }

    private void Attack()
    {
        currentTarget = mainTarget;
        currentWaypointIndex = -1;
        navMeshAgent.SetDestination(currentTarget.position);
        navMeshAgent.speed = attackSpeed;
    }

    private void Chase()
    {
        currentTarget = mainTarget;
        currentWaypointIndex = -1;
        navMeshAgent.SetDestination(currentTarget.position);
        navMeshAgent.speed = chaseSpeed;
    }

    private void Patrol()
    {
        float dst = float.MaxValue;
        if ((currentTarget != null) && (currentTarget != mainTarget))
        {
            Vector3 v1 = transform.position;
            v1.y = 0;
            Vector3 v2 = currentTarget.position;
            v2.y = 0;
            dst = Vector3.Distance(v1, v2);
        }

        if ((currentTarget == null) || (state != State.Patrol) || (dst < WAYPOINT_PASS_DISTANCE))
        {
            currentWaypointIndex = GetNextWaypoint();
            currentTarget = waypoints[currentWaypointIndex];
        }

        navMeshAgent.SetDestination(currentTarget.position);
        navMeshAgent.speed = patrolSpeed;
    }

    private int GetNextWaypoint()
    {
        if ((currentWaypointIndex != -1) && (currentTarget != null))
        {
            return (currentWaypointIndex + (int)direction + waypoints.Length) % waypoints.Length;
        }
        direction = ((Random.Range(0, 100) & 1) == 1) ? Direction.Forward : Direction.Backward;
        return GetNearestWaypoint();
    }

    private int GetNearestWaypoint()
    {
        float minDistance = float.MaxValue;
        int result = -1;
        for (int i = 0; i < waypoints.Length; ++i)
        {
            float dst = Vector3.Distance(transform.position, waypoints[i].position);
            if (dst < minDistance)
            {
                minDistance = dst;
                result = i;
            }
        }
        return result;
    }
}
