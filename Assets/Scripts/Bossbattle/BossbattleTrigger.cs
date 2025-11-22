using UnityEngine;

public class BossbattleTrigger : MonoBehaviour
{
    [SerializeField] protected BossbattleEventManager eventManager;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            eventManager.SpawnBoss();
            Destroy(gameObject);
        }
    }
}
