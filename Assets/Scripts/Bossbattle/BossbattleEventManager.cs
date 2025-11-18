using UnityEngine;

public class BossbattleEventManager : MonoBehaviour
{
    protected float eventTimer;
    [SerializeField] protected float eventDuration = 5f;
    private bool sceneRunning;
    private bool bossPresent;
    public float playerMana = 0f;
    [SerializeField] protected GameObject bossPrefab;
    [SerializeField] protected GameObject bossInstance;

    void Update()
    {
        if (sceneRunning)
        {
            eventTimer += Time.deltaTime;
        }

        if (eventTimer >= eventDuration && bossPresent)
        {
            BossExit();
            bossPresent = false;
        }

        if (eventTimer >= eventDuration && !bossPresent)
        {
            BossReturn();
            bossPresent = true;
        }

        if (playerMana >= 100f && !bossPresent && sceneRunning)
        {
            BossFleeing();
        }
    }


    public void SpawnBoss()
    {
        sceneRunning = true;
        bossInstance = Instantiate(bossPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        bossPresent = true;
        Debug.Log("BossSpawn");
    }

    void BossExit()
    {
        bossPresent = false;
        eventTimer = 0f;
        bossInstance.GetComponent<Animator>().SetBool("IsExiting?", true);
        bossInstance.GetComponent<Animator>().SetBool("IsScanning?", false);
        Debug.Log("BossExit");
    }

    void BossReturn()
    {
        bossPresent = true;
        eventTimer = 0f;
        bossInstance.GetComponent<Animator>().SetBool("IsReturning?", true);
        bossInstance.GetComponent<Animator>().SetBool("IsScanning?", true);
        bossInstance.GetComponent<Animator>().SetBool("IsExiting?", false);
        Debug.Log("BossReturn");
    }

    void BossFleeing()
    {
        bossPresent = false;
        sceneRunning = false;
        bossInstance.GetComponent<Animator>().SetBool("IsFleeing?", true);
        bossInstance.GetComponent<Animator>().SetBool("IsExiting?", false);
        bossInstance.GetComponent<Animator>().SetBool("IsScanning?", false);
        bossInstance.GetComponent<Animator>().SetBool("IsReturning?", false);
        
        Debug.Log("BossFleeing");        
    }
}
