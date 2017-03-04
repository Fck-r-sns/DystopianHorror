using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingHunter : MonoBehaviour
{

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float speed = 10.0f;

    void Update()
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }
}
