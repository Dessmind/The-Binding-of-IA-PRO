using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3; // Salud máxima
    private int currentHealth; // Salud actual
    private bool isInvulnerable = false; // Invulnerabilidad
    private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer
    private PlayerUI playerUI; // UI del jugador
    private PlayerMovement playerMovement; // Script de movimiento
    [SerializeField] private GameObject gameOverCanvas; // Canvas de Game Over
    [SerializeField] private GameObject deathParticlesPrefab; // Prefab de partículas de muerte

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerUI = FindObjectOfType<PlayerUI>();
        playerMovement = GetComponent<PlayerMovement>();
        playerUI.UpdateHearts();
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        playerUI.UpdateHearts();
        StartCoroutine(Invulnerability());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player ha muerto.");
        if (playerMovement != null)
        {
            playerMovement.SetAlive(false);
        }

        // Mostrar pantalla de Game Over
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        StartCoroutine(BlinkAndDisappear());
    }

    private IEnumerator BlinkAndDisappear()
    {
        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.2f);
        }

        // Instanciar partículas de muerte después de que el objeto se ha desactivado
        if (deathParticlesPrefab != null)
        {
            Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        }

        gameObject.SetActive(false);
    }

    private IEnumerator Invulnerability()
    {
        isInvulnerable = true;
        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.2f);
        }
        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }
}
