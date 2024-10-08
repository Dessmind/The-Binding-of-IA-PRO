using UnityEngine;

public class ProjectileTurret : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed; // Velocidad del proyectil
    [SerializeField] private GameObject explosionPrefab; // Prefab de part�culas de explosi�n

    private Transform player; // Referencia al jugador
    private Rigidbody2D rb; // Componente Rigidbody2D

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform; // Busca al jugador en la escena
        rb = GetComponent<Rigidbody2D>(); // Obtiene el Rigidbody2D

        LaunchProjectile(); // Lanza el proyectil
        Destroy(gameObject, 3f); // Destruye el proyectil despu�s de 3 segundos
    }

    private void LaunchProjectile()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized; // Calcula la direcci�n hacia el jugador
        rb.velocity = directionToPlayer * speed; // Establece la velocidad del proyectil
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullets"))
        {
            Explode(); // Llama a la funci�n de explosi�n
            Destroy(collision.gameObject); // Destruye el proyectil que colision�
            Destroy(gameObject); // Destruye este proyectil
        }
        else if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>(); // Obtiene el script PlayerHealth del jugador

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Causa 1 punto de da�o al jugador
            }

            Explode(); // Llama a la funci�n de explosi�n
            Destroy(gameObject); // Destruye este proyectil
        }
        else if (collision.CompareTag("Walls")) // Nueva condici�n para el tag "Walls"
        {
            Explode(); // Llama a la funci�n de explosi�n
            Destroy(gameObject); // Destruye este proyectil
        }
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            // Instancia el prefab de explosi�n en la posici�n del proyectil
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }
}
