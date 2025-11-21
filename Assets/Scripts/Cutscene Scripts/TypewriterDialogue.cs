// TypewriterDialogue.cs
using System.Collections;
using UnityEngine;
using TMPro; // Add this for TextMeshPro

public class TypewriterDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.05f;

    private Coroutine typingCoroutine;
    public bool IsTyping { get; private set; } = false;


    public void ShowDialogue(string text)
    {
        gameObject.SetActive(true);
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypewriterRoutine(text));
    }

    private IEnumerator TypewriterRoutine(string text)
    {
        // --- ADD THIS ---
        IsTyping = true; // Set the flag when we start typing.

        dialogueText.text = "";
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // --- ADD THIS ---
        IsTyping = false; // Clear the flag when we're done.
    }
}