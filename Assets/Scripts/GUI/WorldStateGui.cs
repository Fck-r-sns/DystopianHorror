using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldStateGui : MonoBehaviour
{

    [SerializeField]
    private WorldState worldState;

    private Text location;
    private Text roomsVisited;
    private Text madness;
    private Text collectibles;
    private Text keyFound;
    private Text caught;
    private Text fps;

    void Start () {
        location = transform.Find("LocationValue").gameObject.GetComponent<Text>();
        roomsVisited = transform.Find("RoomsValue").gameObject.GetComponent<Text>();
        madness = transform.Find("MadnessValue").gameObject.GetComponent<Text>();
        collectibles = transform.Find("CollectiblesValue").gameObject.GetComponent<Text>();
        keyFound = transform.Find("KeyFoundValue").gameObject.GetComponent<Text>();
        caught = transform.Find("CaughtValue").gameObject.GetComponent<Text>();
        fps = transform.Find("FpsValue").gameObject.GetComponent<Text>();
    }
	
	void Update () {
        location.text = worldState.location.ToString("F");
        roomsVisited.text = "" + worldState.roomsVisited;
        madness.text = "" + worldState.madness;
        collectibles.text = "" + worldState.collectiblesFound;
        keyFound.text = "" + worldState.keyFound;
        caught.text = "" + worldState.timesCaughtByMonster;
        fps.text = "" + (1.0f / Time.deltaTime);
	}
}
