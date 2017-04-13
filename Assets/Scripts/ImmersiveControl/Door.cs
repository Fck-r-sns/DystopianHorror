using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour
{

    public enum Direction
    {
        Forward = +1,
        Backward = -1
    }

    [SerializeField]
    private bool locked = false;

    [SerializeField]
    private float closeAngle = 0;

    [SerializeField]
    private float openAngle = 90;

    [SerializeField]
    private float initialAngle = 0;

    [SerializeField]
    private float rotationSpeed = 10;

    [SerializeField]
    private AudioClip[] moveSounds;

    [SerializeField]
    private AudioClip[] closeSounds;

    [SerializeField]
    private AudioClip[] lockedSounds;

    private float minAngle;
    private float maxAngle;
    private float openDirection;
    private float zeroShift;
    private float angle;
    private Coroutine lastCoroutine = null;
    private AudioSource audioSource;

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
        if (locked)
        {
            PlayLockedSound();
            return;
        }
        if (lastCoroutine != null)
        {
            StopSound();
            StopCoroutine(lastCoroutine);
        }
        lastCoroutine = StartCoroutine(animateRotation(openAngle + zeroShift, openDirection));
    }

    public void setOpened()
    {
        setAngle(openAngle);
    }

    public void close()
    {
        if (lastCoroutine != null)
        {
            StopSound();
            StopCoroutine(lastCoroutine);
        }
        lastCoroutine = StartCoroutine(animateRotation(closeAngle + zeroShift, -openDirection));
    }

    public void setClosed()
    {
        setAngle(closeAngle);
    }

    public void toggle()
    {
        float median = (openAngle + closeAngle) / 2.0f;
        float a = getAngle();
        if (a * openDirection < median * openDirection)
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
        zeroShift = transform.eulerAngles.y;
        minAngle = Mathf.Min(closeAngle, openAngle);
        maxAngle = Mathf.Max(closeAngle, openAngle);
        openDirection = Mathf.Sign(openAngle - closeAngle);
        initialAngle = Mathf.Clamp(initialAngle, minAngle, maxAngle);
        setAngle(initialAngle);
        audioSource = GetComponent<AudioSource>();
    }

    private void applyAngle(float angle)
    {
        var rotation = transform.eulerAngles;
        rotation.y = angle;
        transform.eulerAngles = rotation;
    }

    private IEnumerator animateRotation(float targetAngle, float directionMultiplier)
    {
        PlayMoveSound();
        while (angle * directionMultiplier < targetAngle * directionMultiplier)
        {
            addAngle(Time.deltaTime * rotationSpeed * directionMultiplier);
            yield return null;
        }
        if (getAngle() == closeAngle)
        {
            PlayCloseSound();
        }
    }

    private void PlayRandomSound(AudioClip[] soundsCollection)
    {
        if (soundsCollection.Length == 0)
        {
            return;
        }
        int n = Random.Range(0, soundsCollection.Length);
        audioSource.clip = soundsCollection[n];
        audioSource.PlayOneShot(audioSource.clip);
    }

    private void PlayMoveSound()
    {
        PlayRandomSound(moveSounds);
    }

    private void PlayCloseSound()
    {
        PlayRandomSound(closeSounds);
    }

    private void PlayLockedSound()
    {
        PlayRandomSound(lockedSounds);
    }

    private void StopSound()
    {
        audioSource.Stop();
    }
}
