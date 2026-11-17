using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Vector3 checkpointPosition;
    public int index;
    private bool activated;

    private void OnTriggerEnter(Collider other)
    {
        if (activated || !other.CompareTag("Player")) return;
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;
        activated = true;
        player.SetCheckpoint(checkpointPosition, index);
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) renderer.material.color = new Color(0.2f, 1f, 0.85f);
    }
}
