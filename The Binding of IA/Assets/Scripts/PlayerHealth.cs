using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3; // Salud máxima como entero
    private int currentHealth; // Salud actual como entero
    private bool isInvulnerable = false; // Indica si el jugador es invulnerable
    private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer
    private PlayerUI playerUI; // Referencia a la UI del jugador
    private PlayerMovement playerMovement; // Referencia al script de movimiento
    [SerializeField] private GameObject gameOverCanvas; // Referencia al Canvas de Game Over

    void Start()
    {
        currentHealth = maxHealth; // Inicializa la salud actual
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtiene el SpriteRenderer
        playerUI = FindObjectOfType<PlayerUI>(); // Busca la UI del jugador
        playerMovement = GetComponent<PlayerMovement>(); // Obtiene el script de movimiento
        playerUI.UpdateHearts(); // Actualiza las vidas en la interfaz al inicio
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return; // Si es invulnerable, no recibe daño

        currentHealth -= damage; // Reduce la salud actual
        playerUI.UpdateHearts(); // Actualiza la interfaz
        StartCoroutine(Invulnerability()); // Activa la invulnerabilidad

        if (currentHealth <= 0)
        {
            Die(); // Llama al método de muerte si la salud es cero o menos
        }
    }

    private void Die()
    {
        Debug.Log("Player ha muerto.");
        if (playerMovement != null) // Verificar que playerMovement no sea null
        {
            playerMovement.SetAlive(false); // Detiene el movimiento del jugador
        }

        // Mostrar pantalla de Game Over utilizando la referencia del Canvas
        if (gameOverCanvas != null) // Verifica que gameOverCanvas no sea null
        {
            gameOverCanvas.SetActive(true); // Activa el Canvas de Game Over
        }

        StartCoroutine(BlinkAndDisappear()); // Llama al parpadeo y desaparición
    }

    private IEnumerator BlinkAndDisappear()
    {
        for (int i = 0; i < 5; i++) // Parpadeo de 5 veces
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Alterna la visibilidad
            yield return new WaitForSeconds(0.2f);
        }
        gameObject.SetActive(false); // Desactiva el objeto y sus hijos
    }

    private IEnumerator Invulnerability()
    {
        isInvulnerable = true; // Activa la invulnerabilidad
        for (int i = 0; i < 5; i++) // Parpadeo de 5 veces
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Alterna la visibilidad
            yield return new WaitForSeconds(0.2f);
        }
        spriteRenderer.enabled = true; // Asegúrate de que el sprite esté visible
        isInvulnerable = false; // Desactiva la invulnerabilidad
    }

    public int GetCurrentHealth()
    {
        return currentHealth; // Devuelve la salud actual
    }
}
