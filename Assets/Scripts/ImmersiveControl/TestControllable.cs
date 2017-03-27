using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immersive {

    public class TestControllable : MonoBehaviour, Controllable {

        private static readonly Color HOVER_COLOR = Color.red;

        private Renderer renderer;
        private Color previousColor;
        
        public void OnHoverOn(Vector3 from)
        {
            Debug.Log("OnHoverOn");
            previousColor = renderer.material.color;
            renderer.material.color = HOVER_COLOR;
        }

        public void OnHoverOut(Vector3 from)
        {
            Debug.Log("OnHoverOut");
            renderer.material.color = previousColor;
        }

        public void OnAcquire(Vector3 from)
        {
            Debug.Log("OnAcquire");
        }

        public void OnRelease(Vector3 from)
        {
            Debug.Log("OnRelease");
        }

        public void OnPress(Vector3 from)
        {
            Debug.Log("OnPress");
        }

        public void OnForceApplied(float xAxis, float yAxis, Vector3 from)
        {
            Debug.Log("OnForceApplied(" + xAxis + "," + yAxis + ")");
            Vector3 forceDirection = transform.position - from;
            transform.Translate(forceDirection * yAxis);
        }

        // Use this for initialization
        void Start() {
            renderer = GetComponent<Renderer>();
        }

        // Update is called once per frame
        void Update() {

        }
    }
}