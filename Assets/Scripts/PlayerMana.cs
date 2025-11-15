// PlayerMana.cs
using System;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [Header("Mana Settings")]
    [Tooltip("The maximum amount of mana the player can hold.")]
    [SerializeField] private float maxMana = 100f;
    private float currentMana;

    // C# event that other scripts can subscribe to. It fires whenever mana changes.
    public event Action<float, float> OnManaChanged;

    // A public property to easily check if mana is full.
    public bool IsManaFull => currentMana >= maxMana;

    private void Start()
    {
        // Player starts with zero mana.
        currentMana = 0;
    }

    // Public method to add mana.
    public void AddMana(float amount)
    {
        // Add the amount, ensuring it doesn't exceed the max.
        currentMana = Mathf.Min(currentMana + amount, maxMana);
        Debug.Log("Mana is now: " + currentMana);

        // Fire the event, sending the current and max mana to all listeners.
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