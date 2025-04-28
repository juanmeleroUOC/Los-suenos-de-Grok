using UnityEngine;

public class LevelExit : MonoBehaviour
{

    public Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            animator.SetTrigger("HitLevelEnd");

            StartCoroutine(GameManager.instance.LevelEndWaiter());
        }
    }

}
