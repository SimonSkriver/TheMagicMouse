using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag your Player GameObject here.")]
    public PlayerMana playerMana;

    [Header("UI Names")]
    [Tooltip("The name of the visual element for the Lute Icon in UI Builder.")]
    public string luteIconName = "lute-icon";

    private VisualElement manaBarFill;
    private VisualElement luteIcon; // Reference to the icon

    private void OnEnable()
    {
        var uiDoc = GetComponent<UIDocument>();

        var root = uiDoc.rootVisualElement;

        // Setup Mana
        manaBarFill = root.Q<VisualElement>("mana-bar-fill");

        if (playerMana != null)
        {
            playerMana.OnManaChanged += UpdateManaBar;
            UpdateManaBar(playerMana.currentMana, 100f);
        }
    }

    private void OnDisable()
    {
        if (playerMana != null)
        {
            playerMana.OnManaChanged -= UpdateManaBar;
        }
    }

    private void UpdateManaBar(float currentMana, float maxMana)
    {
        float percent = currentMana / maxMana;
        manaBarFill.style.height = Length.Percent(percent * 100);
    }

    // --- NEW METHOD ---
    public void ShowLuteIcon()
    {
        if (luteIcon != null)
        {
            luteIcon.style.display = DisplayStyle.Flex; // Make it visible
            Debug.Log("UI: Lute Icon Displayed");
        }
        else
        {
            Debug.LogWarning($"Could not find UI element with name '{luteIconName}'");
        }
    }
}