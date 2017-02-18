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

        private Camera camera;

        // Use this for initialization
        void Start()
        {
            camera = GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            Ray ray = camera.ScreenPointToRay(new Vector3(camera.pixelWidth / 2.0f, camera.pixelHeight / 2.0f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.distance < interactionDistance)
            {
                StartCoroutine(SphereIndicator(hit.point));
                Renderer renderer = hit.transform.gameObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.red;
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