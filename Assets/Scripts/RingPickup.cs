using UnityEngine;

public class RingPickup : MonoBehaviour
{
    private bool collected;

    private void OnTriggerEnter(Collider other)
    {
        if (collected || !other.CompareTag("Player")) return;
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;
        collected = true;
        player.CollectRing();
        Destroy(gameObject);
    }
}
