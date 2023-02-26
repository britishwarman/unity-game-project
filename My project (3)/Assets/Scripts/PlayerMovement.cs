using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 10f;
    public float dashDistance = 5f;
    public float dashTime = 0.5f;
    public float dashCooldown = 1f;

    private float currentSpeed;
    private bool isGrounded = true;
    private bool isDashing = false;
    private bool canDash = true;
    private Vector3 dashDirection;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        // Move player left and right
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * currentSpeed * Time.deltaTime;
        transform.Translate(movement, Space.Self);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        // Dash
        if (Input.GetButtonDown("Dash") && canDash)
        {
            dashDirection = transform.forward;
            isDashing = true;
            canDash = false;
            Invoke(nameof(ResetDash), dashCooldown);
            Invoke(nameof(StopDash), dashTime);
        }

        // Walk/Run
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }

    void FixedUpdate()
    {
        // Apply dash force
        if (isDashing)
        {
            rb.AddForce(dashDirection * dashDistance, ForceMode.VelocityChange);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if player is grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isDashing = false;
        }
    }

    void ResetDash()
    {
        canDash = true;
    }

    void StopDash()
    {
        isDashing = false;
    }
}
