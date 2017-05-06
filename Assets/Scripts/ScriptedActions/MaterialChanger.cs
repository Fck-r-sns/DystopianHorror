using UnityEngine;
using EventBus;

public class MaterialChanger : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private Material normalMaterial;

    [SerializeField]
    private Material madMaterial;

    private Dispatcher dispatcher;
    private int address;

    public void OnReceived(EBEvent e)
    {
        switch (e.type)
        {
            case EBEventType.ChangeStateToNormalRequest:
                GetComponent<Renderer>().material = normalMaterial;
                break;

            case EBEventType.ChangeStateToMadRequest:
                GetComponent<Renderer>().material = madMaterial;
                break;
        }
    }

    private void Start () {
        dispatcher = Dispatcher.GetInstance();
        address = dispatcher.GetFreeAddress();
        dispatcher.Subscribe(EBEventType.ChangeStateToNormalRequest, address, gameObject);
        dispatcher.Subscribe(EBEventType.ChangeStateToMadRequest, address, gameObject);
    }

    private void OnDestroy()
    {
        dispatcher.Unsubscribe(EBEventType.ChangeStateToNormalRequest, address);
        dispatcher.Unsubscribe(EBEventType.ChangeStateToMadRequest, address);
    }

}
