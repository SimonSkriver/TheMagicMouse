using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class SecondInteraction : MonoBehaviour
{ 
    [Header ("Dialogue stuff")]
    public TextMeshProUGUI textDisplay;
    public GameObject dialogueBox;
    public float typeSpeed = 0.2f;
    public string[] sentances;
    public GameObject continueButton;
    public Canvas canvas;
    private int index;

    [Header ("Lute necessities")]
    public GameObject lute;
    public LuteItem luteScript;
    public Sprite[] sprites;
    public SpriteRenderer sr;

    public Animator anim;
    public InputActionAsset inputAsset;
    private PlayerMana manaOnPlayer;
    private PlayerController pc;
    private bool isTalking;

    void Awake()
    {
        pc = FindAnyObjectByType<PlayerController>();
        manaOnPlayer = FindAnyObjectByType<PlayerMana>();
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
            manaOnPlayer.currentMana = 100f;
            isTalking = false;
            anim.SetTrigger("BoxDisappear");
            continueButton.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && luteScript.playerHasLute)
        {
            //dialogueBox.SetActive(true);
            anim.SetTrigger("BoxAppear");
            StartCoroutine(Type());
            Destroy(lute);
            isTalking = true;
            sr.sprite = sprites[1];
            luteScript.playerHasLute = false;
        }
    }

    void HandlePlayerControls()
    {
        if (isTalking)
        {
            inputAsset.Disable();
        }
        else
        {
            inputAsset.Enable();
        } 
    }
}
