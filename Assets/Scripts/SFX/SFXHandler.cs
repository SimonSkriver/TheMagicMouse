using UnityEngine;

public class SFXHandler : MonoBehaviour
{
    [System.Serializable]
    public class SoundEffect
    {
        public AudioClip audioClip;
        public string animatorEventTag;
    }

    public SoundEffect[] soundEffects;
    private AudioSource audioSource;
    public AudioClip EasterEgg1;
    public AudioClip EasterEgg2;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    
    void OnTriggerEnter2D(Collider2D other) //cheeky lil easter egg
    {
        if (other.CompareTag("EasterEgg"))
        {
            audioSource.PlayOneShot(EasterEgg1);
            audioSource.pitch = 1.2f;
            audioSource.PlayOneShot(EasterEgg2);
        }
    }
    

    public void PlaySoundByTag(string tag)
    {
        foreach (var sfx in soundEffects)
        {
            if (sfx.animatorEventTag == tag && sfx.audioClip !=null)
            {
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.volume = Random.Range(0.8f, 1.2f);
                audioSource.PlayOneShot(sfx.audioClip);
            }
        }
    }
}
    
