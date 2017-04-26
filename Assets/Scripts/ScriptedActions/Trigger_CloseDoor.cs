using UnityEngine;

public class Trigger_CloseDoor : MonoBehaviour
{
    [SerializeField]
    private Door door;

    private void OnTriggerEnter(Collider other)
    {
        door.Close();
        door.Lock();
    }
}
