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

    private float zeroShift;
    private float angle;

    public void setAngle(float angle)
    {
        this.angle = Mathf.Clamp(angle, minAngle, maxAngle) + zeroShift;
        applyAngle(this.angle);
    }

    public float getAngle()
    {
        return angle - zeroShift;
    }

    public void addAngle(float angle)
    {
        setAngle(getAngle() + angle);
    }

    public void open()
    {
        StartCoroutine(animateRotation(maxAngle, 1.0f));
    }

    public void close()
    {
        StartCoroutine(animateRotation(minAngle, -1.0f));
    }

    // Use this for initialization
    void Start()
    {
        initialAngle = Mathf.Clamp(initialAngle, minAngle, maxAngle);
        zeroShift = transform.eulerAngles.y;
        setAngle(initialAngle);
    }

    private void applyAngle(float angle)
    {
        var rotation = transform.eulerAngles;
        rotation.y = angle;
        transform.eulerAngles = rotation;
    }

    private IEnumerator animateRotation(float targetAngle, float directionMultiplier)
    {
        while (angle * directionMultiplier < targetAngle * directionMultiplier)
        {
            addAngle(Time.deltaTime * rotationSpeed * directionMultiplier);
            yield return null;
        }
    }
}
