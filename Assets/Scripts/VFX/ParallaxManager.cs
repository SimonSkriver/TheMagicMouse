using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    [System.Serializable]
    public class BackgroundElement
    {
        public Transform transform;
        [Range(0,1)] public float scrollSpeed = 0.5f;
    }

    public BackgroundElement[] layers;
    private Vector3 lastCamPos;
    private Transform mainCam;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = Camera.main.transform;
        lastCamPos = mainCam.position;
    }

    void LateUpdate()
    {
        Vector3 camDelta = mainCam.position - lastCamPos;

        foreach (var layer in layers)
        {
            layer.transform.position += new Vector3(camDelta.x * layer.scrollSpeed, camDelta.y * layer.scrollSpeed, 0f);
        }
        lastCamPos = mainCam.position;
    }

    
}
