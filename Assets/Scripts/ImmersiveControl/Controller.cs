using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immersive
{

    [RequireComponent(typeof(Camera))]
    public class Controller : MonoBehaviour
    {

        [SerializeField]
        private float interactionDistance = 1.0f; // in meters

        [SerializeField]
        private float hoverRadius = 0.25f; // in meters

        [SerializeField]
        private LayerMask layerMask = -1;

        [SerializeField]
        private bool debug = false;

        private const float RAY_ORIGIN_SHIFT = 0.3f; // fix for raycasting with origin inside collider

        private Camera camera;
        private Controllable currentControllable;
        private bool objectAcquired = false;
        private bool movedWhileAcquired = false;

        // Use this for initialization
        void Start()
        {
            camera = GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!objectAcquired)
            {
                Ray ray = camera.ScreenPointToRay(new Vector3(camera.pixelWidth / 2.0f, camera.pixelHeight / 2.0f, 0));
                ray.origin -= ray.direction.normalized * RAY_ORIGIN_SHIFT;
                RaycastHit hit;
                if (Physics.SphereCast(ray, hoverRadius, out hit, interactionDistance + RAY_ORIGIN_SHIFT, layerMask))
                {
                    if (debug)
                    {
                        StartCoroutine(SphereIndicator(hit.point));
                    }
                    Controllable controllable = hit.transform.gameObject.GetComponent<Controllable>();
                    if (controllable != currentControllable)
                    {
                        if (currentControllable != null)
                        {
                            currentControllable.OnHoverOut(transform.position);
                            objectAcquired = false;
                            movedWhileAcquired = false;
                        }
                        if (controllable != null)
                        {
                            controllable.OnHoverOn(transform.position);
                        }
                        currentControllable = controllable;
                    }
                }
                else
                {
                    if (currentControllable != null)
                    {
                        currentControllable.OnHoverOut(transform.position);
                        currentControllable = null;
                    }
                }
            }

            if (currentControllable != null && Input.GetMouseButtonDown(0))
            {
                currentControllable.OnAcquire(transform.position);
                objectAcquired = true;
                movedWhileAcquired = false;
            }

            if (currentControllable != null && Input.GetMouseButtonUp(0) && objectAcquired)
            {
                if (!movedWhileAcquired)
                {
                    currentControllable.OnPress(transform.position);
                }
                currentControllable.OnRelease(transform.position);
                objectAcquired = false;
                movedWhileAcquired = false;
            }

            if (objectAcquired)
            {
                float x = Input.GetAxis("Mouse X");
                float y = Input.GetAxis("Mouse Y");
                if (x != 0.0f && y != 0.0f)
                {
                    currentControllable.OnForceApplied(x, y, transform.position);
                    movedWhileAcquired = true;
                }
            }
        }

        // for raycast debug
        private IEnumerator SphereIndicator(Vector3 pos)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = pos;
            sphere.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            Destroy(sphere.GetComponent<Collider>());

            yield return new WaitForSeconds(1);

            Destroy(sphere);
        }
    }
}