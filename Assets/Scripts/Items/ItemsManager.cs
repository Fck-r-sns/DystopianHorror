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

    private static ItemsManager instance;

    public static ItemsManager GetInstance()
    {
        return instance;
    }

    public GameObject GetItem()
    {
        float chance = GetBookChance() * 100;
        if (Random.Range(0, 99) < chance)
        {
            int index = Random.Range(0, bookPrefabs.Length);
            return Instantiate(bookPrefabs[index]);
        }
        return null;
    }

    void Awake () {
        instance = this;
	}
    

    private float GetBookChance()
    {
        //return (100 * WorldState.BOOK_SPAWN_INITIAL_CHANCE - worldState.collectiblesFound) 
        //    / (100 - worldState.roomsVisited);
        return 1f;
    }
}
