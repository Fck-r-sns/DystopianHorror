using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaylightLamp : MonoBehaviour
{

    public enum State
    {
        Off,
        On,
        Broken
    }

    [SerializeField]
    private State initialState = State.Off;

    [SerializeField]
    private GameObject offObject;

    [SerializeField]
    private GameObject onObject;

    [SerializeField]
    private GameObject brokenObject;

    private Dictionary<State, GameObject> objects = new Dictionary<State, GameObject>();
    private State state;

    // Use this for initialization
    void Start()
    {
        objects.Add(State.Off, offObject);
        objects.Add(State.On, onObject);
        objects.Add(State.Broken, brokenObject);
        ApplyState(initialState);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ApplyState(State newState)
    {
        objects[state].SetActive(false);
        state = newState;
        objects[state].SetActive(true);
    }

}
