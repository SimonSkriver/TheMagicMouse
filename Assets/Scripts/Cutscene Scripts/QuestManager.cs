// QuestManager.cs
using System;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // Singleton Pattern: A single, globally accessible instance of the Quest Manager.
    public static QuestManager Instance { get; private set; }

    // Enum to define the possible states of our quest.
    public enum QuestState
    {
        NotStarted,
        MetNPC,
        HasLute,
        BranchBroken,
        Completed
    }

    public QuestState currentQuestState { get; private set; }

    // C# Event that fires whenever the quest state changes.
    public event Action<QuestState> OnQuestStateChanged;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: makes it persist between scenes.
        }
    }

    // A public method that any script can call to advance the quest.
    public void SetQuestState(QuestState newState)
    {
        if (newState == currentQuestState) return;

        currentQuestState = newState;
        Debug.Log("Quest state changed to: " + newState);

        // Fire the event to notify all listeners (like the UI).
        OnQuestStateChanged?.Invoke(newState);
    }
}