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
        var root = GetComponent<UIDocument>().rootVisualElement;
        manaBarFill = root.Q<VisualElement>("mana-bar-fill");

        // Ensure playerMana is assigned before proceeding
        if (playerMana != null)
        {
            // Subscribe our UpdateManaBar method to the player's OnManaChanged event.
            playerMana.OnManaChanged += UpdateManaBar;

            // Set the initial state of the bar based on the player's actual current mana.
            UpdateManaBar(playerMana.currentMana, 100f); // Assuming max is 100
        }
        else
        {
            Debug.LogError("PlayerMana reference is not set in the HUDController!");
        }
    }

    private void OnDisable()
    {
        // IMPORTANT: Unsubscribe when the object is disabled to prevent errors.
        if (playerMana != null)
        {
            playerMana.OnManaChanged -= UpdateManaBar;
        }
    }

    private void UpdateManaBar(float currentMana, float maxMana)
    {
        if (manaBarFill == null) return; // Safety check
        if (maxMana <= 0) return;

        // Calculate the percentage and update the height of the fill element.
        float percent = currentMana / maxMana;
        manaBarFill.style.height = Length.Percent(percent * 100);
    }
}