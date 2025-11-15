// PurifiableObject.cs (Upgraded with UnityEvent)
using UnityEngine;
using UnityEngine.Events; // Add this line!

public class PurifiableObject : MonoBehaviour
{
    public GameObject corruptedVisuals;
    public GameObject purifiedVisuals;
    public bool disableColliderOnPurify = true;

    public UnityEvent OnPurify; // --- THE NEW, IMPORTANT PART ---

    private bool isPurified = false;

    private void Start()
    {
        corruptedVisuals.SetActive(true);
        if (purifiedVisuals != null) purifiedVisuals.SetActive(false);
    }

    public void Purify()
    {
        if (isPurified) return;
        isPurified = true;

        corruptedVisuals.SetActive(false);
        if (purifiedVisuals != null) purifiedVisuals.SetActive(true);

        if (disableColliderOnPurify)
        {
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
        }

        // Fire the event!
        OnPurify.Invoke();
    }
}