using UnityEngine;

using EventBus;
using System.Collections;

public class QuotesShower : MonoBehaviour, IEventSubscriber
{
    [SerializeField]
    private GameFlowManager gameFlowManager;

    [SerializeField]
    private TextOutput textOutput;

    private int address = AddressProvider.GetFreeAddress();

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
        Dispatcher.Subscribe(EBEventType.ItemCollected, address, gameObject);
    }

    private void OnDestroy()
    {
        Dispatcher.Unsubscribe(EBEventType.ItemCollected, address);
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
