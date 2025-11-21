using UnityEngine;

public class OnGameStart : MonoBehaviour
{
    public GameObject Player;
    public GameObject Button;
    public float speed;
    private bool shouldMovePlayer;
    private float targetPos = -12.1f;

    void Start()
    {
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
        Destroy(Button);
        Debug.Log("You have pressed the button");
    }
}
