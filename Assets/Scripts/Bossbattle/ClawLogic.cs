using UnityEngine;

public class ClawLogic : MonoBehaviour
{
    private GameObject target;
    void Start()
    {
    target = GameObject.FindGameObjectWithTag("Player");
    Destroy(gameObject, 1f); 
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player Hit");
        }
    }
}
