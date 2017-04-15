using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaylightLamp : MonoBehaviour
{

    public enum State
    {
        Off,
        On,
        Broken,
        BrokenOn,
    }

    [SerializeField]
    private State initialState = State.Off;

    [SerializeField]
    private GameObject offObject;

    [SerializeField]
    private GameObject onObject;

    [SerializeField]
    private GameObject brokenObject;

    [SerializeField]
    private GameObject brokenOnObject;

    private Dictionary<State, GameObject> objects = new Dictionary<State, GameObject>();
    private State state;

    public void SetState(State state)
    {
        ApplyState(state);
    }

    // Use this for initialization
    void Start()
    {
        objects.Add(State.Off, offObject);
        objects.Add(State.On, onObject);
        objects.Add(State.Broken, brokenObject);
        objects.Add(State.BrokenOn, brokenOnObject);
        ApplyState(initialState);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ApplyState(State newState)
    {
        if (objects.ContainsKey(state))
        {
            objects[state].SetActive(false);
        }

        state = newState;

        if (objects.ContainsKey(state))
        {
            objects[state].SetActive(true);
        }
    }

}
