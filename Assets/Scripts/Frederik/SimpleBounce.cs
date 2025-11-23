using System.Collections;
using UnityEngine;

public class SimpleBounce : MonoBehaviour
{
    [SerializeField] private float bounceForce = 15f;
    [SerializeField] private float stunTime = 0.5f;
    [SerializeField] private bool isPurified = false;
    [SerializeField] private bool startPurified = false;

    // Til senere brug hvis vi har en animationer
    [SerializeField] private Animator animator;
    [SerializeField] private string purifyAnimName = "Purify";
    [SerializeField] private string bounceAnimName = "Bounce";
    [SerializeField] private SpriteRenderer spriteRenderer; // Til eventuelt skifte spritet i en animation

    private void Start()
    {
        if (startPurified)
        {
            Purify();
        }
    }

    // Kald denne metode fra purify scriptet
    public void Purify()
    {
        isPurified = true;
        // Hvis vi har en animator, spil purify animationen
        if (animator != null)
        {
            animator.Play(purifyAnimName);
        }
        Debug.Log("Mushroom Purified!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Check om vi er purified. Hvis ikke, gør ingenting.
        if (!isPurified) return;
        // 2. Check for Player tag
        if (other.CompareTag("Player"))
        {
            // 3. Hent PlayerController scriptet så vi kan slukke det
            MonoBehaviour PlayerController = other.GetComponent<PlayerController>();
            // 4. Hent Rigidbody for at tilføje Force
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (PlayerController != null && rb != null)
            {
                // Start hoppe sekvensen
                StartCoroutine(PerformBounce(rb, PlayerController));
            }
        }
    }

    // Coroutines gør så vi kan pause koden (vente i sekunder)
    private IEnumerator PerformBounce(Rigidbody2D rb, MonoBehaviour PlayerController)
    {
        // Deaktiver spillerens script så det ikke overskriver vores Force (fordi lige nu overskriver PlayerControlleren vores Force hvis vi ikke holder en movement input nede)
        PlayerController.enabled = false;
        // Nulstil nuværende hastighed så hoppet er ensartet
        rb.linearVelocity = Vector2.zero;
        // Tilføj Force opad (transform.up tillader os at rotere svampen)
        rb.AddForce(transform.up * bounceForce, ForceMode2D.Impulse);
        // Afspil hoppe-animation hvis vi har en
        if (animator != null)
        {   
            animator.Play(bounceAnimName);
        }
        // Vent i stun-tiden
        yield return new WaitForSeconds(stunTime);
        // Genaktiver spillerens script så de kan bevæge sig igen
        PlayerController.enabled = true;
    }
}
