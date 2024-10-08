using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int health = 100; // Vida del enemigo como entero
    [SerializeField] protected int damage = 1; // Daño que causa al jugador
    [SerializeField] protected Color damageColor = Color.red; // Color para el efecto de daño

    private bool isAlive = true; // Estado del enemigo
    private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer

    private Coroutine damageEffectCoroutine; // Coroutine para el efecto de daño

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtener el componente SpriteRenderer
    }

    // Método para aplicar daño al enemigo
    public virtual void TakeDamage(int amount)
    {
        if (!isAlive) return;

        health -= amount; // Reducir la vida

        // Comenzar el efecto de daño
        if (damageEffectCoroutine != null)
            StopCoroutine(damageEffectCoroutine); // Detener cualquier parpadeo anterior
        damageEffectCoroutine = StartCoroutine(ShowDamageEffect()); // Mostrar efecto de daño

        // Comprobar si ha muerto
        if (health <= 0)
        {
            Die(); // Llamar al método de morir
        }
    }

    // Método que se llama al morir
    protected virtual void Die()
    {
        isAlive = false; // Cambiar el estado a no vivo
        if (damageEffectCoroutine != null)
            StopCoroutine(damageEffectCoroutine); // Detener el parpadeo de daño si está activo
        StartCoroutine(DieEffect()); // Iniciar el efecto de muerte
    }

    // Efecto de muerte, parpadea antes de destruir
    private IEnumerator DieEffect()
    {
        float blinkDuration = 0.5f; // Duración total del parpadeo
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

    // Método para mostrar el efecto de daño
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

    // Método para manejar la colisión con el jugador
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isAlive) // Comprobar si colisiona con el jugador y está vivo
        {
            // Aplicar daño al jugador (necesitarás tener referencia al script del jugador)
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Aplicar daño al jugador
            }
        }
    }
}