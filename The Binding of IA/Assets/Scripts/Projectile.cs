using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 10; // Daño del proyectil
    [SerializeField] private float lifespan = 2f; // Tiempo de vida del proyectil
    [SerializeField] private GameObject destructionEffectPrefab; // Prefab de la partícula de destrucción

    private void Start()
    {
        Destroy(gameObject, lifespan); // Destruir el proyectil después de un tiempo
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullets"))
        {
            // Destruir ambos proyectiles y crear el efecto de destrucción
            Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Aplicar daño al enemigo
            }
            Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject); // Destruir el proyectil tras colisionar
        }
        else if (collision.CompareTag("Destructible"))
        {
            Destructible destructible = collision.GetComponent<Destructible>();
            if (destructible != null)
            {
                destructible.TakeDamage(damage); // Aplicar daño al objeto destructible
            }
            Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject); // Destruir el proyectil tras colisionar
        }
        else if (collision.CompareTag("Walls")) // Nueva condición para el tag "Walls"
        {
            Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity); // Crear efecto de destrucción
            Destroy(gameObject); // Destruye este proyectil
        }
    }
}
