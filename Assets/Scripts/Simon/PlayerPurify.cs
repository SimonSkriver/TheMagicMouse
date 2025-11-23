using Mono.Cecil.Cil;
using UnityEngine;

public class PlayerPurify : MonoBehaviour
{
    private PlayerMana manaOnPlayer;
    private bool isNearSmallCorruption;
    private bool isNearBigCorruption;
    public Transform corruptionCheck;
    public LayerMask smallCorruption;
    public LayerMask bigCorruption;
    public float corruptionCheckRadius;
    public GameObject smallCorruptionObject;
    public GameObject bigCorruptionObject;


    void Awake()
    {
        manaOnPlayer = FindAnyObjectByType<PlayerMana>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        checkCorruption();
    }

    void checkCorruption()
    {
        isNearSmallCorruption = Physics2D.OverlapCircle(corruptionCheck.position, corruptionCheckRadius, smallCorruption);
        isNearBigCorruption = Physics2D.OverlapCircle(corruptionCheck.position, corruptionCheckRadius, bigCorruption);
    }

    void OnSmallPurify()
    {
        Debug.Log("Small purify called");
        if (isNearSmallCorruption)
        {
            Destroy(smallCorruptionObject);
        }
    }

    void OnBigPurify()
    {
        if (manaOnPlayer.IsManaFull)
        {
            Destroy(bigCorruptionObject);
        }
        else
        {
            Debug.Log("You don't have enough mana!");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(corruptionCheck.position, corruptionCheckRadius);
    }
}