using UnityEngine;

public class LookRotator : MonoBehaviour {

    [SerializeField]
    private Transform target;

	void Update () {
        transform.LookAt(target);
	}
}
