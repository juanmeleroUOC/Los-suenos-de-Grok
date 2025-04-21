using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;

    public int currentHealth, maxHealth;

    public float invincibleLength = 2f;
    private float invincCounter;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(invincCounter > 0)
        {
            invincCounter -= Time.deltaTime;

            //feedback de daño
            for (int i = 0; i < PlayerMovement.instance.playerPieces.Length; i++)
            {
                if(Mathf.Floor(invincCounter * 5f) % 2 == 0)
                {
                    PlayerMovement.instance.playerPieces[i].SetActive(true);
                } else
                {
                    PlayerMovement.instance.playerPieces[i].SetActive(false);
                }

                if (invincCounter < 0)
                {
                    PlayerMovement.instance.playerPieces[i].SetActive(true);
                }
            }
          
        }
    }

    public void Hurt()
    {
        if(invincCounter <= 0)
        {
            currentHealth--;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                GameManager.instance.Respawn();
            }
            else
            {
                PlayerMovement.instance.Knockback();
                invincCounter = invincibleLength;

            }
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    public void AddHealth(int amountToHeal)
    {
        currentHealth += amountToHeal;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

    }

}
