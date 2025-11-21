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
        if (uiDoc == null) return;

        var root = uiDoc.rootVisualElement;

        // Setup Mana
        manaBarFill = root.Q<VisualElement>("mana-bar-fill");

        // Setup Lute Icon (Hide it by default)
        luteIcon = root.Q<VisualElement>(luteIconName);
        if (luteIcon != null)
        {
            luteIcon.style.display = DisplayStyle.None; // Start hidden
        }

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
        if (manaBarFill == null || maxMana <= 0) return;
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