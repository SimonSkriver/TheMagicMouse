using System;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [Header("Mana Settings")]
    [Tooltip("The maximum amount of mana the player can hold.")]
    [SerializeField] private float maxMana = 100f;
    public float currentMana;

    public event Action<float, float> OnManaChanged;

    public bool IsManaFull => currentMana >= maxMana;

    private void Start()
    {
        currentMana = 0;
    }

    public void AddMana(float amount)
    {
        currentMana = Mathf.Min(currentMana + amount, maxMana);
        Debug.Log("Mana is now: " + currentMana);

        OnManaChanged?.Invoke(currentMana, maxMana);
    }

    // Public method to use all mana.
    public void UseAllMana()
    {
        if (IsManaFull)
        {
            currentMana = 0;
            Debug.Log("Big Purify used all mana!");
            OnManaChanged?.Invoke(currentMana, maxMana);

        }
    }
}