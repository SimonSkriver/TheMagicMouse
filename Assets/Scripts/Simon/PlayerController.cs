using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    // public Animator anim; 

    /*
    [Header("Audio")]
    public AudioSource jumpAudioSource;
    public AudioSource runAudioSource;
    public AudioClip[] jumpClips;
    public AudioClip runClip;
    private bool runClipPlaying;
    */

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
        // Flip(); //flyttet til animations script
        CheckWalled();
        HandleWallJump();
        HandleCoyoteTime();
        // HandleAnimations(); // flyttet til animations script
        //HandleAudio();
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
        if (jumpPressed)
        {
            // Ground Jump
            if (coyoteTimeCounter > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
                jumpsRemaining = maxJumps - 1;
                Debug.Log("Ground Jump executed");
            }
            // Air Jump
            else if (jumpsRemaining > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, secondJumpForce);
                jumpsRemaining--;
                Debug.Log($"Air Jump executed. Jumps remaining: {jumpsRemaining}");
            }

            jumpPressed = false;
            coyoteTimeCounter = 0;
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
            // Use localScale to determine facing direction since isFacingRight might be stale
            // If scale.x is positive, we are facing right, so jump direction is left (-1)
            // If scale.x is negative, we are facing left, so jump direction is right (1)
            wallJumpDirection = transform.localScale.x > 0 ? -1 : 1;
            
            Vector2 force = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            rb.linearVelocity = force;
            Debug.Log($"Walljump called. Direction: {wallJumpDirection}, Force: {force}");

            wallJumpLockTimer = wallJumpLockDuration;
            
            // Consume the jump input so HandleJumping doesn't fire an air jump immediately after
            jumpPressed = false; 
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
                /*
                jumpAudioSource.clip = jumpClips[Random.Range(0, jumpClips.Length)];
                jumpAudioSource.Play();
                */
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
        if (rb.linearVelocityY < -0.1f) 
        {
            targetGravity = fallingGravity;
        }
        else if (rb.linearVelocityY > 0.1f) 
        {
            targetGravity = jumpingGravity;
        }
        else 
        {
            targetGravity = normalGravity;
        }
    }

    /*
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
    */

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, groundCeckSize);
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}