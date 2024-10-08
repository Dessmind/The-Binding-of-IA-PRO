using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUpPrefabs; // Prefabs de power-ups que se pueden generar
    [SerializeField] private float spawnDelay = 2f; // Intervalo de tiempo para generar nuevos power-ups
    [SerializeField] private float width, height; // Dimensiones del �rea de spawn
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
            // Verifica si el jugador est� vivo
            if (playerHealth.GetCurrentHealth() > 0)
            {
                // Selecciona un prefab de power-up aleatorio
                int randomPowerUpIndex = Random.Range(0, powerUpPrefabs.Length);

                // Genera una posici�n aleatoria dentro de los l�mites definidos
                Vector2 randomSpawnPosition = new Vector2(
                    Random.Range(-width / 2, width / 2), // Posici�n X aleatoria
                    Random.Range(-height / 2, height / 2) // Posici�n Y aleatoria
                ) + (Vector2)transform.position; // Ajusta la posici�n a la posici�n del spawner

                // Instancia el power-up en la posici�n aleatoria
                Instantiate(powerUpPrefabs[randomPowerUpIndex], randomSpawnPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnDelay); // Espera el tiempo definido antes de generar el siguiente power-up
        }
    }

    private void OnDrawGizmos()
    {
        // Dibuja un cubo que representa el �rea de spawn en el editor
        Gizmos.color = Color.blue; // Color del Gizmo
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 1)); // Dibuja el �rea de spawn
    }
}
