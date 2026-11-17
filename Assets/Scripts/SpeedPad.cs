using UnityEngine;

public class SpeedPad : MonoBehaviour
{
    public float boostSpeed = 34f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null) player.ApplySpeedPad(transform.forward, boostSpeed);
    }
}
