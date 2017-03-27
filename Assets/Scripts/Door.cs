using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public enum Direction
    {
        Forward = +1,
        Backward = -1
    }

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
    private Coroutine lastCoroutine = null;

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
        if (lastCoroutine != null)
        {
            StopCoroutine(lastCoroutine);
        }
        lastCoroutine = StartCoroutine(animateRotation(maxAngle + zeroShift, 1.0f));
    }

    public void setOpened()
    {
        setAngle(maxAngle);
    }

    public void close()
    {
        if (lastCoroutine != null)
        {
            StopCoroutine(lastCoroutine);
        }
        lastCoroutine = StartCoroutine(animateRotation(minAngle + zeroShift, -1.0f));
    }

    public void setClosed()
    {
        setAngle(minAngle);
    }

    public void toggle()
    {
        float median = (maxAngle + minAngle) / 2.0f;
        float a = getAngle();
        if (a < median)
        {
            open();
        }
        else
        {
            close();
        }
    }

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
