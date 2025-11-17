using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header ("Components")]
    public Rigidbody2D rb;
    public SpriteRenderer sr; 
    public Animator anim;

    [Header ("Movement settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float jumpCutMultiplier = 0.5f; // How much jump velocity is removed when button is released
    [SerializeField] int maxJumps = 2;
    [SerializeField] float normalGravity;
    [SerializeField] float jumpingGravity;
    [SerializeField] float fallingGravity;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool jumpReleased;
    private int jumpsRemaining = 2;

    [Header ("Ground check settings")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius;
    private bool isGrounded;
    
    private float targetGravity;
    

    void Start()
    {
        rb.gravityScale = normalGravity;
    }

    void Update()
    {
        ApplyVariableGravity();
        CheckGrounded();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleJumping();
        rb.gravityScale = targetGravity;
    }

    void HandleMovement()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocityY);
    }

    void HandleJumping()
    {
        if (jumpPressed && jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
            jumpPressed = false;
            jumpReleased = false;
            jumpsRemaining --; 
        }
        
        if (jumpReleased)
        {
            if (rb.linearVelocityY > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * jumpCutMultiplier);
            }
            jumpReleased = false;
        }
    }

    void OnMove(InputValue value)
    {
        Debug.Log("Move called");
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Jump called");
            jumpPressed = true;
            jumpReleased = false; 
        }
        else // When jump button is released
        {
            jumpReleased = true;
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            jumpsRemaining = maxJumps;
        }
    }

    void ApplyVariableGravity()
    {
        if (rb.linearVelocityY < -0.1f) // When falling
        {
            targetGravity = fallingGravity; 
        }
        else if (rb.linearVelocityY > 0.1f) // When rising
        {
            targetGravity = jumpingGravity;
        }
        else // When grounded
        {
            targetGravity = normalGravity;
        }
    }

    private void OnDrawGizmosSelected() // This is for debugging purposes. Draws a ring around the groundcheck GameObject
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
