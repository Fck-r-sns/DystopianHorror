using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomScene : MonoBehaviour
{

    [SerializeField]
    private CompositePredicate predicate;

    [SerializeField]
    private Transform[] collectiblePlaceholders;

    [SerializeField]
    private Transform[] wakeUpPositions;

    private Scene scene;
    private string sceneName;
    private bool collectibleFound = false;

    public bool CheckPredicate(WorldState world)
    {
        if (predicate == null)
        {
            return true;
        }
        return predicate.Check(world);
    }

    public bool IsWakeUpRoom()
    {
        return (wakeUpPositions != null) && (wakeUpPositions.Length > 0);
    }

    public GameObject GetRoot()
    {
        return gameObject;
    }

    public string GetSceneName()
    {
        return sceneName;
    }

    public void ClearCollectibles()
    {
        if (collectiblePlaceholders == null)
        {
            return;
        }
        foreach (Transform placeholder in collectiblePlaceholders)
        {
            foreach (Transform child in placeholder)
            {
                child.parent = null;
                child.gameObject.SetActive(false);
            }
        }
    }

    public Transform GetCollectiblePlaceholder()
    {
        if (collectibleFound || (collectiblePlaceholders == null) || (collectiblePlaceholders.Length == 0))
        {
            return null;
        }
        int index = Random.Range(0, collectiblePlaceholders.Length);
        return collectiblePlaceholders[index];
    }

    public Transform GetWakeUpPosition()
    {
        int index = Random.Range(0, wakeUpPositions.Length);
        return wakeUpPositions[index];
    }

    public void SetScene(Scene scene)
    {
        this.scene = scene;
    }

    public void SetSceneName(string sceneName)
    {
        this.sceneName = sceneName;
    }

    public void SetEnabled(bool enabled)
    {
        gameObject.SetActive(enabled);
    }

    public bool IsValid()
    {
        return scene.IsValid();
    }

    // Use this for initialization
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
