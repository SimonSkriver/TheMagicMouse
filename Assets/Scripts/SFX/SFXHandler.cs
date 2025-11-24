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

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
    
