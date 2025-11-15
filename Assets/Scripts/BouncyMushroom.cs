// BouncyMushroom.cs
using UnityEngine;

public class BouncyMushroom : MonoBehaviour
{
    [Header("Bounce Settings")]
    [Tooltip("How much upward force to apply to the player.")]
    [SerializeField] private float bounceForce = 30f;

    [Header("Feedback")]
    [Tooltip("The Animator for the mushroom, to play a bounce animation.")]
    [SerializeField] private Animator animator;
    [Tooltip("The name of the animation trigger for the bounce (e.g., 'Bounce').")]
    [SerializeField] private string bounceTriggerName = "Bounce";

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the object that collided with us is the Player.
        if (other.gameObject.CompareTag("Player"))
        {
            // Check if the collision is coming from above (the player is landing on it).
            if (IsPlayerLandingOnTop(other))
            {
                // Find the player's Rigidbody.
                Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    // Apply the bounce force by directly setting the vertical velocity.
                    // This provides a consistent, powerful bounce.
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);

                    // Play a bounce animation, if one is set up.
                    if (animator != null && !string.IsNullOrEmpty(bounceTriggerName))
                    {
                        animator.SetTrigger(bounceTriggerName);
                    }
                }
            }
        }
    }

    private bool IsPlayerLandingOnTop(Collision2D collision)
    {
        // We check the "normal" of the collision contact points.
        // A normal is a vector that points straight out from the surface.
        // If the player is landing on top, the normal should be pointing mostly upwards.
        foreach (ContactPoint2D point in collision.contacts)
        {
            if (point.normal.y > 0.7f) // 0.7 is a good threshold, 1.0 would be a perfect flat top.
            {
                return true;
            }
        }
        return false;
    }
}