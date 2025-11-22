using UnityEngine;

public class PlayerPersistence : MonoBehaviour
{
//This script makes the player persistent, allowing for seemless scene loads between corrupted state and purified state.
//THIS SHOULD ONLY BE ON THE PLAYER, WHEN WE DON'T WANT TO RESET THE GUY BETWEEN SCENE LOADS
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
