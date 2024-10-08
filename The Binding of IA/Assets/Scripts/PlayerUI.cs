using UnityEngine;
using UnityEngine.UI;
using TMPro; // Importar el espacio de nombres de TextMeshPro

public class PlayerUI : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private Image[] hearts; // Drag and drop the 3 hearts here in the inspector
    private PlayerHealth playerHealth;

    [Header("Score Settings")]
    [SerializeField] private TextMeshProUGUI scoreText; // Cambiar a TextMeshProUGUI
    private int currentScore;

    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        InitializeHearts(); // Inicializa los corazones al inicio
        UpdateHearts(); // Actualiza las vidas al inicio
        currentScore = 0; // Inicializa el puntaje en cero
        UpdateScoreUI(); // Actualiza la UI del puntaje al inicio
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

    public void AddScore(int amount)
    {
        currentScore += amount; // Aumenta el puntaje
        UpdateScoreUI(); // Actualiza la UI con el nuevo puntaje
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + currentScore.ToString(); // Actualiza el texto de puntaje en la UI
    }

    // Si necesitas reiniciar el puntaje, puedes agregar un método aquí
    public void ResetScore()
    {
        currentScore = 0; // Reinicia el puntaje
        UpdateScoreUI(); // Actualiza la UI
    }
}
