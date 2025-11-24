using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private string jumpStartParam = "JumpStart";
    [SerializeField] private string jumpInAirParam = "IsInAir"; // Bool
    [SerializeField] private string jumpLandParam = "JumpLand";
    [SerializeField] private string doubleJumpParam = "DoubleJump";
    [SerializeField] Vector2 groundCeckSize;


    // Interne variabler til at holde styr på tilstand
    private bool isGrounded;
    private bool wasGrounded;
    private float lastVerticalVelocity;
    private Vector2 moveInput;
    private bool isFacingRight = true;

    private void Start()
    {
        if (playerController == null)
        {
            playerController = GetComponent<PlayerController>();
        }
    }

    private void Update()
    {
        CheckGrounded();
        HandleAnimationLogic();
        HandleFlip();
        HandleRunning();
        
        // Opdater "var vi på jorden sidst?" til næste frame
        wasGrounded = isGrounded;
        lastVerticalVelocity = rb.linearVelocity.y;
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCeckSize, 0, groundLayer);

        // Fortæl animatoren om vi er på jorden eller ej (hvis den bruger det parameter)
        anim.SetBool("IsGrounded", isGrounded);
    }

    // Modtager input fra PlayerInput komponenten (Broadcast Messages)
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void HandleFlip()
    {
        // If PlayerController is disabled (e.g. during bounce), do not flip based on input
        if (playerController != null && !playerController.enabled) return;

        // Brug input til at bestemme retning for at undgå flickering
        if (isFacingRight && moveInput.x < 0 || !isFacingRight && moveInput.x > 0)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 ls = transform.localScale;
        ls.x *= -1f;
        transform.localScale = ls;
    }

    public void SetFacingDirection(int direction)
    {
        // direction: 1 for right, -1 for left
        if (direction > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (direction < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void HandleRunning()
    {
        bool isRunning = moveInput.x != 0;
        anim.SetBool("Running", isRunning);
        anim.SetBool("Idle", !isRunning);
    }

    private void HandleAnimationLogic()
    {
        // Opdater den vertikale hastighed (god til blend trees)

        // 1. JumpStart (Hoppe start)
        // Hvis vi ikke er på jorden, men VAR på jorden lige før, og vi bevæger os opad
        if (!isGrounded && wasGrounded && rb.linearVelocity.y > 0.1f)
        {
            anim.SetTrigger(jumpStartParam);
        }

        // 2. JumpLand (Landing)
        // Hvis vi ER på jorden nu, men IKKE var det før, OG vi ikke bevæger os hurtigt opad (f.eks. hvis vi rammer en væg)
        if (isGrounded && !wasGrounded && rb.linearVelocity.y < 0.1f)
        {
            anim.SetTrigger(jumpLandParam);
        }

        // 3. IsInAir (I luften)
        // Simpel bool der er sand når vi ikke rører jorden
        anim.SetBool(jumpInAirParam, !isGrounded);

        // 4. DoubleJump (Dobbelt hop)
        // Hvis vi allerede er i luften (og VAR i luften før), og pludselig får meget fart opad
        // Threshold lowered to 45f to detect double jumps even when rising (60 - 10 = 50 > 45)
        if (!isGrounded && !wasGrounded && rb.linearVelocity.y > lastVerticalVelocity + 45f)
        {
            anim.SetTrigger(doubleJumpParam);
        }
    }

    // Tegn cirklen i editoren så vi kan se den
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCeckSize);
        }
    }
}
