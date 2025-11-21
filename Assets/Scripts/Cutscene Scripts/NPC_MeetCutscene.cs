// NPC_MeetCutscene.cs (Upgraded for Multiple Dialogue Lines)
using UnityEngine;
using System.Collections;

public class NPC_MeetCutscene : MonoBehaviour
{
    [Header("References")]
    public PlayerController player;
    public Transform holeEdgePosition;
    public TypewriterDialogue dialogueBubble;

    [Header("Dialogue Sequence")]
    [Tooltip("The lines of dialogue the NPC will say in order.")]
    [TextArea(2, 5)] // Makes the text box bigger in the Inspector
    public string[] dialogueLines;
    [Tooltip("How long to wait after each line of dialogue before showing the next.")]
    public float delayBetweenLines = 1f;

    public void StartCutscene()
    {
        StartCoroutine(CutsceneRoutine());
    }

    private IEnumerator CutsceneRoutine()
    {
        // 1. BARS IN & FREEZE PLAYER
        CinematicBarsManager.Instance.ShowBars();
        player.EnterCutscene();

        // 2. DIALOGUE SEQUENCE
        if (dialogueBubble != null && dialogueLines.Length > 0)
        {
            foreach (string line in dialogueLines)
            {
                dialogueBubble.ShowDialogue(line);
                yield return new WaitUntil(() => !dialogueBubble.IsTyping);
                yield return new WaitForSeconds(delayBetweenLines);
            }
            dialogueBubble.gameObject.SetActive(false);
        }

        // 3. MOVE TO LEDGE
        yield return player.MoveToPositionRoutine(holeEdgePosition.position);

        // 4. PAUSE & JUMP
        yield return new WaitForSeconds(0.2f);
        player.PerformCutsceneJump();

        // 5. WAIT FOR LANDING
        // The director now waits for the player to report that they have landed.
        yield return player.WaitForLandingRoutine();

        // --- THE DEFINITIVE FIX ---
        // 6. END THE CUTSCENE
        // These two commands now fire at the exact same time, right after landing.
        CinematicBarsManager.Instance.HideBars();
        player.UnlockDoubleJump();

        player.ExitCutscene();
    }
}