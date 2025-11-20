using UnityEngine;

public class BossbattleEventManager : MonoBehaviour
{
    protected float eventTimer;
    [SerializeField] protected float scanningDuration = 10f;
    [SerializeField] protected float collectionDuration = 5f;
    
    private bool sceneRunning;
    private bool bossPresent;
    public float playerCurrentMana = 0f;
    public float playerFullMana = 100f;
    
    [SerializeField] protected GameObject bossPrefab;
    [SerializeField] protected GameObject bossInstance;

    void Update()
    {
        //Event timer counting up
        if (sceneRunning)
        {
            eventTimer += Time.deltaTime;
        }
        //Scanning stage ends, when timer reaches set duration
        if (eventTimer >= scanningDuration && bossPresent)
        {
            BossExit();
        }
        //Collection stage ends, when timer reaches set duration
        if (eventTimer >= collectionDuration && !bossPresent)
        {
            BossReturn();
        }
        //Boss fleeing after 100 mana
        if (playerCurrentMana >= playerFullMana && !bossPresent && sceneRunning)
        {
            BossFleeing();
        }
    }


    public void SpawnBoss()
    {
        sceneRunning = true;
        bossInstance = Instantiate(bossPrefab, new Vector3(0f, 25f, 0f), Quaternion.identity);
        bossPresent = true;
        Debug.Log("BossSpawn");
    }

    void BossExit()
    {
        bossPresent = false;
        eventTimer = 0f;
        bossInstance.GetComponent<Animator>().SetTrigger("Exit");
        bossInstance.GetComponent<Animator>().SetBool("IsScanning?", false);
        Debug.Log("BossExit");
    }

    void BossReturn()
    {
        bossPresent = true;
        eventTimer = 0f;
        bossInstance.GetComponent<Animator>().SetTrigger("Return");
        bossInstance.GetComponent<Animator>().SetBool("IsScanning?", true);
        
        Debug.Log("BossReturn");
    }

    void BossFleeing()
    {
        bossPresent = false;
        sceneRunning = false;
        bossInstance.GetComponent<Animator>().SetTrigger("Flee");
        bossInstance.GetComponent<Animator>().SetBool("IsScanning?", false);
        Destroy(bossInstance, 2f);
        Debug.Log("BossFleeing");
    }
}
