using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Llamamos al m�todo Hurt() de HealthManager
            HealthManager.instance.Hurt(other.transform);
            other.GetComponent<PlayerPowerUp>().LoseFirePower();
        }
    }
}
