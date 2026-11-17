using UnityEngine;

public class SpringLauncher : MonoBehaviour
{
    public float launchPower = 24f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null) player.AddImpulse(transform.forward * 3f + Vector3.up * launchPower);
    }
}
