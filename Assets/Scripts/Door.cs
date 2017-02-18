using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    [SerializeField]
    private float minAngle = 0;
    [SerializeField]    
    private float maxAngle = 90;
    [SerializeField]
    private float initialAngle = 0;
    [SerializeField]
    private float rotationSpeed = 10;

    private float angle;

    public void setAngle(float angle)
    {
        this.angle = Mathf.Clamp(angle, minAngle, maxAngle);
    }

    public void open()
    {
        StartCoroutine(animateRotation(maxAngle));
    }

    public void close()
    {
        StartCoroutine(animateRotation(minAngle));
    }

    // Use this for initialization
    void Start () {
        initialAngle = Mathf.Clamp(initialAngle, minAngle, maxAngle);
        angle = initialAngle;
	}
	
	// Update is called once per frame
	void Update () {
        //float deltaT = Time.deltaTime;
        //angle += deltaT * 10;
        //if (angle > maxAngle)
        //{
        //    angle = minAngle;
        //}
        //applyAngle(angle);
	}

    private void applyAngle(float angle)
    {
        var rotation = transform.eulerAngles;
        rotation.y = angle;
        transform.eulerAngles = rotation;
    }

    private IEnumerator animateRotation(float targetAngle)
    {
        while (angle < targetAngle)
        {
            angle += Time.deltaTime * rotationSpeed;
            applyAngle(angle);
            yield return null;
        }
    }
}
