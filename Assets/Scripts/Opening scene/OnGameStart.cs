using UnityEngine;
using UnityEngine.UI;

public class OnGameStart : MonoBehaviour
{
    public GameObject Player;
    public GameObject Button;
    public Animator anim;
    private float speed = 7f;
    private bool shouldMovePlayer;
    private float targetPos = -3.5f;

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
