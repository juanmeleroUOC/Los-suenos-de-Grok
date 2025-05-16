using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    Rigidbody rb;

    public float forwardSpeed = 5f;
    public float bounceUpForce = 5f;
    public int maxBounces = 4;
    public float lifeTime = 10f;
    public LayerMask groundLayer;

    private int bounceCount = 0;
    private float timer = 0f;
    private bool destroyed = false;

    public GameObject player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
     
    }

    private void Start()
    {
        if (player != null)
        {
            Collider projectileCollider = GetComponent<Collider>();
            Collider playerCollider = player.GetComponent<Collider>();
            Physics.IgnoreCollision(projectileCollider, playerCollider);
        }
    }

    public void SetDirection(Vector3 direction, float multiplier)
    {
        rb.linearVelocity = direction * (forwardSpeed * multiplier);
    }


    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer > lifeTime && !destroyed)
        {
            Destroy(gameObject);
        }
    }

 void OnCollisionEnter(Collision collision)
    {
        if (destroyed) return;

        // Rebota solo contra el suelo (opcional)
        if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            Vector3 currentVelocity = rb.linearVelocity;

            // Conservar velocidad en X y Z, solo modificar Y para el rebote
            currentVelocity.y = bounceUpForce;


            // Aplicar la nueva velocidad sin modificar X y Z
            rb.linearVelocity = currentVelocity;

            bounceCount++;
            if (bounceCount >= maxBounces)
            {
                Destroy(gameObject);
            }
        }
       
    }
}
