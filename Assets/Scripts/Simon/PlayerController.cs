using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    // public Animator anim; flyttet til animations script

    [Header("Audio")]
    public AudioSource jumpAudioSource;
    public AudioSource runAudioSource;
    public AudioClip[] jumpClips;
    public AudioClip runClip;
    private bool runClipPlaying;

    [Header("Movement settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float secondJumpForce = 60f;
    [SerializeField] float jumpCutMultiplier = 0.5f; // How much jump velocity is removed when button is released
    [SerializeField] int maxJumps = 2;
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
    private int jumpsRemaining;
    private bool isWallSliding;
    private bool isRunning;
    private bool isStanding;
    private float wallJumpDirection;
    private bool isGrounded;

    [Header("Ground check settings")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Vector2 groundCeckSize;

    [Header("Wall check settings")]
    public Transform wallCheck;
    public LayerMask wallLayer;
    public Vector2 wallCheckSize;
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
        CheckRunning();
        // Flip(); // Moved to PlayerAnimations
        CheckWalled();
        HandleWallJump();
        HandleCoyoteTime();
        // HandleAnimations(); // Moved to PlayerAnimations
        HandleAudio();
    }

    void FixedUpdate()
    {
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
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocityY);
    }

    void HandleJumping()
    {
        if (jumpPressed && jumpsRemaining >= 2)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
            jumpPressed = false;
            jumpsRemaining--;
        }

        else if (jumpPressed && jumpsRemaining < 2)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, secondJumpForce);
            jumpPressed = false;
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

    void HandleWallJump()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isWallSliding)
        {
            Debug.Log("Walljump called");
            wallJumpDirection = isFacingRight ? -1 : 1;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpLockTimer = wallJumpLockDuration;
        }
    }

    void HandleCoyoteTime()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
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
            if (coyoteTimeCounter > 0 || jumpsRemaining > 0)
            {
                jumpPressed = true;
                jumpAudioSource.clip = jumpClips[Random.Range(0, jumpClips.Length)];
                jumpAudioSource.Play();

                if (!isGrounded && coyoteTimeCounter <= 0)
                {
                    jumpsRemaining--;
                }
            }
        }
        else // When jump button is released
        {
            coyoteTimeCounter = 0;
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCeckSize, 0, groundLayer);
        if (isGrounded)
        {
            jumpsRemaining = maxJumps;
        }
    }

    void CheckWalled()
    {
        isWalled = Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0, wallLayer);
    }

    void CheckRunning()
    {
        if (moveInput.x != 0)
        {
            isRunning = true;
            isStanding = false;
        }
        else
        {
            isRunning = false;
            isStanding = true;
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

    void HandleAudio()
    {
        if (isRunning && !runClipPlaying)
        {
            runClipPlaying = true;
            runAudioSource.clip = runClip;
            runAudioSource.Play();
        }
        else if (!isRunning)
        {
            runAudioSource.Pause();
            runClipPlaying = false;
        }
    }

    private void OnDrawGizmosSelected() // This is for debugging purposes. Draws a ring around the groundcheck GameObject
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, groundCeckSize);
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}