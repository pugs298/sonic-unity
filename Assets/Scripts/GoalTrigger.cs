using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null) player.CompleteZone();
    }
}
