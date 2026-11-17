using UnityEngine;

public class RingPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        var controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.CollectRing();
        }

        Destroy(gameObject);
    }
}
