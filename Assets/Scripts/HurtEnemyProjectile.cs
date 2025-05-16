using UnityEngine;

public class HurtEnemyProjectile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyKenops" || other.tag =="EnemyShelly")
        { 
            // lo hago así porque tiene dos colliders, el del salto del jugador en la cabeza y el del powerup, así que uso un empty object hijo y no pongo el collider directamente en el enemigo
            EnemyHealthManager enemyHealth = other.GetComponentInParent<EnemyHealthManager>();
            enemyHealth.TakeDamage();
            Destroy(gameObject);
        }

        if (other.tag == "EnemyShelly")
        {
            other.GetComponent<EnemyHealthManager>().TakeDamage();
            Destroy(gameObject);
        }
    }
}
