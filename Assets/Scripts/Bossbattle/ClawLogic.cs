using UnityEngine;

public class ClawLogic : MonoBehaviour
{
    private GameObject target;
    private PlayerMana playerMana;

    void Awake()
    {
        playerMana = FindAnyObjectByType<PlayerMana>();
    }
    void Start()
    {
    target = GameObject.FindGameObjectWithTag("Player");
    Destroy(gameObject, 1f); 
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerMana.currentMana > 0)
        {
            playerMana.currentMana = playerMana.currentMana - 25;
            Debug.Log("Player hit! Mana is: " + playerMana.currentMana);
        }
    }
}
