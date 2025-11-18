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
    [SerializeField] float wallSlideSpeed = 2f;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool jumpReleased;
    private int jumpsRemaining;
    private bool isWallSliding;

    [Header ("Ground check settings")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius;
    private bool isGrounded;

    [Header ("Wall check settings")]
    public Transform wallCheck;
    public LayerMask wallLayer;
    public float wallCheckRadius;
    private bool isWalled;
    
    private float targetGravity;
    private bool isFacingRight = true;
    

    void Start()
    {
        rb.gravityScale = normalGravity;
    }

    void Update()
    {
        ApplyVariableGravity();
        CheckGrounded();
        Flip();
        CheckWalled();
    }

    void FixedUpdate()
    {
        rb.gravityScale = targetGravity;
        HandleMovement();
        HandleJumping();
        HandleWallSlide();
    }

    void HandleMovement()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocityY);
    }

    void HandleJumping()
    {
        if (jumpPressed)
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

    void HandleWallSlide()
    {
        if (isWalled && !isGrounded && moveInput.x != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, Mathf.Max(rb.linearVelocityY, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    } 

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (isGrounded || jumpsRemaining > 0)
            {
                jumpPressed = true;
            }
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

    void CheckWalled()
    {
        isWalled = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);
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

    void Flip()
    {
        if (isFacingRight && moveInput.x < 0 || !isFacingRight && moveInput.x > 0) // If we're facing right but moving left or if we're facing left but moving right.
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnDrawGizmosSelected() // This is for debugging purposes. Draws a ring around the groundcheck GameObject
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
    }
}