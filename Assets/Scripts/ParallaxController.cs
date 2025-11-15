// ParallaxController.cs (Final Version with Negative Factor Support)
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    private Transform cameraTransform;

    [Header("Parallax Factors")]
    [Tooltip("How much the layer moves horizontally. Negative values move in the opposite direction.")]
    [Range(-2f, 2f)] // --- CHANGE: Widened the range to allow negative values ---
    public float parallaxFactorX;

    [Tooltip("How much the layer moves vertically. Set to 0 to lock the Y-axis.")]
    [Range(-2f, 2f)] // --- CHANGE: Widened the range to allow negative values ---
    public float parallaxFactorY;

    private Vector3 lastCameraPosition;
    private float spriteWidth;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteWidth = spriteRenderer.bounds.size.x;
        }
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        transform.position += new Vector3(deltaMovement.x * parallaxFactorX, deltaMovement.y * parallaxFactorY, 0);

        lastCameraPosition = cameraTransform.position;

        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= spriteWidth)
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % spriteWidth;
            transform.position = new Vector3(cameraTransform.position.x - offsetPositionX, transform.position.y);
        }
    }
}