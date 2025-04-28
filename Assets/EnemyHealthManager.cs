using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{

    public int maxHeatlh = 1;
    public int currentHeatlh;

    public int deathSound;

    public GameObject deathEffect;

    void Start()
    {
        currentHeatlh = maxHeatlh;
    }

    public void TakeDamage()
    {
        currentHeatlh--;
        if (currentHeatlh <= 0)
        {
            // AudioManager.instance.PlaySFX(deathSound);
            Destroy(gameObject);
            Instantiate(deathEffect, transform.position, transform.rotation);

        }
        PlayerMovement.instance.Bounce();

    }

}
