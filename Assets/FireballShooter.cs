using UnityEngine;

public class FireballShooter : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform firePoint; // punto desde el que se lanza la bola
    public float fireForce = 10f;

    private Animator animator;

    public PlayerMovement playerMovementScript; // referencia al script que tiene CurrentVelocity

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // cambia esta tecla si lo necesitas
        {
            ShootFireball();
        }
    }


    void ShootFireball()
    {
        if (animator != null)
        {
            animator.ResetTrigger("Shoot");
            animator.SetTrigger("Shoot");
        }
            

        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 inheritedVelocity = playerMovementScript.CurrentVelocity;
            Vector3 launchDirection = (firePoint.forward + Vector3.up * 0.3f).normalized;

            rb.linearVelocity = inheritedVelocity; // hereda la velocidad del jugador
            rb.AddForce(launchDirection * fireForce, ForceMode.Impulse);
        }
    }
}