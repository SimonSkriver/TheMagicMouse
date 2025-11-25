using JetBrains.Annotations;
using UnityEngine;

public class ManaScatter : MonoBehaviour
{
    [SerializeField] protected GameObject manaPrefab;
    [SerializeField] protected int manaCount = 10;
    [SerializeField] protected Vector2 areaSize = new Vector2(250f, 150f);
    [SerializeField] protected AnimationCurve verticalDensity;
    [SerializeField] protected float manaDespawnTime = 5;
    [SerializeField] protected AudioClip manaSpawnSound;
    [SerializeField] protected AudioSource audioSource;
    
    void Start()
    {
        
    }
    
    public void ScatterMana()
    {
        int manaSpawned = 0;
        audioSource.PlayOneShot(manaSpawnSound);

        while (manaSpawned < manaCount)
        {
            float yNorm = Random.value;
            if (verticalDensity != null && verticalDensity.Evaluate(yNorm) < Random.value)
                continue;

            Vector2 spawnPos = new Vector2(Random.Range(-areaSize.x / 2f, areaSize.x / 2f), Mathf.Lerp(-areaSize.y / 2f, areaSize.y / 2f, yNorm));

            GameObject manaInstance = Instantiate(manaPrefab, spawnPos, Quaternion.identity);
            Destroy(manaInstance, manaDespawnTime);

            manaSpawned ++;
        }
    }
}
