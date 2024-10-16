using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUpPrefabs; // Prefabs de power-ups que se pueden generar
    [SerializeField] private float spawnDelay = 2f; // Intervalo de tiempo para generar nuevos power-ups
    [SerializeField] private float width, height; // Dimensiones del área de spawn
    private PlayerHealth playerHealth; // Referencia al script de salud del jugador

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>(); // Busca el script de salud del jugador
        StartCoroutine(SpawnPowerUps()); // Inicia la corutina para spawn de power-ups
    }

    private IEnumerator SpawnPowerUps()
    {
        // Espera el tiempo de spawn inicial antes de comenzar a generar power-ups
        yield return new WaitForSeconds(spawnDelay);

        while (true)
        {
            // Verifica si el jugador está vivo
            if (playerHealth.GetCurrentHealth() > 0)
            {
                // Selecciona un prefab de power-up aleatorio
                int randomPowerUpIndex = Random.Range(0, powerUpPrefabs.Length);

                // Genera una posición aleatoria dentro de los límites definidos
                Vector2 randomSpawnPosition = new Vector2(
                    Random.Range(-width / 2, width / 2), // Posición X aleatoria
                    Random.Range(-height / 2, height / 2) // Posición Y aleatoria
                ) + (Vector2)transform.position; // Ajusta la posición a la posición del spawner

                // Instancia el power-up en la posición aleatoria
                Instantiate(powerUpPrefabs[randomPowerUpIndex], randomSpawnPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnDelay); // Espera el tiempo definido antes de generar el siguiente power-up
        }
    }

    private void OnDrawGizmos()
    {
        // Dibuja un cubo que representa el área de spawn en el editor
        Gizmos.color = Color.blue; // Color del Gizmo
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 1)); // Dibuja el área de spawn
    }
}
