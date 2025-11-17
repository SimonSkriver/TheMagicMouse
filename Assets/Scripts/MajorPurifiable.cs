using UnityEngine;

public class MajorPurifiable : MonoBehaviour
{
    public void Purify()
    {
        Debug.Log("Major Corruption has been cleared!");
        // Add a cool particle effect or animation here.
        Destroy(gameObject);
    }
}