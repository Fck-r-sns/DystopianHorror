using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class HallMovingTrigger : MonoBehaviour
{

    private RoomTranslator roomTranslator;

    private void Start()
    {
        roomTranslator = transform.parent.GetComponent<RoomTranslator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        roomTranslator.OnTranslationTriggerEnter();
    }
}
