using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float duration = 3.5f; // Duración del power-up
    [SerializeField] private float colorChangeSpeed = 1f; // Velocidad de cambio de color
    [SerializeField] private float disappearTime = 5f; // Tiempo antes de desaparecer
    [SerializeField] private float blinkDuration = 2f; // Duración del parpadeo antes de desaparecer
    private PlayerMovement playerMovement;
    private SpriteRenderer spriteRenderer; // Componente SpriteRenderer

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtener el SpriteRenderer del objeto
        StartCoroutine(ChangeColor()); // Iniciar la coroutine para cambiar colores
        StartCoroutine(HandleDisappearance()); // Iniciar la coroutine para manejar la desaparición
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el jugador ha recogido el power-up
        if (other.CompareTag("Player"))
        {
            playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.ActivatePowerUp(duration);
            }
            Destroy(gameObject); // Destruir el power-up después de recogerlo
        }
    }

    // Coroutine para cambiar el color del power-up
    private System.Collections.IEnumerator ChangeColor()
    {
        // Define los colores pastel
        Color[] colors = new Color[]
        {
            new Color(1f, 0.6f, 0.6f),   // Rojo pastel
            new Color(1f, 1f, 0.6f),      // Amarillo pastel
            new Color(0.6f, 1f, 0.6f),    // Verde pastel
            new Color(0.6f, 1f, 1f),       // Cian pastel
            new Color(0.6f, 0.6f, 1f),     // Azul pastel
            new Color(1f, 0.6f, 1f)        // Magenta pastel
        };

        while (true)
        {
            // Cambia de color en un ciclo
            for (int i = 0; i < colors.Length; i++)
            {
                spriteRenderer.color = colors[i]; // Cambia al color actual
                yield return new WaitForSeconds(colorChangeSpeed); // Espera antes de cambiar al siguiente color
            }
        }
    }

    // Coroutine para manejar la desaparición del power-up
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

        // Destruye el power-up después de parpadear
        Destroy(gameObject);
    }
}
