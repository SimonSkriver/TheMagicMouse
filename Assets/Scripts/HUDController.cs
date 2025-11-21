// HUDController.cs
using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    [Tooltip("Drag your Player GameObject here.")]
    public PlayerMana playerMana;

    private VisualElement manaBarFill;

    private void OnEnable()
    {
        // Find the UI elements by name.
        var root = GetComponent<UIDocument>().rootVisualElement;
        manaBarFill = root.Q<VisualElement>("mana-bar-fill");

        // Subscribe our UpdateManaBar method to the player's OnManaChanged event.
        playerMana.OnManaChanged += UpdateManaBar;

        // Set the initial state of the bar.
        UpdateManaBar(0, 100);
    }

    private void OnDisable()
    {
        // IMPORTANT: Unsubscribe when the object is disabled to prevent errors.
        playerMana.OnManaChanged -= UpdateManaBar;
    }

    private void UpdateManaBar(float currentMana, float maxMana)
    {
        if (maxMana <= 0) return;

        // Calculate the percentage and update the width of the fill element.
        float percent = currentMana / maxMana;
        manaBarFill.style.width = Length.Percent(percent * 100);
    }
}