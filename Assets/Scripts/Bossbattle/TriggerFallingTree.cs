using UnityEngine;

public class TriggerFallingTree : MonoBehaviour
{
    private Animator treeAnimator;

    void Awake()
    {
        GameObject tree = GameObject.Find("FinaleAccessTree");
        treeAnimator =  tree.GetComponent<Animator>();
    }
    
    void TriggerFall()
    {
        treeAnimator.SetTrigger("TreeFall");
    }
}
