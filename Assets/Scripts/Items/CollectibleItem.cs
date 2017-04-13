using UnityEngine;

using Immersive;

public class CollectibleItem : MonoBehaviour, Controllable
{

    public enum Type {
        Key,
        Book
    }

    [SerializeField]
    private Type type = Type.Book;

    public void OnHoverOn(Vector3 from)
    {
        // show HUD
    }

    public void OnHoverOut(Vector3 from)
    {
        // hide HUD
    }

    public void OnAcquire(Vector3 from)
    {
        // take item
    }

    public void OnRelease(Vector3 from)
    {
        // do nothing
    }

    public void OnPress(Vector3 from)
    {
        // do nothing
    }

    public void OnForceApplied(float xAxis, float yAxis, Vector3 from)
    {
        // do nothing
    }

}
