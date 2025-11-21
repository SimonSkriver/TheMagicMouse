// PlayerController.cs (Teammate's Script + Integrated Cutscene Logic)
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Teammate's Original Variables & Methods
    [Header("Components")]
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator anim;

    [Header("Movement settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float jumpCutMultiplier = 0.5f;
    [SerializeField] int maxJumps = 1; // Set to 1 for a standard double jump
    [SerializeField] float normalGravity;
    [SerializeField] float jumpingGravity;
    [SerializeField] float fallingGravity;
    [SerializeField] float wallSlideSpeed = 2f;
    [SerializeField] Vector2 wallJumpPower = new Vector2(5f, 18f);
    [SerializeField] float wallJumpLockDuration = 0.2f;
    [SerializeField] float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float wallJumpLockTimer;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool jumpReleased;
    private int jumpsRemaining;
    private bool isWallSliding;
    private bool isRunning;
    private bool isStanding;
    private float wallJumpDirection;
    private float movementLockTimer;

    [Header("Ground check settings")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius;
    private bool isGrounded;

    [Header("Wall check settings")]
    public Transform wallCheck;
    public LayerMask wallLayer;
    public float wallCheckRadius;
    private bool isWalled;

    [Header("Animation settings")]
    [SerializeField] AnimationClip jumpAnimation;

    private float targetGravity;
    private bool isFacingRight = true;

    void Start()
    {
        rb.gravityScale = normalGravity;
        jumpsRemaining = maxJumps;
    }

    void Update()
    {
        // These methods need to run even during cutscenes for our routines to work.
        CheckGrounded();
        CheckWalled();
        ApplyVariableGravity();
        if (movementLockTimer > 0)
        {
            movementLockTimer -= Time.deltaTime;
        }



        if (isInCutscene) return; // --- MASTER GATE FOR PLAYER-ONLY ACTIONS ---

        // Original Update Logic
        CheckRunning();
        Flip();
        HandleWallJump();
        HandleCoyoteTime();
        HandleAnimations();
    }

    void FixedUpdate()
    {
        // We let FixedUpdate run, but gate the methods inside.
        wallJumpLockTimer -= Time.fixedDeltaTime;
        if (wallJumpLockTimer <= 0f)
        {
            HandleMovement();
        }

        rb.gravityScale = targetGravity;

        HandleJumping();
        HandleWallSlide();
    }

    void HandleMovement()
    {
        if (movementLockTimer > 0) return;

        if (isInCutscene) return; // --- SURGICAL GATE ---
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    void HandleJumping()
    {
        if (isInCutscene) return; // --- SURGICAL GATE ---

        if (jumpPressed)
        {
            if (!isGrounded)
            {
                jumpsRemaining--;
            }
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpPressed = false;
        }

        if (jumpReleased)
        {
            if (rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            }
            jumpReleased = false;
        }
    }

    void HandleWallSlide()
    {
        if (isInCutscene) return; // --- SURGICAL GATE ---

        if (isWalled && !isGrounded && moveInput.x != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }

    void HandleWallJump()
    {
        if (isInCutscene) return; // --- SURGICAL GATE ---

        // Note: This is hardcoded to the space key. For full Input System support,
        // this logic would need to move into the OnJump method.
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isWallSliding)
        {
            wallJumpDirection = isFacingRight ? -1 : 1;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpLockTimer = wallJumpLockDuration;
        }
    }

    void HandleCoyoteTime()
    {
        if (isGrounded) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;
    }

    void OnMove(InputValue value)
    {
        if (isInCutscene) { moveInput = Vector2.zero; return; } // --- INPUT GATE ---
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (isInCutscene) return; // --- INPUT GATE ---

        if (value.isPressed)
        {
            if (isGrounded || coyoteTimeCounter > 0f || jumpsRemaining > 0)
            {
                jumpPressed = true;
            }
        }
        else
        {
            jumpReleased = true;
            coyoteTimeCounter = 0;
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

    void CheckRunning()
    {
        if (moveInput.x != 0) isRunning = true;
        else isRunning = false;
    }

    void ApplyVariableGravity()
    {
        if (rb.linearVelocity.y < -0.1f) targetGravity = fallingGravity;
        else if (rb.linearVelocity.y > 0.1f) targetGravity = jumpingGravity;
        else targetGravity = normalGravity;
    }

    void Flip()
    {
        if (isInCutscene) return; // --- SURGICAL GATE ---

        if (isFacingRight && moveInput.x < 0 || !isFacingRight && moveInput.x > 0)
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(transform.localScale.x * -1f, 1f, 1f);
        }
    }

    void HandleAnimations()
    {
        // Safety check for the Animator component
        if (anim != null)
        {
            anim.SetBool("Running", isRunning);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
    }
    #endregion

    #region --- NEW --- Cutscene Control Methods
    [Header("Cutscene Controls")]
    [Tooltip("The force applied for a scripted jump during a cutscene.")]
    [SerializeField] private Vector2 cutsceneJumpForce = new Vector2(10f, 25f);

    [HideInInspector]
    public bool isInCutscene = false;

    public void EnterCutscene()
    {
        isInCutscene = true;
        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    public void ExitCutscene()
    {
        isInCutscene = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public IEnumerator MoveToPositionRoutine(Vector3 targetPosition)
    {
        isInCutscene = true; // Ensure we are in cutscene mode
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        float direction = Mathf.Sign(targetPosition.x - transform.position.x);

        // Manually flip the sprite
        if (direction > 0 && !isFacingRight)
        {
            isFacingRight = true;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), 1, 1);
        }
        else if (direction < 0 && isFacingRight)
        {
            isFacingRight = false;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), 1, 1);
        }

        while (Mathf.Abs(transform.position.x - targetPosition.x) > 0.1f)
        {
            rb.linearVelocity = new Vector2(direction * moveSpeed * 0.8f, rb.linearVelocity.y);
            if (Mathf.Sign(targetPosition.x - transform.position.x) != direction) break;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        transform.position = new Vector2(targetPosition.x, transform.position.y);
        yield return new WaitForFixedUpdate();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    public void PerformCutsceneJump()
    {
        isInCutscene = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        float jumpDirection = isFacingRight ? 1f : -1f;

        rb.linearVelocity = new Vector2(cutsceneJumpForce.x * jumpDirection, cutsceneJumpForce.y);
    }

    public IEnumerator WaitForLandingRoutine()
    {
        isInCutscene = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        yield return new WaitUntil(() => !isGrounded);
        yield return new WaitUntil(() => isGrounded);

        yield return new WaitForSeconds(0.2f);
    }

    public void UnlockDoubleJump()
    {
        Debug.Log("Double Jump has been unlocked!");
        maxJumps = 1;
        if (jumpsRemaining < 1 && !isGrounded)
        {
            jumpsRemaining = 1;
        }
    }

    public void Launch(Vector2 velocity, float duration)
    {
        // Apply the velocity directly
        rb.linearVelocity = velocity;

        // Lock movement controls for a short time so the controller doesn't fight the launch
        movementLockTimer = duration;

        // Treat it as a jump state so gravity behaves correctly
        coyoteTimeCounter = 0;
    }
    #endregion
}