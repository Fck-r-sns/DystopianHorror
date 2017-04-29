using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;
using System;

public class BadEndingAnimation : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private Door door;

    [SerializeField]
    private CameraDirector director;

    [SerializeField]
    private Transform rotationTarget;

    [SerializeField]
    private Transform movementTarget;

    [SerializeField]
    private GameObject monster;

    private int address = AddressProvider.GetFreeAddress();
    private Camera camera;
    private FirstPersonController controller;
    private bool isTriggered = false;

    public void OnReceived(EBEvent e)
    {
        switch (e.type)
        {
            case EBEventType.InteractionWithDoor:
                {
                    DoorInteractionEvent die = e as DoorInteractionEvent;
                    if (die.door == door)
                    {
                        monster.SetActive(true);
                        monster.GetComponent<CameraVisibilityChecker>().SetCamera(camera);
                        monster.GetComponent<MonsterBehaviour>().SetMainTarget(controller.transform);
                        NoiseEffectsManager noiseEffectsManager = controller.gameObject.GetComponentInChildren<NoiseEffectsManager>();
                        noiseEffectsManager.SetMonster(monster.transform);
                        noiseEffectsManager.enabled = true;
                        Dispatcher.Subscribe(EBEventType.CaughtByMonster, address, gameObject);
                        Dispatcher.Subscribe(EBEventType.MonsterInFrustum, address, gameObject);
                    }
                }
                break;

            case EBEventType.CaughtByMonster:
                controller.enabled = false;
                monster.GetComponent<MonsterBehaviour>().SetPatrolEnabled(false);
                camera.GetComponent<CameraFading>().FadeToBlack(5);
                StartCoroutine(FadeOutMonsterSounds(5));
                break;

            case EBEventType.MonsterInFrustum:
                controller.SetMouseLookEnabled(false);
                controller.SetHeadBobEnabled(false);
                controller.enabled = false;
                director.StartAnimating(camera, rotationTarget, camera.transform);
                break;
        }
    }

    private void Start()
    {
        Dispatcher.Subscribe(EBEventType.InteractionWithDoor, address, gameObject);
    }

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.InteractionWithDoor, address);
        Dispatcher.Unsubscribe(EBEventType.CaughtByMonster, address);
        Dispatcher.Unsubscribe(EBEventType.MonsterInFrustum, address);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.gameObject.tag.Equals("Player"))
        {
            isTriggered = true;
            camera = other.gameObject.GetComponentInChildren<Camera>();
            controller = other.gameObject.GetComponentInChildren<FirstPersonController>();
        }
    }

    private IEnumerator FadeOutMonsterSounds(float fadingTime)
    {
        AudioSource source = monster.GetComponentInChildren<AudioSource>();
        float delta = source.volume / fadingTime;
        while (source.volume > 0)
        {
            source.volume -= delta * Time.deltaTime;
            yield return null;
        }
    }

}
