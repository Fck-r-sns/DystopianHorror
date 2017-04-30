using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolBell : MonoBehaviour
{

    public enum State
    {
        Normal,
        Mad,
    }

    [SerializeField]
    private State initialState = State.Normal;

    [SerializeField]
    private GameObject normalObject;

    [SerializeField]
    private GameObject madObject;

    [SerializeField]
    private AudioSource audioSource;

    private Dictionary<State, GameObject> objects = new Dictionary<State, GameObject>();
    private State state;

    public void SetState(State state)
    {
        ApplyState(state);
    }

    public void StartRinging()
    {
        audioSource.Play();
    }

    public void StopRinging()
    {
        audioSource.Stop();
    }

    // Use this for initialization
    void Start()
    {
        objects.Add(State.Normal, normalObject);
        objects.Add(State.Mad, madObject);
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
