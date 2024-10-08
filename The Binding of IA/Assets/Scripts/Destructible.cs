using UnityEngine;
using System.Collections; // Necesario para usar coroutines

public class Destructible : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3; // Salud m�xima
    private int currentHealth; // Salud actual

    [SerializeField] private float blinkDuration = 0.5f; // Duraci�n total del parpadeo
    [SerializeField] private int blinkCount = 5; // Cantidad de parpadeos

    private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer

    private void Start()
    {
        currentHealth = maxHealth; // Inicializa la salud
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtiene el componente SpriteRenderer
    }

    // M�todo para aplicar da�o
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce la salud actual
        StartCoroutine(Blink()); // Inicia el efecto de parpadeo

        if (currentHealth <= 0)
        {
            Die(); // Llama al m�todo de morir
        }
    }

    // M�todo que se llama al morir
    private void Die()
    {
        Destroy(gameObject); // Destruir el objeto si su salud llega a 0
    }

    // M�todo para mostrar el efecto de parpadeo
    private IEnumerator Blink()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            spriteRenderer.enabled = false; // Desactiva el sprite
            yield return new WaitForSeconds(blinkDuration / (blinkCount * 2)); // Espera medio tiempo de parpadeo
            spriteRenderer.enabled = true; // Activa el sprite
            yield return new WaitForSeconds(blinkDuration / (blinkCount * 2)); // Espera medio tiempo de parpadeo
        }
    }
}
