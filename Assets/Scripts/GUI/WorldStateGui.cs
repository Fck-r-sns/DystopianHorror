using UnityEngine;
using UnityEngine.UI;

public class WorldStateGui : MonoBehaviour
{

    [SerializeField]
    private WorldState worldState;

    private GameObject worldStateWidget;
    private Text location;
    private Text roomsVisited;
    private Text madness;
    private Text collectibles;
    private Text keyFound;
    private Text caught;
    private Text fps;

    void Start () {
        worldStateWidget = transform.Find("Canvas/WorldState").gameObject;
        location = transform.Find("Canvas/WorldState/LocationValue").gameObject.GetComponent<Text>();
        roomsVisited = transform.Find("Canvas/WorldState/RoomsValue").gameObject.GetComponent<Text>();
        madness = transform.Find("Canvas/WorldState/MadnessValue").gameObject.GetComponent<Text>();
        collectibles = transform.Find("Canvas/WorldState/CollectiblesValue").gameObject.GetComponent<Text>();
        keyFound = transform.Find("Canvas/WorldState/KeyFoundValue").gameObject.GetComponent<Text>();
        caught = transform.Find("Canvas/WorldState/CaughtValue").gameObject.GetComponent<Text>();
        fps = transform.Find("Canvas/WorldState/FpsValue").gameObject.GetComponent<Text>();
    }
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.F12))
        {
            worldStateWidget.SetActive(!worldStateWidget.activeSelf);
        }

        location.text = worldState.location.ToString("F");
        roomsVisited.text = "" + worldState.roomsVisited;
        madness.text = "" + worldState.madness;
        collectibles.text = "" + worldState.collectiblesFound;
        keyFound.text = "" + worldState.keyFound;
        caught.text = "" + worldState.timesCaughtByMonster;
        fps.text = "" + (1.0f / Time.deltaTime);
	}
}
