using UnityEngine;

using EventBus;
using System.Collections;

public class QuotesShower : MonoBehaviour, IEventSubscriber
{
    [SerializeField]
    private GameFlowManager gameFlowManager;

    [SerializeField]
    private TextOutput textOutput;

    private Dispatcher dispatcher;
    private int address;

    public void OnReceived(EBEvent e)
    {
        if (e.type == EBEventType.ItemCollected)
        {
            ItemCollectedEvent ice = e as ItemCollectedEvent;
            if (ice.item.GetItemType() == CollectibleItem.Type.Book)
            {
                StartCoroutine(ShowQuote());
            }
        }
    }

    private void Start()
    {
        dispatcher = Dispatcher.GetInstance();
        address = dispatcher.GetFreeAddress();
        dispatcher.Subscribe(EBEventType.ItemCollected, address, gameObject);
    }

    private void OnDestroy()
    {
        dispatcher.Unsubscribe(EBEventType.ItemCollected, address);
    }

    private IEnumerator ShowQuote()
    {
        gameFlowManager.SetPauseAllowed(false);
        gameFlowManager.SetKeepMouseLock(true);
        gameFlowManager.PauseGame(false);
        textOutput.ShowText(TextManager.GetQuote(), TextOutput.TextAreaSize.Small);
        yield return new WaitWhile(() => textOutput.IsActive());
        gameFlowManager.SetPauseAllowed(true);
        gameFlowManager.SetKeepMouseLock(false);
        gameFlowManager.ResumeGame(false);
    }

}
