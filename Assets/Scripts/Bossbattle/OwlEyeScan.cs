using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OwlEyeScan : MonoBehaviour
{
    [SerializeField] protected GameObject target;
    [SerializeField] protected float detectionAngle = 90f;
    [SerializeField] protected float lightSoftness = 2f;
    private Light2D lightCone;

    void Awake()
    {
        //Match spotlight angle to visionAngle
        lightCone = GetComponent<Light2D>();
        lightCone.pointLightOuterAngle = detectionAngle;
        lightCone.pointLightInnerAngle = detectionAngle - lightSoftness;
    }
    void Update()
    {
        DetectTarget();
    }

    void DetectTarget()
    {
        Vector2 eyePosition = transform.position;
        Vector2 targetPosition = target.transform.position;
        Vector2 forwardVector = transform.up;

        //Angle check
        Vector2 targetVector = (targetPosition - eyePosition).normalized;
        float targetAngle = Vector2.Angle(forwardVector, targetVector);
        if (targetAngle > detectionAngle * 0.5f) return;
        
        //Raycast. If line hits a collider, that also has the tag Player then trigger attack
        RaycastHit2D targetHit = Physics2D.Linecast(eyePosition, targetPosition, ~0); //~0 means it checks on all layers. We could add a layer mask if that makes more sense?
        if (targetHit.collider != null && targetHit.collider.CompareTag("Player"))
        {
            SlamAttack();
        }
    }

    void SlamAttack()
    {
        Debug.Log("I SEE YOU");
    }
}