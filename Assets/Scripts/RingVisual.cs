using UnityEngine;

public class RingVisual : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(0f, 95f * Time.deltaTime, 0f, Space.World);
        transform.position += Vector3.up * Mathf.Sin(Time.time * 2f + transform.position.z) * 0.003f;
    }
}
