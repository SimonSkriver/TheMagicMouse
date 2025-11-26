using UnityEngine;


public class QuitGame : MonoBehaviour
{
    public void ExitButton()
    {
        Debug.Log("Button pressed");
        Application.Quit();
    }
}
