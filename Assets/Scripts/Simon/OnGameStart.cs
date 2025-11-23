using UnityEngine;

public class OnGameStart : MonoBehaviour
{
    public GameObject Player;
    public GameObject Button;
    public float speed;
    private bool shouldMovePlayer;
    private float targetPos = -12.1f;
    public PlayerController pc;
    private bool buttonHasBeenPressed = false;
    public float secondsAfterButtonPress = 2f;
    void Start()
    {
        pc.enabled = false;
    }
    void Update()
    {
        EnablePlayerControls();
    }
    void FixedUpdate()
    {
        if (shouldMovePlayer)
        {
            Vector3 pos = Player.transform.position;
            
            if (pos.x < targetPos)
            {
                pos.x += speed * Time.deltaTime;
                Player.transform.position = pos;
            }
            else
            {
                shouldMovePlayer = false;
            }
        }
    }
    public void ButtonPress()
    {
        shouldMovePlayer = true;
        buttonHasBeenPressed = true;
        Destroy(Button);
        Debug.Log("You have pressed the button");
    }
    void PlayerCanMove()
    {
        pc.enabled = true;
    }
    void EnablePlayerControls()
    {
     if (buttonHasBeenPressed == true) //if statements går altid ud fra at men checker om ens boolean er true unless man checker at den er = false
        {
            Invoke("PlayerCanMove", secondsAfterButtonPress);
         Debug.Log("Player Controllers enabled.");
        }
    }
}
