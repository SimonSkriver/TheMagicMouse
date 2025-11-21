// LutePickupCutscene.cs
using UnityEngine;
using System.Collections;

public class LutePickupCutscene : MonoBehaviour
{
    [Header("References")]
    public PlayerController player;
    public BreakableBranch branchScript;
    public HUDController hudController;
    public SpriteRenderer luteSprite;

    [Header("Timing")]
    public float waitBeforeBreak = 2.0f;
    // We removed "fallDuration" because we now detect the landing dynamically!

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            // Check if we are in the right state (or if we are debugging)
            if (QuestManager.Instance.currentQuestState == QuestManager.QuestState.MetNPC)
            {
                StartCoroutine(PickupRoutine());
            }
            else
            {
                Debug.Log("Wrong Quest State for Lute Pickup: " + QuestManager.Instance.currentQuestState);
            }
        }
    }

    private IEnumerator PickupRoutine()
    {
        hasTriggered = true;

        // 1. LOCK PLAYER
        CinematicBarsManager.Instance.ShowBars();
        player.EnterCutscene();

        // 2. PICKUP VISUALS
        if (luteSprite != null) luteSprite.enabled = false;


        QuestManager.Instance.SetQuestState(QuestManager.QuestState.HasLute);

        // 3. WAIT FOR "UH OH" MOMENT
        yield return new WaitForSeconds(waitBeforeBreak);

        // 4. BREAK BRANCH
        if (branchScript != null)
        {
            branchScript.Break();
        }

        // 5. WAIT FOR FALL TO START
        // We wait briefly to ensure the branch rotates enough that the player 
        // isn't touching it anymore, effectively starting the "fall".
        yield return new WaitForSeconds(0.5f);

        // 6. WAIT FOR LANDING
        // This uses your existing PlayerController function to wait until IsGrounded is true again.
        yield return player.WaitForLandingRoutine();

        // Optional: Brief pause after landing for impact
        yield return new WaitForSeconds(0.5f);

        // 7. UNLOCK
        CinematicBarsManager.Instance.HideBars();
        player.ExitCutscene();
        if (hudController != null) hudController.ShowLuteIcon();

        QuestManager.Instance.SetQuestState(QuestManager.QuestState.BranchBroken); // Update state so next events know
        gameObject.SetActive(false);
    }
}