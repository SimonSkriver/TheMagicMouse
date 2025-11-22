using System.Collections;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private string jumpStartParam = "JumpStart";
    [SerializeField] private string jumpInAirParam = "IsInAir"; // Bool
    [SerializeField] private string jumpLandParam = "JumpLand";
    [SerializeField] private string doubleJumpParam = "DoubleJump";

    // Interne variabler til at holde styr på tilstand
    private bool isGrounded;
    private bool wasGrounded;
    private float lastVerticalVelocity;
    private void Update()
    {
        CheckGrounded();
        HandleAnimationLogic();
        // Opdater "var vi på jorden sidst?" til næste frame
        wasGrounded = isGrounded;
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        anim.SetBool("IsGrounded", isGrounded);
    }

    private void HandleAnimationLogic()
    {
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
        if (!isGrounded && !wasGrounded && rb.linearVelocity.y > lastVerticalVelocity + 99f)
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
