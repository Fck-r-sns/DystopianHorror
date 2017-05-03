using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;
using System;

public class GoodEndingAnimation : MonoBehaviour, IEventSubscriber
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
    private GameObject light;

    [SerializeField]
    private AudioSource birds;

    private int address = AddressProvider.GetFreeAddress();
    private Camera camera;
    private FirstPersonController controller;
    private bool isTriggered = false;

    public void OnReceived(EBEvent e)
    {
        if (e.type == EBEventType.InteractionWithDoor)
        {
            DoorInteractionEvent die = e as DoorInteractionEvent;
            if (die.door == door)
            {
                StartCoroutine(Animation());
            }
        }
    }

    // Use this for initialization
    private void Start () {
        Dispatcher.Subscribe(EBEventType.InteractionWithDoor, address, gameObject);
	}

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.InteractionWithDoor, address);
    }

    // Update is called once per frame
    void Update () {
		
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

    private IEnumerator Animation()
    {
        RenderSettings.fog = false;
        camera.gameObject.GetComponent<UnityStandardAssets.CinematicEffects.AmbientOcclusion>().enabled = false;
        light.SetActive(true);
        controller.SetMouseLookEnabled(false);
        controller.SetHeadBobEnabled(false);
        controller.enabled = false;
        birds.Play();
        director.StartAnimating(camera, rotationTarget, movementTarget);
        yield return new WaitUntil(() => director.IsFinished());

        yield return new WaitForSeconds(1.0f);

        TextOutput textOutput = TextOutput.GetInstance();
        textOutput.ShowText(TextManager.GetGoodEndingText());
        yield return new WaitWhile(() => textOutput.IsActive());

        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

}
