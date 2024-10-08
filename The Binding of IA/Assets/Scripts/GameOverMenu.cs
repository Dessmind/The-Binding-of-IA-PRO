using UnityEngine;
using UnityEngine.SceneManagement; // Asegúrate de importar esto para poder cargar escenas

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverCanvas; // Referencia al Canvas de Game Over
    public GameObject hud; // Referencia al HUD

    private void Start()
    {
        hud.SetActive(true); // Asegúrate de que el HUD esté activo al inicio
        gameOverCanvas.SetActive(false); // Asegúrate de que el Canvas de Game Over esté oculto al inicio
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reinicia la escena actual
    }

    public void QuitGame()
    {
        Application.Quit(); // Cierra la aplicación
    }

    public void LoadMainMenu() // Nueva función para cargar el menú principal
    {
        Time.timeScale = 1; // Asegúrate de restaurar el tiempo del juego
        SceneManager.LoadScene("MainMenu"); // Carga la escena llamada "MainMenu"
    }
}
