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

    public void SetAngle(float angle)
    {
        this.angle = Mathf.Clamp(angle, minAngle, maxAngle) + zeroShift;
        ApplyAngle(this.angle);
    }

    public float GetAngle()
    {
        return angle - zeroShift;
    }

    public void AddAngle(float angle)
    {
        SetAngle(GetAngle() + angle);
    }

    public void Lock()
    {
        locked = true;
    }

    public void Unlock()
    {
        locked = false;
    }

    public void Open()
    {
        if (GetAngle() == openAngle)
        {
            return;
        }
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
        lastCoroutine = StartCoroutine(AnimateRotation(openAngle + zeroShift, openDirection));
    }

    public void SetOpened()
    {
        SetAngle(openAngle);
    }

    public void Close()
    {
        if (GetAngle() == closeAngle)
        {
            return;
        }
        if (lastCoroutine != null)
        {
            StopSound();
            StopCoroutine(lastCoroutine);
        }
        lastCoroutine = StartCoroutine(AnimateRotation(closeAngle + zeroShift, -openDirection));
    }

    public void SetClosed()
    {
        SetAngle(closeAngle);
    }

    public void Toggle()
    {
        float median = (openAngle + closeAngle) / 2.0f;
        float a = GetAngle();
        if (a * openDirection < median * openDirection)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    void Start()
    {
        zeroShift = transform.eulerAngles.y;
        minAngle = Mathf.Min(closeAngle, openAngle);
        maxAngle = Mathf.Max(closeAngle, openAngle);
        openDirection = Mathf.Sign(openAngle - closeAngle);
        initialAngle = Mathf.Clamp(initialAngle, minAngle, maxAngle);
        SetAngle(initialAngle);
        audioSource = GetComponent<AudioSource>();
    }

    private void ApplyAngle(float angle)
    {
        var rotation = transform.eulerAngles;
        rotation.y = angle;
        transform.eulerAngles = rotation;
    }

    private IEnumerator AnimateRotation(float targetAngle, float directionMultiplier)
    {
        PlayMoveSound();
        while (angle * directionMultiplier < targetAngle * directionMultiplier)
        {
            AddAngle(Time.deltaTime * rotationSpeed * directionMultiplier);
            yield return null;
        }
        if (GetAngle() == closeAngle)
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
