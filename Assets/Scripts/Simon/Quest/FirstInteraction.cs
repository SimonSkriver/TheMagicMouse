using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class FirstInteraction : MonoBehaviour
{
    [Header ("Dialogue stuff")]
    public TextMeshProUGUI textDisplay;
    public GameObject dialogueBox;
    public float typeSpeed = 0.2f;
    public string[] sentances;
    public GameObject continueButton;
    public Canvas canvas;
    private int index;
    public bool hasMet = false;

    public InputActionAsset playerControls;
    public Animator anim;
    private bool isTalking;

    void Awake()
    {
    }

    void Update()
    {
        if (textDisplay.text == sentances[index])
        {
            continueButton.SetActive(true);
        }
        HandlePlayerControls();
    }

    IEnumerator Type()
    {
        foreach (char letter in sentances[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    public void NextSentance()
    {
        continueButton.SetActive(false);
        if (index < sentances.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());    
        }
        else // When the dialogue is over
        {
            textDisplay.text = "";
            anim.SetTrigger("BoxDisappear");
            continueButton.SetActive(false);
            isTalking = false;
        }
    }
    

    void OnTriggerEnter2D(Collider2D other)
    { 
        if (other.CompareTag("Player") && !hasMet)
        {   
            isTalking = true;
            anim.SetTrigger("BoxAppear");
            StartCoroutine(Type());
            hasMet = true;
        }
    }

    void HandlePlayerControls()
    {
        if (isTalking)
        {
            playerControls.Disable();
        }
        else
        {
            playerControls.Enable(); 
        }
    }
}
