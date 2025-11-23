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
        if (isNearSmallCorruption)
        {
            Destroy(smallCorruptionObject);
        }
    }

    void OnBigPurify()
    {
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(corruptionCheck.position, corruptionCheckRadius);
    }
}