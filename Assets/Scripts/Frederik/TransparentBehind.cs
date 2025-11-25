using UnityEngine;

public class TransparentBehind : MonoBehaviour
{
    [Header("Transparency Settings")]
    [Tooltip("Hvor gennemsigtig skal objektet blive? (0 = usynlig, 1 = fuld synlighed)")]
    [SerializeField] private float transparencyAlpha = 0.5f;
    
    [Tooltip("Hvor hurtigt skal den fade ud og ind?")]
    [SerializeField] private float fadeSpeed = 5f;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private float targetAlpha = 0f;

    private void Start()
    {
        // Hvis vi ikke har sat spriteRenderer i inspectoren, så prøv at finde den på samme objekt
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        // Hvis vi har en sprite renderer, så opdater dens farve
        if (spriteRenderer != null)
        {
            Color currentColor = spriteRenderer.color;
            // Brug Mathf.MoveTowards eller Lerp for en glidende overgang
            float newAlpha = Mathf.MoveTowards(currentColor.a, targetAlpha, fadeSpeed * Time.deltaTime);
            
            spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Tjek om det er spilleren der går ind i træet
        if (other.CompareTag("Player"))
        {
            targetAlpha = transparencyAlpha;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Når spilleren går ud igen, sæt alpha tilbage til 1 (fuld synlighed)
        if (other.CompareTag("Player"))
        {
            targetAlpha = 0f;
        }
    }
}
