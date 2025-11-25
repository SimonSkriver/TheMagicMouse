using UnityEngine;

public class PlayerPurify : MonoBehaviour
{
    private PlayerMana manaOnPlayer;
    private Collider2D smallCorruptionCollider;
    private Collider2D bigCorruptionCollider;
    [SerializeField] private Animator anim; //Tif�ljje animator til at spille animationer
    [SerializeField] private string bigPurifyParam = "BigPurify"; //Navn p� parameter i animator til big purify


    [Header ("Corruption check settings")]
    public Transform corruptionCheck;
    public LayerMask smallCorruption;
    public LayerMask bigCorruption;
    public float corruptionCheckRadius;


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
        smallCorruptionCollider = Physics2D.OverlapCircle(corruptionCheck.position, corruptionCheckRadius, smallCorruption);
        bigCorruptionCollider = Physics2D.OverlapCircle(corruptionCheck.position, corruptionCheckRadius, bigCorruption);
    }

    void OnSmallPurify()
    {
        Debug.Log("Small purify called");
        if (smallCorruptionCollider != null)
        {
            Destroy(smallCorruptionCollider.gameObject);
        }
    }

    void OnBigPurify()
    {
        if (manaOnPlayer.IsManaFull && bigCorruptionCollider != null)
        {
            Destroy(bigCorruptionCollider.gameObject);
            manaOnPlayer.UseAllMana(); 
            anim.SetTrigger(bigPurifyParam);
        }
        else
        {
            Debug.Log("You don't have enough");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(corruptionCheck.position, corruptionCheckRadius);
    }
}