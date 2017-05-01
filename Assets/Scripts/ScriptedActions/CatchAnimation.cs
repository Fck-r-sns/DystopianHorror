using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class CatchAnimation : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private float catchAnimationTime = 0.5f;

    [SerializeField]
    private float wakeUpTime = 4.0f;

    [SerializeField]
    private float fadeToBlackTime = 2.0f;

    [SerializeField]
    private float fadeToNormalTime = 4.0f;

    [SerializeField]
    private string roomsManagerId = "School";

    private int address = AddressProvider.GetFreeAddress();
    private Camera camera;
    private FadingManager cameraFading;
    private FirstPersonController controller;
    private RoomsManager roomsManager;

    public void Init(Camera camera, FirstPersonController controller)
    {
        this.camera = camera;
        this.cameraFading = FadingManager.GetInstance();
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
        float initialY = controller.transform.position.y;
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
        cameraFading.FadeToBlack(fadeToBlackTime);
        yield return new WaitUntil(() => cameraFading.GetState() == FadingManager.State.Faded);

        RoomScene room = roomsManager.GetRandomWakeUpRoom();
        RoomEntry entry = roomsManager.GetRandomRoomEntry();
        room.ClearCollectibles();
        entry.AttachRoom(room);
        entry.SetSpawningEnabled(false);

        Transform wakeUpPosition = room.GetWakeUpPosition();
        controller.transform.position = wakeUpPosition.position;
        controller.transform.rotation = wakeUpPosition.rotation;

        Vector3 eulerAngles = controller.transform.eulerAngles;
        eulerAngles.x = 0.0f;
        eulerAngles.z = 0.0f;
        controller.transform.eulerAngles = eulerAngles;

        Vector3 pos = controller.transform.position;
        pos.y = initialY;
        controller.transform.position = pos;

        yield return new WaitForSeconds(wakeUpTime);

        cameraFading.FadeToNormal(fadeToNormalTime);
        controller.enabled = true;

        Dispatcher.SendEvent(new EBEvent() { type = EBEventType.ApplyMadnessAfterMonsterCaught });
    }

    private IEnumerator AnimateRestoration()
    {
        yield break;
    }

}
