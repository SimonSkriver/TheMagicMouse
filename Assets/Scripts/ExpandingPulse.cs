// ExpandingPulse.cs (Upgraded and Configurable)
using System.Collections;
using UnityEngine;

public class ExpandingPulse : MonoBehaviour
{
    // These are no longer configured in the Inspector, but set by the script that creates the pulse.
    private float maxRadius;
    private float expandDuration;

    [Header("References")]
    [Tooltip("The visual part of the pulse that will be scaled up.")]
    public Transform pulseVisuals;

    private CircleCollider2D circleCollider;

    // This is the new method that allows us to configure the pulse on the fly.
    public void Initialize(float radius, float duration)
    {
        this.maxRadius = radius;
        this.expandDuration = duration;
    }

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        StartCoroutine(ExpandRoutine());
    }

    private IEnumerator ExpandRoutine()
    {
        float elapsedTime = 0f;
        float initialRadius = circleCollider.radius;

        while (elapsedTime < expandDuration)
        {
            float currentRadius = Mathf.Lerp(initialRadius, maxRadius, elapsedTime / expandDuration);
            circleCollider.radius = currentRadius;

            if (pulseVisuals != null)
            {
                pulseVisuals.localScale = Vector3.one * currentRadius * 2;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PurifiableObject>(out PurifiableObject purifiable))
        {
            purifiable.Purify();
        }
    }
}