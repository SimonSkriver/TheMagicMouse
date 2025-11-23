using UnityEngine;
using TMPro;
using System.Collections;

public class NPC : MonoBehaviour
{
    [Header ("Dialogue stuff")]
    public TextMeshProUGUI textDisplay;
    public float typeSpeed = 0.2f;
    public string[] sentances;
    public GameObject continueButton;
    private int index;

    [Header ("Other necesseties")]
    //public GameObject Player;
    public GameObject lute;
    public LuteItem luteScript;
    public Sprite[] sprites;
    public SpriteRenderer sr;

    void Start()
    {
        StartCoroutine(Type());
    }

    void Update()
    {
        if (textDisplay.text == sentances[index])
        {
            continueButton.SetActive(true);
        }
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
            continueButton.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    { 
        if (other.CompareTag("Player") && luteScript.playerHasLute)
        {
            Destroy(lute);
            luteScript.playerHasLute = false;
            sr.sprite = sprites[1];
        }
    }
}
