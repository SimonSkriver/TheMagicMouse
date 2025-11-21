// CinematicBarsManager.cs (Final, Explicit, Correct Version)
using System.Collections.Generic; // Add this namespace
using UnityEngine;
using UnityEngine.UIElements;

public class CinematicBarsManager : MonoBehaviour
{
    public static CinematicBarsManager Instance { get; private set; }

    private VisualElement topBar;
    private VisualElement bottomBar;

    private void Awake() { Instance = this; }

    private void Start()
    {
        var uiDocument = FindAnyObjectByType<UIDocument>();
        if (uiDocument != null)
        {
            topBar = uiDocument.rootVisualElement.Q("top-bar");
            bottomBar = uiDocument.rootVisualElement.Q("bottom-bar");
        }
    }

    public void ShowBars(float animationTime = 0.6f)
    {
        if (topBar == null || bottomBar == null) return;

        // Create a new list containing our single time value.
        var duration = new List<TimeValue> { new TimeValue(animationTime, TimeUnit.Second) };
        topBar.style.transitionDuration = duration;
        bottomBar.style.transitionDuration = duration;

        topBar.schedule.Execute(() => topBar.style.top = 0);
        bottomBar.schedule.Execute(() => bottomBar.style.bottom = 0);
    }

    public void HideBars(float animationTime = 0.3f)
    {
        if (topBar == null || bottomBar == null) return;

        var duration = new List<TimeValue> { new TimeValue(animationTime, TimeUnit.Second) };
        topBar.style.transitionDuration = duration;
        bottomBar.style.transitionDuration = duration;

        topBar.schedule.Execute(() => topBar.style.top = Length.Percent(-10));
        bottomBar.schedule.Execute(() => bottomBar.style.bottom = Length.Percent(-10));
    }
}