﻿using UnityEngine;

using EventBus;

public class Trigger_PositiveEpilogueEntered : MonoBehaviour
{

    [SerializeField]
    private Door door;

    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            triggered = true;
            door.Close();
            door.Lock();
            Dispatcher.GetInstance().SendEvent(new EBEvent() { type = EBEventType.PositiveEpilogueEntered });
            NoiseEffectsManager noiseEffectManager = other.gameObject.GetComponentInChildren<NoiseEffectsManager>();
            noiseEffectManager.enabled = false;
            noiseEffectManager.SetMonster(null);
            RoomsManager manager = RoomsManager.GetManager();
            manager.UnloadHall();
        }
    }
}
