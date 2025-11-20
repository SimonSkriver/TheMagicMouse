using UnityEngine;

public class ManaScatter : MonoBehaviour
{
    [SerializeField] protected GameObject manaPrefab;
    [SerializeField] protected int count = 10;
    [SerializeField] protected Vector2 areaSize = new Vector2(250f, 150f);
    [SerializeField] protected AnimationCurve verticalDensity; 

    
    void Start()
    {
        ScatterMana();
    }
    
    void ScatterMana()
    {
        for (int i = 0; i < count; i++)
        {
            float yNorm = Random.value;
            if (verticalDensity != null && verticalDensity.Evaluate(yNorm) < Random.value)
                continue;

            Vector2 spawnPos = new Vector2(Random.Range(-areaSize.x / 2f, areaSize.x / 2f), Mathf.Lerp(-areaSize.y / 2f, areaSize.y / 2f, yNorm));

            Instantiate(manaPrefab, spawnPos, Quaternion.identity);
        }
    }
}
