// PlayerPurify.cs (Update-Loop Version)
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerPurify : MonoBehaviour
{
    [Header("Pulse Prefab")]
    [SerializeField] private GameObject pulsePrefab;
    [SerializeField] private Transform staffFirePoint;

    [Header("Small Pulse Settings")]
    [SerializeField] private float smallPulseRadius = 5f;
    [SerializeField] private float smallPulseDuration = 0.25f;
    [SerializeField] private float smallPulseCooldown = 0.5f; // Lowered slightly for better feel

    [Header("Big Pulse Settings")]
    [SerializeField] private float bigPulseRadius = 15f;
    [SerializeField] private float bigPulseDuration = 1f;
    [SerializeField] private float holdTimeThreshold = 0.4f; // How long to hold for Big Pulse

    // --- State Variables ---
    private PlayerMana playerMana;
    private PlayerController playerController;
    private MajorPurifiable currentMajorTarget = null;

    private bool canFireSmallPulse = true;
    private bool isHoldingButton = false;
    private bool bigPulseHasFired = false;
    private float holdTimer = 0f;

    private void Awake()
    {
        playerMana = GetComponent<PlayerMana>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // If the button is being held...
        if (isHoldingButton)
        {
            // ...and we haven't already fired the big pulse...
            if (!bigPulseHasFired)
            {
                holdTimer += Time.deltaTime;

                // ...and the timer exceeds the threshold...
                if (holdTimer >= holdTimeThreshold)
                {
                    // FIRE THE BIG PULSE!
                    AttemptBigPurify();
                    bigPulseHasFired = true; // Lock it so we don't fire again or fire small pulse on release.
                }
            }
        }
    }

    public void OnPurify(InputValue value)
    {
        // Cutscene Gate
        if (playerController != null && playerController.isInCutscene)
        {
            isHoldingButton = false;
            return;
        }

        if (value.isPressed)
        {
            // --- BUTTON DOWN ---
            isHoldingButton = true;
            bigPulseHasFired = false;
            holdTimer = 0f; // Reset timer
        }
        else
        {
            // --- BUTTON UP ---
            isHoldingButton = false;

            // If we released the button and the Big Pulse NEVER fired...
            if (!bigPulseHasFired)
            {
                // ...then it was a short tap. Fire Small Pulse.
                FireSmallPulse();
            }

            // Reset everything for safety
            holdTimer = 0f;
            bigPulseHasFired = false;
        }
    }

    private void FireSmallPulse()
    {
        if (!canFireSmallPulse || pulsePrefab == null || staffFirePoint == null) return;

        canFireSmallPulse = false;

        SpawnPulse(smallPulseRadius, smallPulseDuration);

        StartCoroutine(SmallPulseCooldownRoutine());
    }

    private IEnumerator SmallPulseCooldownRoutine()
    {
        yield return new WaitForSeconds(smallPulseCooldown);
        canFireSmallPulse = true;
    }

    private void AttemptBigPurify()
    {
        // Logic: We attempted it. Whether it succeeds (mana/target) or fails,
        // we count it as "Fired" so the small pulse doesn't trigger on release.

        if (currentMajorTarget == null)
        {
            Debug.Log("Big Pulse: No Target.");
            return;
        }

        if (!playerMana.IsManaFull)
        {
            Debug.Log("Big Pulse: Not Enough Mana.");
            return;
        }

        // Success logic
        playerMana.UseAllMana();
        currentMajorTarget.Purify();
        SpawnPulse(bigPulseRadius, bigPulseDuration);
    }

    private void SpawnPulse(float radius, float duration)
    {
        GameObject pulseGO = Instantiate(pulsePrefab, staffFirePoint.position, Quaternion.identity);
        // IMPORTANT: Parent it to the staff so it flips/moves with the player
        pulseGO.transform.SetParent(staffFirePoint);

        if (pulseGO.TryGetComponent<ExpandingPulse>(out ExpandingPulse pulse))
        {
            pulse.Initialize(radius, duration);
        }
    }

    #region Trigger Detection
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<MajorPurifiable>(out var target))
        {
            currentMajorTarget = target;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<MajorPurifiable>() == currentMajorTarget)
        {
            currentMajorTarget = null;
        }
    }
    #endregion
}