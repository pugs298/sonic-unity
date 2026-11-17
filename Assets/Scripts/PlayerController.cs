using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 14f;
    public float sprintSpeed = 24f;
    public float acceleration = 8f;
    public float gravity = -24f;
    public float jumpForce = 10f;
    public float turnSpeed = 10f;
    public float boostForce = 4f;

    private CharacterController characterController;
    private Vector3 velocity;
    private Vector3 moveDirection;
    private float currentSpeed;
    private int ringsCollected;
    private bool isGrounded;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(inputX, 0f, inputZ).normalized;
        Vector3 desiredDirection = input;

        if (desiredDirection.magnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(desiredDirection.x, desiredDirection.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
            moveDirection = transform.forward * desiredDirection.magnitude;
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        bool boosting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space);
        float targetSpeed = boosting ? sprintSpeed : walkSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

        if (characterController.isGrounded)
        {
            isGrounded = true;
            velocity.y = -2f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = jumpForce;
                isGrounded = false;
            }
        }

        if (boosting && isGrounded)
        {
            velocity.y = Mathf.Max(velocity.y, 2.5f);
        }

        velocity.y += gravity * Time.deltaTime;
        Vector3 move = moveDirection * currentSpeed * Time.deltaTime;
        characterController.Move(move + new Vector3(0f, velocity.y * Time.deltaTime, 0f));

        if (characterController.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        if (transform.position.y < -20f)
        {
            transform.position = new Vector3(0f, 2f, 0f);
            velocity = Vector3.zero;
        }
    }

    public void CollectRing()
    {
        ringsCollected++;
        Debug.Log("Rings: " + ringsCollected);
    }

    public int GetRingsCollected()
    {
        return ringsCollected;
    }
}
