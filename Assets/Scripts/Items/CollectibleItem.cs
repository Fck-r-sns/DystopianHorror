using UnityEngine;

using EventBus;
using Immersive;

public class CollectibleItem : MonoBehaviour, Controllable
{

    public enum Type
    {
        Key,
        Book
    }

    [SerializeField]
    private Type type = Type.Book;

    [SerializeField]
    private Texture2D gestureTexture;

    private bool drawGesture = false;
    private RoomScene roomScene;

    public Type GetItemType()
    {
        return type;
    }

    public void SetRoomScene(RoomScene scene)
    {
        this.roomScene = scene;
    }

    public RoomScene GetRoomScene()
    {
        return roomScene;
    }

    public void OnHoverOn(Vector3 from)
    {
        // show HUD
        drawGesture = true;
    }

    public void OnHoverOut(Vector3 from)
    {
        // hide HUD
        drawGesture = false;
    }

    public void OnAcquire(Vector3 from)
    {
        gameObject.SetActive(false);
        Dispatcher.SendEvent(new ItemCollectedEvent(this));
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

    private void OnGUI()
    {
        if (drawGesture)
        {
            Camera cam = Camera.main;
            Vector3 pos = transform.position;
            pos = cam.WorldToViewportPoint(pos);
            pos = cam.ViewportToScreenPoint(pos);
            GUI.DrawTexture(new Rect(pos.x, cam.pixelHeight - pos.y, gestureTexture.width, gestureTexture.height), gestureTexture);
        }
    }

}
