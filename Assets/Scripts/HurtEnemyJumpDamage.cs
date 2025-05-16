using UnityEngine;

public class HurtEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyKenops")
        {
            other.GetComponent<EnemyHealthManager>().TakeDamage();
        }
    }
}
