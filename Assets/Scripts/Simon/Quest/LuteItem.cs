using UnityEngine;

public class LuteItem : MonoBehaviour
{
    public GameObject Player;
    public bool playerHasLute;

    void Update()
    {
        if (playerHasLute)
        {
            transform.position = Player.transform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHasLute = true;
        }
    }
}
