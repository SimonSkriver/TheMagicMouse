// PurifiableObject.cs (Upgraded with Reset Functionality)
using UnityEngine;
using UnityEngine.Events;

public class PurifiableObject : MonoBehaviour
{
    public GameObject corruptedVisuals;
    public UnityEvent OnPurify;

    // We remove the other variables as they are not needed for this setup.

    private bool canBePurified = true;

    // This is the public method our pulse will call.
    public void Purify()
    {
        if (!canBePurified) return;

        canBePurified = false; // Prevent spamming while the event is firing.

        if (corruptedVisuals != null)
        {
            corruptedVisuals.SetActive(false);
        }

        OnPurify.Invoke();
    }

    // --- THE NEW METHOD ---
    // A public method that other scripts (like our vine) can call to reset this object.
    public void ResetPurifiable()
    {
        Debug.Log(gameObject.name + " has been reset and can be purified again.");
        canBePurified = true;
        if (corruptedVisuals != null)
        {
            corruptedVisuals.SetActive(true);
        }
    }
}