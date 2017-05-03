using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDirector : MonoBehaviour
{

    [SerializeField]
    private float rotationAnimationTime = 4.0f;

    [SerializeField]
    private float movementAnimationTime = 4.0f;

    private Camera camera;
    private float animationStartTime = 0.0f;
    private Quaternion fromRotation;
    private Quaternion toRotation;
    private Vector3 fromMovement;
    private Vector3 toMovement;
    private bool finished = false;

    public void StartAnimating(Camera camera, Transform rotationTarget, Transform movementTarget)
    {
        this.camera = camera;

        fromRotation = camera.transform.rotation;
        toRotation = Quaternion.LookRotation(rotationTarget.position - camera.transform.position);

        fromMovement = camera.transform.position;
        toMovement = movementTarget.position;

        animationStartTime = Time.time;
        StartCoroutine(Animation());
    }

    public bool IsFinished ()
    {
        return finished;
    }

    private IEnumerator Animation()
    {
        float rotationT = 0.0f;
        float movementT = 0.0f;
        while ((rotationT < 1.0f) && (movementT < 1.0f))
        {
            rotationT = (Time.time - animationStartTime) / rotationAnimationTime;
            Quaternion rotation = Quaternion.Lerp(fromRotation, toRotation, rotationT);
            camera.transform.rotation = rotation;

            movementT = (Time.time - animationStartTime) / movementAnimationTime;
            Vector3 position = Vector3.Lerp(fromMovement, toMovement, movementT);
            camera.transform.position = position;

            yield return null;
        }

        finished = true;
    }
}
