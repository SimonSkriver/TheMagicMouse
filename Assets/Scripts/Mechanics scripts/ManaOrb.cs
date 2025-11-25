// ManaOrb.cs
using UnityEngine;

public class ManaOrb : MonoBehaviour
{
    [Tooltip("How much mana this orb provides.")]
    [SerializeField] private float manaAmount = 25f;
    [SerializeField] private AudioClip pickUpSfx;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is the Player.
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(pickUpSfx, transform.position);
            // Try to get the PlayerMana component from the object we hit.
            if (other.TryGetComponent<PlayerMana>(out PlayerMana playerMana))
            {
                // Call the AddMana method on the player's script.
                playerMana.AddMana(manaAmount);

                // "Collect" the orb by disabling its components and destroying it.
                // This is a safe way to prevent collecting the same orb multiple times.
                GetComponent<Collider2D>().enabled = false;
                //GetComponent<SpriteRenderer>().enabled = false;
                Destroy(gameObject, 0.2f);
            }
        }
    }
}