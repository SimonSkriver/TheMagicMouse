using System.Collections;
using UnityEngine;

public class BreakableBranch : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("How fast the branch falls.")]
    [SerializeField] private float fallSpeed = 2.0f;
    [Tooltip("The target rotation angle (e.g., -80 for straight down).")]
    [SerializeField] private float targetRotation = -80f;

    private bool isBreaking = false;

    public void Break()
    {
        if (!isBreaking)
        {
            isBreaking = true;
            StartCoroutine(BreakRoutine());
        }
    }

    private IEnumerator BreakRoutine()
    {
        Quaternion startRot = transform.rotation;
        // Calculate the target rotation relative to world space or local space 
        // (Assuming local is best for a child object)
        Quaternion endRot = Quaternion.Euler(0, 0, targetRotation);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * fallSpeed;
            transform.localRotation = Quaternion.Lerp(startRot, endRot, t);
            yield return null;
        }
    }
}