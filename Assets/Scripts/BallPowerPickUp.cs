using UnityEngine;
using DG.Tweening;

public class BallPowerPickup : MonoBehaviour
{
    //Variables para dotween (Animación del pickup)
    public float floatAmplitude = 0.25f; // Altura del movimiento
    public float floatDuration = 1.5f;   // Tiempo para subir/bajar

    //public ParticleSystem pickupParticles; 

    private Vector3 initialPosition;


    void Start()
    {
        initialPosition = transform.position;

        // Movimiento vertical 
        transform.DOMoveY(initialPosition.y + floatAmplitude, floatDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    void OnDestroy()
    {
        // Cuando se coge el pickup, destruir las animaciones
        transform.DOKill();
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerPowerUp powerup = other.GetComponent<PlayerPowerUp>();

        if (powerup != null)
        {
            powerup.GrantFirePower();
            Destroy(gameObject);
        }
    }
}