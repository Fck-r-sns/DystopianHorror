using UnityEngine;
using EventBus;

public class MaterialChanger : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private Material normalMaterial;

    [SerializeField]
    private Material madMaterial;

    private int address = AddressProvider.GetFreeAddress();

    public void OnReceived(EBEvent e)
    {
        switch (e.type)
        {
            case EBEventType.ChangeWallsToNormalRequest:
                GetComponent<Renderer>().material = normalMaterial;
                break;

            case EBEventType.ChangeWallsToMadRequest:
                GetComponent<Renderer>().material = madMaterial;
                break;
        }
    }

    private void Start () {
        Dispatcher.Subscribe(EBEventType.ChangeWallsToNormalRequest, address, gameObject);
        Dispatcher.Subscribe(EBEventType.ChangeWallsToMadRequest, address, gameObject);
    }

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.ChangeWallsToNormalRequest, address);
        Dispatcher.Unsubscribe(EBEventType.ChangeWallsToMadRequest, address);
    }

}
