// PlayerPurify.cs (Upgraded for Proximity Targeting)
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerPurify : MonoBehaviour
{
    [Header("Small Pulse")]
    [SerializeField] private GameObject smallPulsePrefab;
    [SerializeField] private Transform staffFirePoint;
    [SerializeField] private float smallPulseRadius = 5f;
    [SerializeField] private float smallPulseDuration = 0.5f;

    [Header("Big Pulse Settings")]
    [SerializeField] private float bigPulseRadius = 15f;
    [SerializeField] private float bigPulseDuration = 1.0f;

    // References to other player components.
    private PlayerMana playerMana;

    private bool holdPerformed = false;

    // --- NEW VARIABLE ---
    // This will store a reference to the major purifiable object we are currently in range of.
    private MajorPurifiable currentMajorTarget = null;

    private void Awake()
    {
        playerMana = GetComponent<PlayerMana>();
    }

    #region Trigger Detection for Major Targets
    // This runs when the player's collider enters a trigger zone.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the trigger we entered has the MajorPurifiable script.
        if (other.TryGetComponent<MajorPurifiable>(out MajorPurifiable target))
        {
            Debug.Log("Entered purification zone for: " + target.gameObject.name);
            currentMajorTarget = target;
            // You could add a UI prompt here like "[Hold E] to Purify"
        }
    }

    // This runs when the player's collider leaves a trigger zone.
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the trigger we are leaving is our current target.
        if (other.GetComponent<MajorPurifiable>() == currentMajorTarget)
        {
            Debug.Log("Exited purification zone.");
            currentMajorTarget = null;
            // Hide the UI prompt here.
        }
    }
    #endregion

    public void OnPurify(InputAction.CallbackContext context)
    {
        if (context.performed && context.interaction is HoldInteraction)
        {
            holdPerformed = true;
            AttemptBigPurify();
        }
        else if (context.canceled)
        {
            if (!holdPerformed)
            {
                FireSmallPulse();
            }
            holdPerformed = false;
        }
        else if (context.started)
        {
            holdPerformed = false;
        }
    }

    private void FireSmallPulse()
    {
        if (smallPulsePrefab == null || staffFirePoint == null) return;

        GameObject pulseGO = Instantiate(smallPulsePrefab, staffFirePoint.position, Quaternion.identity);
        pulseGO.transform.SetParent(staffFirePoint);

        if (pulseGO.TryGetComponent<ExpandingPulse>(out ExpandingPulse pulse))
        {
            pulse.Initialize(smallPulseRadius, smallPulseDuration);
        }
    }

    private void AttemptBigPurify()
    {
        // --- THE CRUCIAL FIX ---
        // First, check if we are even in range of a target.
        if (currentMajorTarget == null)
        {
            Debug.Log("No major purifiable object in range!");
            return;
        }

        if (!playerMana.IsManaFull)
        {
            Debug.Log("Not enough mana for the big purify!");
            return;
        }

        playerMana.UseAllMana();

        // Spawn the big visual pulse.
        if (smallPulsePrefab != null && staffFirePoint != null)
        {
            GameObject pulseGO = Instantiate(smallPulsePrefab, staffFirePoint.position, Quaternion.identity);
            pulseGO.transform.SetParent(staffFirePoint);
            if (pulseGO.TryGetComponent<ExpandingPulse>(out ExpandingPulse pulse))
            {
                pulse.Initialize(bigPulseRadius, bigPulseDuration);
            }
        }

        // Purify the specific target we are in range of.
        currentMajorTarget.Purify();
    }
}