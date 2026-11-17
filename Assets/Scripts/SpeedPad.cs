using UnityEngine;

public class SpeedPad : MonoBehaviour
{
    public float force = 16f;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        var controller = other.GetComponent<CharacterController>();
        if (controller == null)
            return;

        controller.Move(transform.forward * force * Time.deltaTime);
    }
}
