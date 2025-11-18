using UnityEngine;

public class ShroomBounce : MonoBehaviour
{
    [SerializeField] float upwardsBounce;
    [SerializeField] float sidewaysBounce;
    public Rigidbody2D rb;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rb.linearVelocity = new Vector2(sidewaysBounce, upwardsBounce);
        }
    }
}
