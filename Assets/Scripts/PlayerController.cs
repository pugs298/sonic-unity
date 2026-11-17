using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float acceleration = 34f;
    public float groundFriction = 10f;
    public float airControl = 9f;
    public float maxRunSpeed = 23f;
    public float boostSpeed = 37f;
    public float jumpPower = 11f;
    public float gravity = 27f;

    private CharacterController character;
    private Vector3 horizontalVelocity;
    private float verticalVelocity;
    private Vector3 checkpoint;
    private int rings;
    private float boostMeter = 100f;
    private bool finished;
    private float speedPadTimer;
    private float speedPadSpeed;
    private string statusMessage = "Find the rings, explore the isles, and reach the beacon!";
    private float statusTimer = 4f;

    public float Speed { get { return horizontalVelocity.magnitude; } }
    public int Rings { get { return rings; } }
    public float BoostMeter { get { return boostMeter; } }
    public bool IsGrounded { get { return character != null && character.isGrounded; } }
    public string StatusMessage { get { return statusMessage; } }
    public bool Finished { get { return finished; } }

    private void Awake()
    {
        character = GetComponent<CharacterController>();
        checkpoint = transform.position;
    }

    private void Update()
    {
        if (finished) return;
        float dt = Time.deltaTime;
        statusTimer -= dt;
        if (speedPadTimer > 0f) speedPadTimer -= dt;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Transform cameraTransform = Camera.main != null ? Camera.main.transform : transform;
        Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
        Vector3 input = Vector3.ClampMagnitude(forward * z + right * x, 1f);
        bool boosting = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && boostMeter > 0f;
        float targetSpeed = boosting ? boostSpeed : maxRunSpeed;

        if (input.sqrMagnitude > 0.01f)
        {
            Vector3 desiredVelocity = input * targetSpeed;
            horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, desiredVelocity, (character.isGrounded ? acceleration : airControl) * dt);
            Quaternion facing = Quaternion.LookRotation(input, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, facing, 12f * dt);
        }
        else if (character.isGrounded)
        {
            horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, Vector3.zero, groundFriction * dt);
        }
        else
        {
            horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, Vector3.zero, 0.25f * dt);
        }

        if (speedPadTimer > 0f) horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, speedPadSpeed);
        if (boosting)
        {
            boostMeter = Mathf.Max(0f, boostMeter - 22f * dt);
            if (input.sqrMagnitude < 0.01f && horizontalVelocity.sqrMagnitude > 0.1f)
                horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, horizontalVelocity.normalized * boostSpeed, acceleration * dt);
        }
        else boostMeter = Mathf.Min(100f, boostMeter + 9f * dt);

        if (character.isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;
        if (character.isGrounded && Input.GetKeyDown(KeyCode.Space)) verticalVelocity = jumpPower;
        verticalVelocity -= gravity * dt;

        if (Input.GetKeyDown(KeyCode.E)) TryHomingAttack();
        character.Move((horizontalVelocity + Vector3.up * verticalVelocity) * dt);

        if (transform.position.y < -35f) Respawn();
    }

    private void TryHomingAttack()
    {
        HomingTarget[] targets = FindObjectsOfType<HomingTarget>();
        HomingTarget closest = null;
        float closestDistance = 32f;
        foreach (HomingTarget target in targets)
        {
            if (target == null || target.Used) continue;
            float distance = Vector3.Distance(transform.position + Vector3.up, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = target;
            }
        }
        if (closest != null)
        {
            Vector3 direction = (closest.transform.position - transform.position).normalized;
            horizontalVelocity = Vector3.ProjectOnPlane(direction * 34f, Vector3.up);
            verticalVelocity = Mathf.Max(12f, direction.y * 34f);
            closest.Activate();
        }
    }

    public void CollectRing()
    {
        rings++;
        boostMeter = Mathf.Min(100f, boostMeter + 3f);
        ShowMessage("Ring collected!  " + rings);
    }

    public void AddImpulse(Vector3 impulse)
    {
        horizontalVelocity = Vector3.ProjectOnPlane(impulse, Vector3.up);
        verticalVelocity = impulse.y;
    }

    public void ApplySpeedPad(Vector3 direction, float speed)
    {
        horizontalVelocity = direction.normalized * speed;
        speedPadSpeed = speed;
        speedPadTimer = 0.55f;
        ShowMessage("BOOST!");
    }

    public void SetCheckpoint(Vector3 position, int index)
    {
        checkpoint = position;
        ShowMessage("Checkpoint " + index + " reached");
    }

    public void CompleteZone()
    {
        finished = true;
        ShowMessage("ZONE CLEAR!  Rings: " + rings);
    }

    public void ShowMessage(string message)
    {
        statusMessage = message;
        statusTimer = 2.5f;
    }

    private void Respawn()
    {
        character.enabled = false;
        transform.position = checkpoint;
        character.enabled = true;
        horizontalVelocity = Vector3.zero;
        verticalVelocity = 0f;
        ShowMessage("Back to the last checkpoint");
    }
}
