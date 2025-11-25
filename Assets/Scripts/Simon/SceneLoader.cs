using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string nextScene;
    public Animator transition;
    [SerializeField] private float transitionDuration = 1f; //Overall duration of transition, which is adjustable
    private float fadeAnimInSeconds = 1f; //duration of keyframed transition anim. NO TOUCHY!!

    
    void Start()
    {
        transition.speed = fadeAnimInSeconds / transitionDuration; //Set animator speed so that fade in plays at the specified rate
    }
    void OnTriggerEnter2D(Collider2D other) //check if player done did enter the scene loader
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered sceneloader");
            LoadNextScene();
        }
    }

    public void LoadNextScene() //loads next scene by starting the coroutine
    {
        Debug.Log("Loading: "+ nextScene);

        if (nextScene == "Bossbattle") //Check to see if loading Bossbattle, in which case we want to load instantly for jumpscare effect
        {
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single); //load specified scene
        }
        else
        {
            StartCoroutine(LoadScene()); //Else starts coroutine
        }
    }

    IEnumerator LoadScene() //Coroutine making sure scene only loads after animation plays
    {
        transition.speed = fadeAnimInSeconds / transitionDuration; //Sync anim speed to transition duration
        transition.SetTrigger("Start"); //start transition animation
        yield return new WaitForSeconds(transitionDuration); //wait for transition duration, so that anim can play
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single); //load specified scene

    }
}