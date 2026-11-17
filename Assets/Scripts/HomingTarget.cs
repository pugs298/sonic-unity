using UnityEngine;

public class HomingTarget : MonoBehaviour
{
    public bool Used { get; private set; }
    private Vector3 startPosition;
    private Renderer targetRenderer;

    private void Start()
    {
        startPosition = transform.position;
        targetRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (!Used) transform.position = startPosition + Vector3.up * Mathf.Sin(Time.time * 2f + startPosition.z) * 0.3f;
        transform.Rotate(0f, 100f * Time.deltaTime, 0f);
    }

    public void Activate()
    {
        if (Used) return;
        Used = true;
        if (targetRenderer != null) targetRenderer.enabled = false;
        Collider targetCollider = GetComponent<Collider>();
        if (targetCollider != null) targetCollider.enabled = false;
        Destroy(gameObject, 1.2f);
    }
}
