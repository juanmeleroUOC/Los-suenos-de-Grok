using DG.Tweening;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount;
    public bool isFullHeal;

    public GameObject healthParticlesEffect;

    private Vector3 initialPosition;
    void Start()
    {
        initialPosition = transform.position;
        transform.DOMoveY(initialPosition.y + 0.25f, 1.25f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

        
    }

    void Update()
    {
        // rotar sobre si mismo
        transform.Rotate(0, 20f * Time.deltaTime, 0);
    }

    void OnDestroy()
    {
        // Cuando se coge el pickup, destruir las animaciones
        transform.DOKill();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);

            Instantiate(healthParticlesEffect, PlayerMovement.instance.transform.position + new Vector3(0f, 1f, 0f), PlayerMovement.instance.transform.rotation);

            if (isFullHeal)
            {
                HealthManager.instance.ResetHealth();
            }
            else
            {
                HealthManager.instance.AddHealth(healAmount);
            }
        }
    }

}
