using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float lifeTime = 3f;
    public int maxBounces = 1;
    public float bounceForce = 6f;

    private int bounceCount = 0;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Si choca con el suelo (normal apuntando hacia arriba)
        if (collision.contacts[0].normal.y > 0.5f)
        {
            if (bounceCount < maxBounces)
            {
                bounceCount++;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); 
                rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // Si choca con pared u objeto
            Destroy(gameObject);
        }
    }
}
