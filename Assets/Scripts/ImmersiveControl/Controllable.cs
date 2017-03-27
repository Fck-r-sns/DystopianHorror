using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immersive
{

    public interface Controllable
    {
        void OnHoverOn(Vector3 from);
        void OnHoverOut(Vector3 from);
        void OnAcquire(Vector3 from);
        void OnRelease(Vector3 from);
        void OnPress(Vector3 from);
        void OnForceApplied(float xAxis, float yAxis, Vector3 from);
    }

}