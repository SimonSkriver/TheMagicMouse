// CutsceneTrigger.cs
using UnityEngine;
using UnityEngine.Events;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private QuestManager.QuestState requiredState;
    [SerializeField] private QuestManager.QuestState newState;
    public UnityEvent OnCutsceneStart;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && QuestManager.Instance.currentQuestState == requiredState)
        {
            QuestManager.Instance.SetQuestState(newState);
            OnCutsceneStart.Invoke();
            gameObject.SetActive(false); // Trigger only once
        }
    }
}