using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject Player;
    public GameObject Lute;
    public LuteItem LuteScript;
    public Sprite[] sprites;
    public SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && LuteScript.playerHasLute)
        {
            Destroy(Lute);
            LuteScript.playerHasLute = false;
            sr.sprite = sprites[1];
        }
    }
}
