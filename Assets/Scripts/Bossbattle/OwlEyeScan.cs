using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OwlEyeScan : MonoBehaviour
{
    protected GameObject target;
    [Header("Detection Variables")]
    [SerializeField] protected float detectionAngle = 90f;
    [SerializeField] protected float lightSoftness = 2f;

    [Header("Attack Variables")]
    [SerializeField] protected GameObject clawPrefab;
    [SerializeField] protected float slamCooldownDuration = 2.5f;
    [SerializeField] protected float slamCooldownTimer;
    private bool onCooldown = true;


    private Light2D lightCone;

    void Awake()
    {
        //Match spotlight angle to detectionAngle
        lightCone = GetComponent<Light2D>();
        lightCone.pointLightOuterAngle = detectionAngle;
        lightCone.pointLightInnerAngle = detectionAngle - lightSoftness;
        //Set target to Player by using "Player" tag
        target = GameObject.FindGameObjectWithTag("Player");
    }
    void FixedUpdate()
    {
        if (onCooldown)
        {
        slamCooldownTimer += Time.deltaTime;
        if (slamCooldownTimer >= slamCooldownDuration)
            {
                onCooldown = false;
                slamCooldownTimer = 0f;
            }
        }
        DetectTarget();
    }

    void DetectTarget()
    {
        Vector2 eyePosition = transform.position;
        Vector2 targetPosition = target.transform.position;
        Vector2 visionConeVector = Vector2.down;

        //Angle check
        Vector2 targetVector = (targetPosition - eyePosition).normalized;
        float targetAngle = Vector2.Angle(visionConeVector, targetVector);
        if (targetAngle > detectionAngle * 0.5f) return;
        
        //Raycast. If line hits a collider, that also has the tag "Player" then trigger attack
        RaycastHit2D targetHit = Physics2D.Linecast(eyePosition, targetPosition, ~0); //~0 means it checks on all layers. We could add a layer mask if that makes more sense?
        if (targetHit.collider != null && targetHit.collider.CompareTag("Player"))
        {
            SlamAttack();
        }
    }

    void SlamAttack()
    {
        if (onCooldown) return;

        Debug.Log("SLAM!");
        Instantiate(clawPrefab, target.transform.position, Quaternion.identity);
        
        onCooldown = true;
        slamCooldownTimer = 0f;
    }
}