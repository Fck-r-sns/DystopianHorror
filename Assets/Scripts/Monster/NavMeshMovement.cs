using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovement : MonoBehaviour
{

    [SerializeField]
    private Transform[] waypoints;

    [SerializeField]
    private Transform mainTarget;

    [SerializeField]
    private LayerMask layerMask = -1;

    private NavMeshAgent navMeshAgent;

    // Use this for initialization
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //layerMask = LayerMask.GetMask("Architecture") | LayerMask.GetMask("Player");
        Debug.Log(layerMask.value);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, mainTarget.position - transform.position);
        RaycastHit hit;
        Vector3 target;
        if (Physics.Raycast(ray, out hit, float.MaxValue, layerMask))
        {
            Debug.DrawLine(transform.position, hit.point, Color.black, 1);
            if (hit.transform.gameObject.tag.Equals("Player"))
            {
                target = mainTarget.position;
            }
            else
            {
                target = GetNearestWaypoint();
            }
        }
        else
        {
            target = GetNearestWaypoint();
        }

        if (target != null)
        {
            target.y = transform.position.y;
            navMeshAgent.SetDestination(target);
        }
    }

    private Vector3 GetNearestWaypoint()
    {

        float minDistance = float.MaxValue;
        Vector3 result = transform.position;
        foreach(Transform waypoint in waypoints)
        {
            float dst = Vector3.Distance(transform.position, waypoint.position);
            if (dst < minDistance)
            {
                minDistance = dst;
                result = waypoint.position;
            }
        }
        return result;
    }
}
