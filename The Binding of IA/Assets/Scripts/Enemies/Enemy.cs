using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int health = 100;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected Color damageColor = Color.red;

    protected bool isAlive = true; // Cambiado a protected para que sea accesible en TankEnemy
    private SpriteRenderer spriteRenderer;

    private Coroutine damageEffectCoroutine;

    // Para las partículas
    [SerializeField] private GameObject explosionParticlesPrefab; // Asignar en el Inspector

    // Nueva variable para la cantidad de puntos
    [SerializeField] private int pointsValue = 100; // Asignar en el Inspector

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on Enemy object.");
        }
    }

    public virtual void TakeDamage(int amount)
    {
        if (!isAlive) return;

        health -= amount;

        if (damageEffectCoroutine != null)
            StopCoroutine(damageEffectCoroutine);
        damageEffectCoroutine = StartCoroutine(ShowDamageEffect());

        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isAlive = false;
        if (damageEffectCoroutine != null)
            StopCoroutine(damageEffectCoroutine);

        // Llama a la función para sumar puntos
        PlayerUI playerUI = FindObjectOfType<PlayerUI>();
        if (playerUI != null)
        {
            playerUI.AddScore(pointsValue); // Sumar puntos al jugador
        }

        StartCoroutine(DieEffect());
    }

    private IEnumerator DieEffect()
    {
        // Aquí puedes inicializar las partículas justo antes de destruir el objeto
        if (explosionParticlesPrefab != null)
        {
            // Instanciamos las partículas en la posición del enemigo
            GameObject explosion = Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 2f); // Destruir las partículas después de 2 segundos
        }
        else
        {
            Debug.LogError("Explosion particles prefab is not assigned in the Inspector.");
        }

        float blinkDuration = 0.5f;
        int blinkCount = 5;

        for (int i = 0; i < blinkCount; i++)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
                yield return new WaitForSeconds(blinkDuration / (blinkCount * 2));
                spriteRenderer.enabled = true;
                yield return new WaitForSeconds(blinkDuration / (blinkCount * 2));
            }
        }

        Destroy(gameObject);
    }

    private IEnumerator ShowDamageEffect()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = Color.white;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isAlive)
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
