using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immersive {

    [RequireComponent(typeof(Door))]
    public class DoorControl : MonoBehaviour, Controllable
    {

        public enum Mode
        {
            Normal,
            Immersive
        }

        [SerializeField]
        private Mode mode = Mode.Normal;

        [SerializeField]
        private float forceMultiplier = 3.5f;

        [SerializeField]
        private Transform gesturePosition;

        [SerializeField]
        private Texture2D gestureTexture;

        private Door door;
        private float direction = 1.0f;
        private bool drawGesture = false;

        private void Start()
        {
            door = GetComponent<Door>();
        }

        public void OnHoverOn(Vector3 from)
        {
            drawGesture = true;
        }

        public void OnHoverOut(Vector3 from)
        {
            drawGesture = false;
        }

        public void OnAcquire(Vector3 from)
        {
            if (mode == Mode.Immersive)
            {
                direction = Mathf.Sign(Vector3.Dot(transform.forward, from - transform.position));
            }
            else
            {
                door.toggle();
            }
        }

        public void OnRelease(Vector3 from)
        {
        }

        public void OnPress(Vector3 from)
        {
            if (mode == Mode.Immersive)
            {
                door.toggle();
            }
        }

        public void OnForceApplied(float xAxis, float yAxis, Vector3 from)
        {
            if (mode == Mode.Immersive)
            {
                Vector2 force = (new Vector2(xAxis, yAxis) * direction).normalized * forceMultiplier;
                door.addAngle(force.y + force.x);
            }
        }

        private void OnGUI()
        {
            if (drawGesture)
            {
                Camera cam = Camera.main;
                Vector3 pos = gesturePosition.position;
                pos = cam.WorldToViewportPoint(pos);
                pos = cam.ViewportToScreenPoint(pos);
                GUI.DrawTexture(new Rect(pos.x, cam.pixelHeight - pos.y, gestureTexture.width, gestureTexture.height), gestureTexture);
            }
        }
    }

}
