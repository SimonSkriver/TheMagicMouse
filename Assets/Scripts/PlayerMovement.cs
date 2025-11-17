// PlayerMovement.cs (Final with Deceleration and Variable Jump Height)
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Tuning Parameters
    [Header("Feature Toggles")]
    [SerializeField] private bool enableWallMechanics = true;

    [Header("Movement")]
    [Tooltip("The character's maximum running speed.")]
    [SerializeField] private float moveSpeed = 8f;
    [Tooltip("The rate at which the player accelerates and decelerates on the ground.")]
    [SerializeField] private float groundAcceleration = 20f;
    [Tooltip("The rate at which the player can change direction in the air.")]
    [SerializeField] private float airAcceleration = 30f;

    [Header("Jumping")]
    [Tooltip("The initial upward power of a jump.")]
    [SerializeField] private float jumpForce = 13f;
    [Tooltip("How much faster the character falls than they rise.")]
    [SerializeField] private float fallGravityMultiplier = 2.1f;
    [Tooltip("The multiplier applied to upward velocity when the jump button is released.")]
    [Range(0f, 1f)]
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [Tooltip("The upward force applied when jumping off a 'Bouncy' surface.")]
    [SerializeField] private float bounceForce = 30f;

    [Header("Player Forgiveness")]
    [Tooltip("Time in seconds to still be able to jump after leaving a ledge.")]
    [SerializeField] private float coyoteTime = 0.2f;
    [Tooltip("Time in seconds to buffer a jump input before landing.")]
    [SerializeField] private float jumpBufferTime = 0.3f;

    [Header("Wall Mechanics")]
    [Tooltip("How quickly the character slides down a wall.")]
    [SerializeField] private float wallSlideSpeed = 4f;
    [Tooltip("The force of a wall jump off a new wall.")]
    [SerializeField] private Vector2 wallJumpForce = new Vector2(9f, 13f);
    [Tooltip("A multiplier for wall jumps off the same wall, making them weaker.")]
    [Range(0f, 1f)]
    [SerializeField] private float sameWallJumpMultiplier = 0.7f;
    [Tooltip("Time after a wall jump where player input is locked.")]
    [SerializeField] private float wallJumpDirectionControlDelay = 0.15f;
    #endregion

    #region References
    [Header("Object & Layer References")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.43f, 0.03f);
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheckPoint;
    [SerializeField] private Vector2 wallCheckCapsuleSize = new Vector2(0.8f, 0.98f);
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    #endregion

    #region State Variables
    private float moveInput;
    private bool isFacingRight = true;
    private bool isGrounded;
    private float baseGravityScale;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool canMove = true;
    private bool isWallSliding;
    private Transform lastWall;
    private bool isWallJumping;
    private Collider2D currentWallCollider;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        baseGravityScale = rb.gravityScale;
    }

    private void Update()
    {
        #region Checks & Timers
        bool wasGrounded = isGrounded;
        // --- THE BOUNCE FIX ---
        // We now store the collider we hit, not just a true/false.
        Collider2D groundCollider = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);
        isGrounded = groundCollider != null;

        if (isGrounded && !wasGrounded)
        {
            lastWall = null;

            // Check if the ground we just landed on is bouncy.
            if (groundCollider.CompareTag("Bouncy"))
            {
                // If it is, apply the bounce force immediately.
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
            }
        }

        if (isGrounded) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;

        jumpBufferCounter -= Time.deltaTime;
        #endregion

        #region Wall Sliding Logic
        if (enableWallMechanics)
        {
            currentWallCollider = Physics2D.OverlapCapsule(wallCheckPoint.position, wallCheckCapsuleSize, CapsuleDirection2D.Vertical, 0, wallLayer);

            if (currentWallCollider != null && !isGrounded && coyoteTimeCounter <= 0)
            {
                float wallSide = Mathf.Sign(currentWallCollider.transform.position.x - transform.position.x);
                bool isPushingTowardsWall = Mathf.Sign(moveInput) == wallSide;
                if (isPushingTowardsWall) isWallSliding = true;
                else isWallSliding = false;
            }
            else
            {
                isWallSliding = false;
            }
        }
        #endregion

        Flip();
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        ApplyMovement();
        ApplyGravity();
    }

    #region Input Methods
    public void OnMove(InputAction.CallbackContext context) { moveInput = context.ReadValue<Vector2>().x; }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started) jumpBufferCounter = jumpBufferTime;

        // --- VARIABLE JUMP HEIGHT FIX ---
        // If the jump button is released while the player is moving up, cut the jump short.
        if (context.canceled && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            coyoteTimeCounter = 0f; // Prevent coyote time after cutting a jump
        }

        if (jumpBufferCounter > 0f)
        {
            if (isWallSliding && enableWallMechanics) StartCoroutine(WallJump());
            else if (coyoteTimeCounter > 0f) PerformJump();
        }
    }
    #endregion

    #region Core Logic Methods
    private void ApplyMovement()
    {
        // This method handles all horizontal movement. The slope fix is now in ApplyGravity.
        float targetSpeed = moveInput * moveSpeed;
        float accelerationRate = isGrounded ? groundAcceleration : airAcceleration;
        float newHorizontalVelocity = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, accelerationRate * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector2(newHorizontalVelocity, rb.linearVelocity.y);
    }

    private void ApplyGravity()
    {
        // --- DECELERATION & SLOPE FIX ---
        // This is the true "Gold Standard" fix.
        if (isGrounded && moveInput == 0 && Mathf.Abs(rb.linearVelocity.x) < 0.01f)
        {
            // If we have fully decelerated on the ground, turn off gravity to "stick" to slopes.
            rb.gravityScale = 0;
        }
        else if (isWallSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
            isWallJumping = false;
            rb.gravityScale = baseGravityScale;
        }
        else if (isWallJumping)
        {
            rb.gravityScale = baseGravityScale;
            if (rb.linearVelocity.y < 0) isWallJumping = false;
        }
        else if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravityScale * fallGravityMultiplier;
        }
        else
        {
            // In all other cases (moving, jumping, falling), ensure gravity is on.
            rb.gravityScale = baseGravityScale;
        }
    }

    private void PerformJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpBufferCounter = 0f;
        coyoteTimeCounter = 0f;
    }

    private IEnumerator WallJump()
    {
        Vector2 forceToApply = wallJumpForce;

        if (currentWallCollider.transform == lastWall)
        {
            forceToApply *= sameWallJumpMultiplier;
        }

        lastWall = currentWallCollider.transform;
        isWallSliding = false;
        isWallJumping = true;
        canMove = false;
        rb.gravityScale = baseGravityScale;
        rb.linearVelocity = Vector2.zero;

        float wallSide = Mathf.Sign(currentWallCollider.transform.position.x - transform.position.x);
        float jumpDirection = -wallSide;
        float forceX = forceToApply.x * jumpDirection;
        rb.AddForce(new Vector2(forceX, forceToApply.y), ForceMode2D.Impulse);

        Flip();

        yield return new WaitForSeconds(wallJumpDirectionControlDelay);
        canMove = true;
    }
    #endregion

    #region Misc Methods
    private void Flip()
    {
        if (!canMove) return;
        if (moveInput != 0 && isFacingRight != moveInput > 0)
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(groundCheckPoint.position, groundCheckSize);
        Gizmos.color = Color.blue;
        if (wallCheckPoint != null) Gizmos.DrawWireCube(wallCheckPoint.position, wallCheckCapsuleSize);
    }
    #endregion
}