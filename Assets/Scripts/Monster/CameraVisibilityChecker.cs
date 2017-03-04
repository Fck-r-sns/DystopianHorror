using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVisibilityChecker : MonoBehaviour {

    [SerializeField]
    private Camera camera;

    private Collider targetCollider;

    private bool visible = false;

    public bool isVisible()
    {
        return visible;
    }

    private void Awake()
    {
        targetCollider = GetComponent<Collider>();
    }

    private void Update () {
        visible = CheckVisibility();
	}

    private bool CheckVisibility()
    {
        Plane[] cameraPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
        Bounds targetBounds = targetCollider.bounds;
        return GeometryUtility.TestPlanesAABB(cameraPlanes, targetBounds);
    }
}
