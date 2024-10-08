using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int health = 100; // Vida del enemigo como entero
    [SerializeField] protected int damage = 1; // Da�o que causa al jugador
    [SerializeField] protected Color damageColor = Color.red; // Color para el efecto de da�o

    private bool isAlive = true; // Estado del enemigo
    private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer

    private Coroutine damageEffectCoroutine; // Coroutine para el efecto de da�o

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtener el componente SpriteRenderer
    }

    // M�todo para aplicar da�o al enemigo
    public virtual void TakeDamage(int amount)
    {
        if (!isAlive) return;

        health -= amount; // Reducir la vida

        // Comenzar el efecto de da�o
        if (damageEffectCoroutine != null)
            StopCoroutine(damageEffectCoroutine); // Detener cualquier parpadeo anterior
        damageEffectCoroutine = StartCoroutine(ShowDamageEffect()); // Mostrar efecto de da�o

        // Comprobar si ha muerto
        if (health <= 0)
        {
            Die(); // Llamar al m�todo de morir
        }
    }

    // M�todo que se llama al morir
    protected virtual void Die()
    {
        isAlive = false; // Cambiar el estado a no vivo
        if (damageEffectCoroutine != null)
            StopCoroutine(damageEffectCoroutine); // Detener el parpadeo de da�o si est� activo
        StartCoroutine(DieEffect()); // Iniciar el efecto de muerte
    }

    // Efecto de muerte, parpadea antes de destruir
    private IEnumerator DieEffect()
    {
        float blinkDuration = 0.5f; // Duraci�n total del parpadeo
        int blinkCount = 5; // Cantidad de parpadeos

        for (int i = 0; i < blinkCount; i++)
        {
            spriteRenderer.enabled = false; // Desactiva el sprite
            yield return new WaitForSeconds(blinkDuration / (blinkCount * 2)); // Espera medio tiempo de parpadeo
            spriteRenderer.enabled = true; // Activa el sprite
            yield return new WaitForSeconds(blinkDuration / (blinkCount * 2)); // Espera medio tiempo de parpadeo
        }

        Destroy(gameObject); // Destruir el objeto
    }

    // M�todo para mostrar el efecto de da�o
    private IEnumerator ShowDamageEffect()
    {
        if (spriteRenderer != null)
        {
            // Cambiar el color del enemigo a rojo
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(0.2f); // Esperar un poco
            spriteRenderer.color = Color.white; // Regresar al color original
        }
    }

    // M�todo para manejar la colisi�n con el jugador
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isAlive) // Comprobar si colisiona con el jugador y est� vivo
        {
            // Aplicar da�o al jugador (necesitar�s tener referencia al script del jugador)
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Aplicar da�o al jugador
            }
        }
    }
}