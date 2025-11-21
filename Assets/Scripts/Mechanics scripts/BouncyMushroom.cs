using UnityEngine;

public class ShroomBounce : MonoBehaviour
{
    [Tooltip("The total strength of the bounce.")]
    [SerializeField] private float bounceForce = 20f;

    [Tooltip("How long (in seconds) the player loses control after bouncing.")]
    [SerializeField] private float stunDuration = 0.2f; // 0.2s is usually a good feel

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the CONTROLLER, not the Rigidbody
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                // Calculate direction based on rotation (Green Arrow)
                Vector2 direction = transform.up;

                // Tell the player controller to launch and lock input
                player.Launch(direction * bounceForce, stunDuration);
            }
        }
    }
}