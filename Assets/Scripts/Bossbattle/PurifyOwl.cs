using UnityEngine;
using UnityEngine.SceneManagement;

public class PurifyOwl : MonoBehaviour
{
    private PlayerMana manaOnPlayer;
    private Collider2D smallCorruptionCollider;
    private Collider2D bigCorruptionCollider;
    

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
            FindAnyObjectByType<SceneLoader>().LoadNextScene();
            manaOnPlayer.currentMana = 0;
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