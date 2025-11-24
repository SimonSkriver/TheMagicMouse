using UnityEngine;

public class LuteItem : MonoBehaviour
{
    public GameObject player;
    public bool playerHasLute;

    void Update()
    {
        if (playerHasLute)
        {
            transform.position = player.transform.position;
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
