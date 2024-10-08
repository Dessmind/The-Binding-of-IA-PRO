using UnityEngine;

public class ProjectileTurret : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed; // Velocidad del proyectil
    [SerializeField] private GameObject explosionPrefab; // Prefab de partículas de explosión

    private Transform player; // Referencia al jugador
    private Rigidbody2D rb; // Componente Rigidbody2D

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform; // Busca al jugador en la escena
        rb = GetComponent<Rigidbody2D>(); // Obtiene el Rigidbody2D

        LaunchProjectile(); // Lanza el proyectil
        Destroy(gameObject, 3f); // Destruye el proyectil después de 3 segundos
    }

    private void LaunchProjectile()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized; // Calcula la dirección hacia el jugador
        rb.velocity = directionToPlayer * speed; // Establece la velocidad del proyectil
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullets"))
        {
            Explode(); // Llama a la función de explosión
            Destroy(collision.gameObject); // Destruye el proyectil que colisionó
            Destroy(gameObject); // Destruye este proyectil
        }
        else if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>(); // Obtiene el script PlayerHealth del jugador

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Causa 1 punto de daño al jugador
            }

            Explode(); // Llama a la función de explosión
            Destroy(gameObject); // Destruye este proyectil
        }
        else if (collision.CompareTag("Walls")) // Nueva condición para el tag "Walls"
        {
            Explode(); // Llama a la función de explosión
            Destroy(gameObject); // Destruye este proyectil
        }
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            // Instancia el prefab de explosión en la posición del proyectil
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }
}
