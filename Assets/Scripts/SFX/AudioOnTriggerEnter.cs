using UnityEngine;

public class AudioOnTriggerEnter : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sfx;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(sfx);
        }
    }
}
