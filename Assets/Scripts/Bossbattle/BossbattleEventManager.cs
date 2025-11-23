using UnityEngine;

public class BossbattleEventManager : MonoBehaviour
{
    protected float eventTimer;
    [SerializeField] protected float scanningDuration = 10f;
    [SerializeField] protected float collectionDuration = 5f;
    
    private bool sceneRunning;
    private bool bossPresent;
    
    [SerializeField] protected GameObject bossPrefab;
    protected GameObject bossInstance;
    private PlayerMana playerMana;
    protected ManaScatter manaScatter;


    void Awake()
    {
        manaScatter = GetComponent<ManaScatter>();
        playerMana = FindAnyObjectByType<PlayerMana>();
    }
    
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
        if (playerMana.IsManaFull && !bossPresent && sceneRunning)
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
        manaScatter.ScatterMana();
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
        Destroy(bossInstance, 10f);
        Debug.Log("BossFleeing");
    }
}
