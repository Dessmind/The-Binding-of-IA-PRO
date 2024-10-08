using UnityEngine;
using UnityEngine.SceneManagement; // Aseg�rate de importar esto para poder cargar escenas

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverCanvas; // Referencia al Canvas de Game Over
    public GameObject hud; // Referencia al HUD
    private PlayerUI playerUI; // Referencia al script PlayerUI

    private void Start()
    {
        hud.SetActive(true); // Aseg�rate de que el HUD est� activo al inicio
        gameOverCanvas.SetActive(false); // Aseg�rate de que el Canvas de Game Over est� oculto al inicio
        playerUI = FindObjectOfType<PlayerUI>(); // Encuentra el PlayerUI en la escena
    }

    public void ShowGameOverScreen()
    {
        Time.timeScale = 0; // Pausa el juego
        hud.SetActive(false); // Oculta el HUD
        gameOverCanvas.SetActive(true); // Muestra la pantalla de Game Over
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Restaura el tiempo del juego
        hud.SetActive(true); // Muestra el HUD
        playerUI.ResetScore(); // Reinicia el puntaje
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reinicia la escena actual
    }

    public void QuitGame()
    {
        Application.Quit(); // Cierra la aplicaci�n
    }

    public void LoadMainMenu() // Nueva funci�n para cargar el men� principal
    {
        Time.timeScale = 1; // Aseg�rate de restaurar el tiempo del juego
        SceneManager.LoadScene("MainMenu"); // Carga la escena llamada "MainMenu"
    }
}
