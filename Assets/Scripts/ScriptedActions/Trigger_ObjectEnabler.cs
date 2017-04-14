using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_ObjectEnabler : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    private void OnTriggerEnter(Collider other)
    {
        target.SetActive(true);
    }
}
