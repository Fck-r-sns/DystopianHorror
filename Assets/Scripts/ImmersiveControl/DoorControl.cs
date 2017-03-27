using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immersive {

    [RequireComponent(typeof(Door))]
    public class DoorControl : MonoBehaviour, Controllable
    {

        [SerializeField]
        private float forceMultiplier = 3.5f;

        private Door door;
        private float direction = 1.0f;

        private void Start()
        {
            door = GetComponent<Door>();
        }

        public void OnHoverOn(Vector3 from)
        {
        }

        public void OnHoverOut(Vector3 from)
        {
        }

        public void OnAcquire(Vector3 from)
        {
            direction = Mathf.Sign(Vector3.Dot(transform.forward, from - transform.position));
        }

        public void OnRelease(Vector3 from)
        {
        }

        public void OnPress(Vector3 from)
        {
            door.open();
        }

        public void OnForceApplied(float xAxis, float yAxis, Vector3 from)
        {
            Vector2 force = (new Vector2(xAxis, yAxis) * direction).normalized * forceMultiplier;
            door.addAngle(force.y + force.x);
        }
    }

}
