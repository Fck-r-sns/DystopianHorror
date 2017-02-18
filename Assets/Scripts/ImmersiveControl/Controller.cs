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

        private Camera camera;
        private Controllable currentControllable;
        private bool objectAcquired = false;

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
                RaycastHit hit;
                if (Physics.SphereCast(ray, hoverRadius, out hit) && hit.distance < interactionDistance)
                {
                    Controllable controllable = hit.transform.gameObject.GetComponent<Controllable>();
                    if (controllable != currentControllable)
                    {
                        if (currentControllable != null)
                        {
                            currentControllable.OnHoverOut();
                            objectAcquired = false;
                        }
                        if (controllable != null)
                        {
                            controllable.OnHoverOn();
                        }
                        currentControllable = controllable;
                    }
                }
                else
                {
                    if (currentControllable != null)
                    {
                        currentControllable.OnHoverOut();
                        currentControllable = null;
                    }
                }
            }

            if (currentControllable != null && Input.GetMouseButtonDown(0))
            {
                currentControllable.OnAcquire();
                objectAcquired = true;
            }

            if (currentControllable != null && Input.GetMouseButtonUp(0) && objectAcquired)
            {
                currentControllable.OnRelease();
                objectAcquired = false;
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