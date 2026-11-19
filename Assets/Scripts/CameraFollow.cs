using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    private Vector3 velocity;
    private float yaw;
    private float pitch = 12f;
    private float distance = 9f;

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
            pitch = Mathf.Clamp(pitch - Input.GetAxis("Mouse Y") * 2f, 4f, 32f);
        }
        else
        {
            yaw = Mathf.LerpAngle(yaw, target.eulerAngles.y, Time.deltaTime * 2.2f);
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 focus = target.position + Vector3.up * 1.45f + target.forward * 2.2f;
        Vector3 desired = focus - rotation * Vector3.forward * distance;

        // Keep the camera clear of the player and add a little forward anticipation.
        desired += target.forward * Mathf.Min(PlayerSpeed(target), 3f) * 0.12f;
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, 0.10f);

        Vector3 lookDirection = focus - transform.position;
        transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        Camera camera = GetComponent<Camera>();
        if (camera != null)
        {
            float speed = PlayerSpeed(target);
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, speed > 28f ? 82f : 76f, Time.deltaTime * 3f);
        }
    }

    private static float PlayerSpeed(Transform target)
    {
        PlayerController player = target.GetComponent<PlayerController>();
        return player != null ? player.Speed : 0f;
    }
}
