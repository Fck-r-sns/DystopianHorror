using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class CatchAnimation : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private float catchAnimationTime = 2.0f;

    [SerializeField]
    private string roomsManagerId = "School";

    private int address = AddressProvider.GetFreeAddress();
    private Camera camera;
    private CameraFading cameraFading;
    private FirstPersonController controller;
    private RoomsManager roomsManager;

    public void Init(Camera camera, CameraFading cameraFading, FirstPersonController controller)
    {
        this.camera = camera;
        this.cameraFading = cameraFading;
        this.controller = controller;
        roomsManager = RoomsManager.GetManager(roomsManagerId);
    }

    public void OnReceived(EBEvent e)
    {
        if (e.type == EBEventType.CaughtByMonster)
        {
            controller.enabled = false;
            StartCoroutine(AnimateCatch());
        }
    }

    private void Start()
    {
        Dispatcher.Subscribe(EBEventType.CaughtByMonster, address, gameObject);
    }

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.CaughtByMonster, address);
    }

    private IEnumerator AnimateCatch()
    {
        float startTime = Time.time;
        Quaternion from = controller.transform.rotation;
        Quaternion to = Quaternion.LookRotation(Vector3.up, controller.transform.right);
        bool soundPlayed = false;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t = (Time.time - startTime) / catchAnimationTime;
            Quaternion rotation = Quaternion.Slerp(from, to, t);
            controller.transform.rotation = rotation;
            if ((t > 0.7f) && !soundPlayed)
            {
                controller.PlayFallingSound();
                soundPlayed = true;
            }
            yield return null;
        }
        cameraFading.FadeToBlack();
        yield return new WaitForSeconds(1.0f);

        RoomScene room = roomsManager.GetMonsterRoom();
        RoomEntry entry = roomsManager.GetRandomRoomEntry();
        room.ClearCollectibles();
        entry.AttachRoom(room);
        entry.SetSpawningEnabled(false);
    }

    private IEnumerator AnimateRestoration()
    {
        yield break;
    }

}
