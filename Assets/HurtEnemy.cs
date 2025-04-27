using UnityEngine;

public class HurtEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyHealthManager>().TakeDamage();
        }
    }
}
