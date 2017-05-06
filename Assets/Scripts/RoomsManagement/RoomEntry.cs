using UnityEngine;
using EventBus;

public class RoomEntry : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private int id;

    [SerializeField]
    private bool spawningEnabled = true;

    [SerializeField]
    private Vector3 rootOffset;

    [SerializeField]
    private Vector3 rootRotation;

    [SerializeField]
    private CompositePredicate compositePredicate;

    private Dispatcher dispatcher;
    private int address;
    private RoomScene lastScene;
    private RoomsManager roomsManager;
    private ItemsManager itemsManager;
    private Door door;

    public int GetId()
    {
        return id;
    }

    public Vector3 GetRootOffset()
    {
        return transform.TransformPoint(rootOffset);
    }

    public Vector3 GetRootRotation()
    {
        return rootRotation;
    }

    public bool IsSpawningEnabled()
    {
        return spawningEnabled;
    }

    public void SetSpawningEnabled(bool enabled)
    {
        spawningEnabled = enabled;
    }

    public bool CheckPredicate(WorldState world)
    {
        return (compositePredicate != null) && compositePredicate.Check(world);
    }

    public void OnReceived(EBEvent e)
    {
        switch (e.type)
        {
            case EBEventType.RoomSpawningTrigger:
                RoomSpawningTriggerEvent rstee = (e as RoomSpawningTriggerEvent);
                if (rstee.action == TriggerAction.Enter)
                {
                    if ((rstee.roomEntryId == id) && spawningEnabled)
                    {
                        roomsManager.DisableAllRoomScenes();
                        RoomScene room = roomsManager.GetRandomRoomScene();
                        AttachRoom(room);
                        SetSpawningEnabled(false);

                        room.ClearCollectibles();
                        Transform itemPlace = room.GetCollectiblePlaceholder();
                        if (itemPlace != null)
                        {
                            CollectibleItem item = itemsManager.GetItem();
                            if (item != null)
                            {
                                item.transform.parent = itemPlace;
                                item.transform.position = itemPlace.position;
                                item.transform.rotation = itemPlace.rotation;
                                item.SetRoomScene(room);
                                item.gameObject.SetActive(true);
                            }
                        }
                    }

                    if (rstee.roomEntryId != id)
                    {
                        SetSpawningEnabled(true);
                    }
                }
                break;

            case EBEventType.DoorClosingTrigger:
                DoorClosingTriggerEvent dcte = (e as DoorClosingTriggerEvent);
                if ((dcte.roomEntryId == id) && (dcte.action == TriggerAction.Exit))
                {
                    CloseDoor();
                }
                break;

            case EBEventType.HallMovingTriggerEntered:
                CloseDoor();
                break;
        }
    }

    void Start()
    {
        roomsManager = RoomsManager.GetManager();
        roomsManager.RegisterRoomEntry(this);
        itemsManager = ItemsManager.GetInstance();
        door = GetComponentInChildren<Door>();
        dispatcher = Dispatcher.GetInstance();
        address = dispatcher.GetFreeAddress();
        dispatcher.Subscribe(EBEventType.RoomSpawningTrigger, address, gameObject);
        dispatcher.Subscribe(EBEventType.DoorClosingTrigger, address, gameObject);
        dispatcher.Subscribe(EBEventType.HallMovingTriggerEntered, address, gameObject);
    }

    private void OnDestroy()
    {
        dispatcher.Unsubscribe(EBEventType.RoomSpawningTrigger, address);
        dispatcher.Unsubscribe(EBEventType.DoorClosingTrigger, address);
        dispatcher.Unsubscribe(EBEventType.HallMovingTriggerEntered, address);
    }

    public void CloseDoor()
    {
        door.Close();
    }

    public void AttachRoom(RoomScene scene)
    {
        if (lastScene != null && lastScene.IsValid())
        {
            if (lastScene.GetSceneName().Equals(scene.name))
            {
                MoveScene(scene);
                return;
            }
            else
            {
                lastScene.SetEnabled(false);
            }
        }
        MoveScene(scene);
        lastScene = scene;
    }

    private void MoveScene(RoomScene scene)
    {
        GameObject root = scene.GetRoot();
        root.transform.position = GetRootOffset();
        root.transform.eulerAngles = GetRootRotation();
        scene.SetEnabled(true);
    }

}
