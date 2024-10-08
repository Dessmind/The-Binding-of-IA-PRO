using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifespan = 2f;
    [SerializeField] private GameObject destructionEffectPrefab;

    private void Start()
    {
        Destroy(gameObject, lifespan);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullets"))
        {
            Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Destructible"))
        {
            Destructible destructible = collision.GetComponent<Destructible>();
            if (destructible != null)
            {
                destructible.TakeDamage(damage);
            }
            Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Walls"))
        {
            Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Obstacles")) // Nueva condición para "Obstacles"
        {
            Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
