using UnityEngine;

public class ProjectileEscapist : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed;                     // Velocidad del proyectil
    [SerializeField] private GameObject explosionPrefab;       // Prefab de la explosión
    [SerializeField] private float lifespan = 3f;             // Tiempo de vida del proyectil en segundos

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            player = playerMovement.transform;
        }

        rb = GetComponent<Rigidbody2D>();

        if (player != null)
        {
            LaunchProjectile();
        }

        // Programar la destrucción del proyectil después de su lifespan
        Destroy(gameObject, lifespan);
    }

    private void LaunchProjectile()
    {
        if (player != null)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            rb.velocity = directionToPlayer * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Solo colisiona con el jugador
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }

            Explode();
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }
}
