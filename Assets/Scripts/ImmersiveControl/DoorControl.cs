using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immersive {

    [RequireComponent(typeof(Door))]
    public class DoorControl : MonoBehaviour, Controllable
    {
        Door door;

        private void Start()
        {
            door = GetComponent<Door>();
        }

        public void OnHoverOn()
        {
        }

        public void OnHoverOut()
        {
        }

        public void OnAcquire()
        {
        }

        public void OnRelease()
        {
        }

        public void OnForceApplied(float xAxis, float yAxis, Vector3 from)
        {
            float direction = Mathf.Sign(Vector3.Dot(transform.forward, from - transform.position));
            door.addAngle(direction * yAxis * 10);
        }

    }

}
