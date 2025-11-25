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

    public InputActionAsset actionAsset;
    private PlayerController pc;
    public Rigidbody2D playerRB;
    public Animator anim;
    private bool isTalking;

    void Awake()
    {
        pc = FindAnyObjectByType<PlayerController>();
    }

    void Update()
    {
        if (textDisplay.text == sentances[index])
        {
            continueButton.SetActive(true);
        }
        HandlePlayerControls();
        HandleDJ();
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
            //dialogueBox.SetActive(false);
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
            //dialogueBox.SetActive(true);
            anim.SetTrigger("BoxAppear");
            StartCoroutine(Type());
            playerRB.linearVelocity = new Vector2(0, 0);
            hasMet = true;
        }
    }

    void HandleDJ()
    {
        if (hasMet)
        {
            pc.maxJumps = 2; 
        }
    }

    void HandlePlayerControls()
    {
        if (isTalking)
        {
            actionAsset.Disable();
        }
        else
        {
            actionAsset.Enable(); 
        }
    }
}
