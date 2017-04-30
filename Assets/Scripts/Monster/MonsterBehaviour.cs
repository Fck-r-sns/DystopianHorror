using UnityEngine;
using UnityEngine.AI;

using EventBus;

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
    private bool patrolEnabled = true;

    [SerializeField]
    private bool ignoreTarget = false;

    [SerializeField]
    private float patrolSpeed = 5;

    [SerializeField]
    private float chaseSpeed = 7;

    [SerializeField]
    private float attackSpeed = 10;

    [SerializeField]
    private LayerMask layerMask = -1;

    private static float WAYPOINT_PASS_DISTANCE = 0.1f;

    private NavMeshAgent navMeshAgent;
    private CameraVisibilityChecker visibilityChecker;
    private Direction direction = Direction.Forward;
    private State state = State.Patrol;
    private Transform currentTarget;
    private int currentWaypointIndex = -1;
    private bool inPlainSight = false;

    public void SetMainTarget(Transform target)
    {
        mainTarget = target;
    }

    public void SetPatrolEnabled(bool enabled)
    {
        patrolEnabled = enabled;
    }

    public void MoveToFarthestWaypointFrom(Vector3 point)
    {
        Transform farthest = null;
        float maxDst = 0f;
        for (int i = 0; i < waypoints.Length; ++i)
        {
            Transform waypoint = waypoints[i];
            float dst = Mathf.Abs(point.x - waypoint.transform.position.x) + Mathf.Abs(point.z - waypoint.transform.position.z);
            if (farthest == null || dst > maxDst)
            {
                farthest = waypoint;
                maxDst = dst;
            }
        }
        if (farthest != null)
        {
            transform.position = farthest.position;
        }
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        visibilityChecker = GetComponent<CameraVisibilityChecker>();
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, mainTarget.position - transform.position);
        RaycastHit hit;
        bool newSightState = Physics.Raycast(ray, out hit, float.MaxValue, layerMask) && (hit.transform == mainTarget);
        if (!ignoreTarget && newSightState)
        {
            if (visibilityChecker.isVisible())
            {
                navMeshAgent.speed = attackSpeed;
                state = State.Attack;
            }
            else
            {
                navMeshAgent.speed = chaseSpeed;
                state = State.Chase;
            }
        }
        else
        {
            navMeshAgent.speed = patrolSpeed;
            state = State.Patrol;
        }

        Patrol();

        if (inPlainSight != newSightState)
        {
            inPlainSight = newSightState;
            Dispatcher.SendEvent(new EBEvent() { type = inPlainSight ? EBEventType.MonsterInPlainSight : EBEventType.MonsterOutOfPlainSight });
        }
    }

    private void Patrol()
    {
        if (!patrolEnabled)
        {
            return;
        }

        float dst = float.MaxValue;
        if (currentTarget != null)
        {
            Vector3 v1 = transform.position;
            v1.y = 0;
            Vector3 v2 = currentTarget.position;
            v2.y = 0;
            dst = Vector3.Distance(v1, v2);
        }

        if ((currentTarget == null) || (dst < WAYPOINT_PASS_DISTANCE))
        {
            currentWaypointIndex = GetNextWaypoint();
            currentTarget = waypoints[currentWaypointIndex];
        }

        navMeshAgent.SetDestination(currentTarget.position);
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
