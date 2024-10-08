using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image[] hearts; // Drag and drop the 3 hearts here in the inspector
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        InitializeHearts(); // Inicializa los corazones al inicio
        UpdateHearts(); // Actualiza las vidas al inicio
    }

    private void InitializeHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].color = new Color(1, 1, 1, 1); // Asegúrate de que todos los corazones tengan opacidad completa al iniciar
        }
    }

    public void UpdateHearts()
    {
        int currentHealth = playerHealth.GetCurrentHealth();

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].color = new Color(1, 1, 1, 1); // Opacidad completa
            }
            else
            {
                hearts[i].color = new Color(1, 1, 1, 0.3f); // Opacidad reducida
            }
        }
    }
}
