using UnityEngine;

public class PowerUp2 : MonoBehaviour
{
    [SerializeField] private float duration = 3.5f; // Duraci�n del power-up
    [SerializeField] private float disappearTime = 5f; // Tiempo antes de desaparecer
    [SerializeField] private float blinkDuration = 2f; // Duraci�n del parpadeo antes de desaparecer
    [SerializeField] private float speedUpMultiplier = 3f; // Multiplicador para la velocidad de disparo
    private PlayerMovement playerMovement; // Referencia al script PlayerMovement
    private SpriteRenderer spriteRenderer; // Componente SpriteRenderer

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtener el SpriteRenderer del objeto
        transform.rotation = Quaternion.Euler(0, 0, 90); // Rotar 90 grados en el eje Z
        StartCoroutine(HandleDisappearance()); // Iniciar la coroutine para manejar la desaparici�n
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el jugador ha recogido el power-up
        if (other.CompareTag("Player"))
        {
            playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.ActivateRapidFire(duration, speedUpMultiplier); // Activar disparo r�pido
            }
            Destroy(gameObject); // Destruir el power-up despu�s de recogerlo
        }
    }

    // Coroutine para manejar la desaparici�n del power-up
    private System.Collections.IEnumerator HandleDisappearance()
    {
        // Espera el tiempo antes de comenzar a parpadear
        yield return new WaitForSeconds(disappearTime - blinkDuration);

        // Comienza a parpadear
        float blinkInterval = 0.2f; // Intervalo de parpadeo
        float elapsedTime = 0f; // Tiempo transcurrido

        while (elapsedTime < blinkDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Alterna la visibilidad del sprite
            elapsedTime += blinkInterval; // Aumenta el tiempo transcurrido
            yield return new WaitForSeconds(blinkInterval); // Espera el intervalo de parpadeo
        }

        // Destruye el power-up despu�s de parpadear
        Destroy(gameObject);
    }
}
