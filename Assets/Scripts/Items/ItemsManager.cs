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
    private Predicate keysSpawningPredicate;

    private static ItemsManager instance;
    private CollectibleItem keys;
    private CollectibleItem[] booksPool;

    public static ItemsManager GetInstance()
    {
        return instance;
    }

    public CollectibleItem GetItem()
    {
        float chance = 0.0f;
        if (keysSpawningPredicate != null && keysSpawningPredicate.Check(worldState))
        {
            chance = GetKeyChance() * 100;
            if (Random.Range(0, 99) < chance)
            {
                return keys;
            }
        }

        chance = GetBookChance() * 100;
        if (Random.Range(0, 99) < chance)
        {
            int index = Random.Range(0, booksPool.Length);
            return booksPool[index];
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
        return 0.3f;
    }

    private float GetBookChance()
    {
        //return (100 * WorldState.BOOK_SPAWN_INITIAL_CHANCE - worldState.collectiblesFound) 
        //    / (100 - worldState.roomsVisited);
        return 1f;
    }
}
