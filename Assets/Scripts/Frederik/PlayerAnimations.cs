using System.Collections;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header("Komponenter")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    [Header("Jord Check Indstillinger")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Animation Navne (Triggers/Bools)")]
    [SerializeField] private string jumpStartParam = "JumpStart";
    [SerializeField] private string jumpInAirParam = "IsInAir"; // Bool
    [SerializeField] private string jumpLandParam = "JumpLand";
    [SerializeField] private string doubleJumpParam = "DoubleJump";
    [SerializeField] private string verticalVelocityParam = "VerticalVelocity"; // Float

    // Interne variabler til at holde styr på tilstand
    private bool isGrounded;
    private bool wasGrounded;
    private float lastVerticalVelocity;

    private void Start()
    {
        // Hvis komponenterne ikke er sat i inspectoren, prøv at finde dem selv
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (anim == null) anim = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckGrounded();
        HandleAnimationLogic();
        
        // Opdater "var vi på jorden sidst?" til næste frame
        wasGrounded = isGrounded;
        lastVerticalVelocity = rb.linearVelocity.y;
    }

    private void CheckGrounded()
    {
        // Simpel cirkel check for at se om vi rører jorden
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        // Fortæl animatoren om vi er på jorden eller ej (hvis den bruger det parameter)
        anim.SetBool("IsGrounded", isGrounded);
    }

    private void HandleAnimationLogic()
    {
        // Opdater den vertikale hastighed (god til blend trees)
        anim.SetFloat(verticalVelocityParam, rb.linearVelocity.y);

        // 1. JumpStart (Hoppe start)
        // Hvis vi ikke er på jorden, men VAR på jorden lige før, og vi bevæger os opad
        if (!isGrounded && wasGrounded && rb.linearVelocity.y > 0.1f)
        {
            anim.SetTrigger(jumpStartParam);
        }

        // 2. JumpLand (Landing)
        // Hvis vi ER på jorden nu, men IKKE var det før
        if (isGrounded && !wasGrounded)
        {
            anim.SetTrigger(jumpLandParam);
        }

        // 3. IsInAir (I luften)
        // Simpel bool der er sand når vi ikke rører jorden
        anim.SetBool(jumpInAirParam, !isGrounded);

        // 4. DoubleJump (Dobbelt hop)
        // Hvis vi allerede er i luften, og pludselig får meget fart opad (mere end før)
        // Dette betyder at et "nyt hop" er startet midt i luften
        if (!isGrounded && rb.linearVelocity.y > lastVerticalVelocity + 1f)
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
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
