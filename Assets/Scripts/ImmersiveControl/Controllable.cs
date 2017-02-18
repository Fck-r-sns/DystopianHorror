using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immersive
{

    public interface Controllable
    {
        void OnHoverOn();
        void OnHoverOut();
        void OnAcquire();
        void OnRelease();
        void OnForceApplied();
    }

}