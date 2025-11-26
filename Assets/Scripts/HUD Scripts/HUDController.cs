using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag your Player GameObject here.")]
    public PlayerMana playerMana;

    private VisualElement manaBarFill;
    
    private void OnEnable()
    {
        var uiDoc = GetComponent<UIDocument>();

        var root = uiDoc.rootVisualElement;

        // Setup Mana
        manaBarFill = root.Q<VisualElement>("mana-bar-fill");
        playerMana = FindAnyObjectByType<PlayerMana>();

        playerMana.OnManaChanged += UpdateManaBar;
        
            UpdateManaBar(playerMana.currentMana, 100f);
        /*if (playerMana != null)
        {
            playerMana.OnManaChanged += UpdateManaBar;
            UpdateManaBar(playerMana.currentMana, 100f);
        }
        */
    }
        void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
}