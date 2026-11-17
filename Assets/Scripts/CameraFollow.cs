using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    private Vector3 velocity;
    private float yaw;
    private float pitch = 14f;
    private float distance = 10f;

    public void SetTarget(Transform followTarget)
    {
        target = followTarget;
        if (target != null) yaw = target.eulerAngles.y;
    }

    private void LateUpdate()
    {
        if (target == null) return;
        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * 3f;
            pitch = Mathf.Clamp(pitch - Input.GetAxis("Mouse Y") * 2f, 5f, 35f);
        }
        else yaw = Mathf.LerpAngle(yaw, target.eulerAngles.y, Time.deltaTime * 1.6f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 focus = target.position + Vector3.up * 1.5f + target.forward * 2f;
        Vector3 desired = focus - rotation * Vector3.forward * distance;
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, 0.13f);
        transform.rotation = Quaternion.LookRotation(focus - transform.position, Vector3.up);
    }
}
