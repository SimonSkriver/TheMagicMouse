// BridgeController.cs (Final Robust Version)
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BridgeController : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float growDuration = 0.5f;
    [SerializeField] private float holdDuration = 3f;
    [SerializeField] private float retractDuration = 1f;

    [Header("Events")]
    public UnityEvent OnAnimationComplete;

    private Vector3 finalScale;
    private Vector3 startScale = new Vector3(0.1f, 1, 1);
    private bool hasBeenTriggered = false; // Prevents OnEnable from firing on game start

    private void Awake()
    {
        finalScale = transform.localScale;
        transform.localScale = startScale;
        gameObject.SetActive(false); // Start disabled.
    }

    // OnEnable is called by Unity automatically when SetActive(true) is called.
    private void OnEnable()
    {
        // We only want to run the animation if it's been triggered by the player,
        // not if it happens to be active when the game first starts.
        if (hasBeenTriggered)
        {
            StartCoroutine(AnimateBridgeRoutine());
        }
    }

    // The PurifiableObject will now call this simple method.
    public void TriggerAnimation()
    {
        // This method's only job is to enable the object and set the flag.
        hasBeenTriggered = true;
        gameObject.SetActive(true);
    }

    private IEnumerator AnimateBridgeRoutine()
    {
        // Reset the flag so OnEnable doesn't fire again if the object is disabled/re-enabled.
        hasBeenTriggered = false;

        yield return AnimateScale(startScale, finalScale, growDuration);
        yield return new WaitForSeconds(holdDuration);
        yield return AnimateScale(finalScale, startScale, retractDuration);

        OnAnimationComplete.Invoke();
        gameObject.SetActive(false); // Disable self when done.
    }

    private IEnumerator AnimateScale(Vector3 from, Vector3 to, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(from, to, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = to;
    }
}