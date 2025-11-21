// QuestManager.cs
using System;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public enum QuestState
    {
        NotStarted,
        MetNPC,
        HasLute,
        BranchBroken,
        Completed
    }

    [Header("Debug")]
    [Tooltip("Select a state here to start the game with it, or apply it during runtime via Context Menu.")]
    [SerializeField] private QuestState debugStartState = QuestState.NotStarted;

    // Variable is now visible in Debug mode, but protected
    public QuestState currentQuestState { get; private set; }

    public event Action<QuestState> OnQuestStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        // Apply the debug state on start
        if (debugStartState != QuestState.NotStarted)
        {
            SetQuestState(debugStartState);
        }
    }

    // --- NEW: Right-click the script in Inspector to trigger this ---
    [ContextMenu("Apply Debug State Now")]
    public void ApplyDebugState()
    {
        SetQuestState(debugStartState);
    }

    public void SetQuestState(QuestState newState)
    {
        if (newState == currentQuestState) return;

        currentQuestState = newState;
        Debug.Log("Quest state changed to: " + newState);
        OnQuestStateChanged?.Invoke(newState);
    }
}