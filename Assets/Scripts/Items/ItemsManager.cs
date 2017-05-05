using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject keyPrefab;

    [SerializeField]
    private GameObject[] bookPrefabs;

    [SerializeField]
    private WorldState worldState;

    [SerializeField]
    private CompositePredicate keysSpawningPredicate;

    private static ItemsManager instance;
    private CollectibleItem keys;
    private CollectibleItem[] booksPool;
    private int bookSpawnTries = 0;
    private int keySpawnTries = 0;

    public static ItemsManager GetInstance()
    {
        return instance;
    }

    public CollectibleItem GetItem()
    {
        float chance = 0.0f;
        if ((keysSpawningPredicate != null) && keysSpawningPredicate.Check(worldState))
        {
            chance = GetKeyChance();
            if (Random.value <= chance)
            {
                keySpawnTries = 0;
                return keys;
            }
            else
            {
                ++keySpawnTries;
            }
        }

        chance = GetBookChance();
        if (Random.value <= chance)
        {
            bookSpawnTries = 0;
            int index = Random.Range(0, booksPool.Length);
            return booksPool[index];
        }
        else
        {
            ++bookSpawnTries;
        }
        return null;
    }

    void Awake()
    {
        instance = this;

        GameObject keysObject = Instantiate(keyPrefab);
        keysObject.SetActive(false);
        keys = keysObject.GetComponent<CollectibleItem>();

        booksPool = new CollectibleItem[bookPrefabs.Length];
        for (int i = 0; i < bookPrefabs.Length; ++i)
        {
            GameObject book = Instantiate(bookPrefabs[i]);
            book.SetActive(false);
            booksPool[i] = book.GetComponent<CollectibleItem>();
        }
    }

    private float GetKeyChance()
    {
        return Mathf.Clamp01(1 / (WorldState.KEY_SPAWN_TURNS - keySpawnTries));
    }

    private float GetBookChance()
    {
        if (worldState.roomsVisited == 0)
        {
            return 1.0f; // first room always has book
        }
        return Mathf.Clamp01(worldState.madness / 200.0f + bookSpawnTries * 0.05f); 
    }
}
