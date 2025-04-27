using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{

    public int maxHeatlh = 1;
    public int currentHeatlh;

    public int deathSound;

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
        }
    }

}
