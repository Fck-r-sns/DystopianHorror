using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class Trigger_HallEntered : MonoBehaviour, IEventSubscriber
{
    public enum Mode
    {
        Primary,
        Secondary
    }

    [SerializeField]
    private Mode mode = Mode.Primary;

    [SerializeField]
    private Door door;

    [SerializeField]
    private RoomEntry roomEntry;

    [SerializeField]
    private GameObject monster;

    [SerializeField]
    private CatchAnimation catchAnimation;

    private Dispatcher dispatcher;
    private int address;
    private static bool triggered = false;
    
    public void OnReceived(EBEvent e)
    {
        if (e.type == EBEventType.RoomSpawningTrigger)
        {
            RoomSpawningTriggerEvent rste = e as RoomSpawningTriggerEvent;
            if (rste.roomEntryId != roomEntry.GetId())
            {
                door.Unlock();
                roomEntry.SetSpawningEnabled(true);
                RoomsManager manager = RoomsManager.GetManager();
                manager.UnloadPrologue();
                StartCoroutine(UnsubscribeOnNextUpdate());
            }
        }
    }

    private void Awake()
    {
        if (triggered)
        {
            triggered = false;
        }
    }

    private void Start()
    {
        dispatcher = Dispatcher.GetInstance();
        address = dispatcher.GetFreeAddress();
        if (mode == Mode.Primary)
        {
            dispatcher.Subscribe(EBEventType.RoomSpawningTrigger, address, gameObject);
        }
    }

    private void OnDestroy()
    {
        if (mode == Mode.Primary)
        {
            dispatcher.Unsubscribe(EBEventType.RoomSpawningTrigger, address);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            triggered = true;
            dispatcher.SendEvent(new EBEvent() { type = EBEventType.HallEntered });
            door.Close();
            door.Lock();
            if (other.gameObject.tag.Equals("Player"))
            {
                NoiseEffectsManager noiseEffectsManager = other.gameObject.GetComponentInChildren<NoiseEffectsManager>();
                noiseEffectsManager.enabled = true;
                noiseEffectsManager.SetMonster(monster.transform);

                catchAnimation.Init(
                    other.gameObject.GetComponentInChildren<Camera>(),
                    other.gameObject.GetComponent<FirstPersonController>()
                    );

                monster.GetComponent<MonsterBehaviour>().SetMainTarget(other.transform);
                monster.GetComponent<CameraVisibilityChecker>().SetCamera(other.gameObject.GetComponentInChildren<Camera>());
            }
        }
    }

    private IEnumerator UnsubscribeOnNextUpdate()
    {
        yield return null;
        dispatcher.Unsubscribe(EBEventType.RoomSpawningTrigger, address);
    }

}
