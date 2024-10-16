using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs; // Prefabs de enemigos que se pueden generar
    [SerializeField] private float spawnDelay = 2f; // Intervalo de tiempo para generar nuevos enemigos
    [SerializeField] private float minSpawnDistance = 3f; // Distancia mínima desde el jugador para spawnear enemigos

    // Define las áreas de spawn
    [System.Serializable]
    public struct SpawnArea
    {
        public Vector2 position; // Posición del centro del área
        public float width; // Ancho del área
        public float height; // Alto del área
    }

    [SerializeField] private SpawnArea[] spawnAreas; // Arreglo de áreas de spawn
    private PlayerHealth playerHealth; // Referencia al script de salud del jugador
    private Transform player; // Referencia al Transform del jugador

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>(); // Busca el script de salud del jugador
        player = FindObjectOfType<PlayerMovement>().transform; // Busca el transform del jugador
        StartCoroutine(SpawnEnemies()); // Inicia la corutina para spawn de enemigos
    }

    private IEnumerator SpawnEnemies()
    {
        // Espera el tiempo de spawn inicial antes de comenzar a generar enemigos
        yield return new WaitForSeconds(spawnDelay);

        while (true)
        {
            // Verifica si el jugador está vivo
            if (playerHealth.GetCurrentHealth() > 0)
            {
                // Selecciona un prefab de enemigo aleatorio
                int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);

                // Selecciona un área de spawn aleatoria
                SpawnArea selectedArea = spawnAreas[Random.Range(0, spawnAreas.Length)];

                // Genera una posición aleatoria dentro de los límites del área seleccionada
                Vector2 randomSpawnPosition;
                const int maxAttempts = 100; // Máximo de intentos para encontrar una posición válida
                int attempts = 0; // Contador de intentos
                bool spawnPositionFound = false; // Flag para indicar si se encontró una posición válida

                // Repite la generación de la posición hasta que esté lejos del jugador o se agoten los intentos
                do
                {
                    randomSpawnPosition = new Vector2(
                        Random.Range(selectedArea.position.x - selectedArea.width / 2, selectedArea.position.x + selectedArea.width / 2), // Posición X aleatoria
                        Random.Range(selectedArea.position.y - selectedArea.height / 2, selectedArea.position.y + selectedArea.height / 2) // Posición Y aleatoria
                    );

                    // Verifica si la distancia al jugador es suficiente
                    if (Vector2.Distance(randomSpawnPosition, player.position) >= minSpawnDistance)
                    {
                        spawnPositionFound = true; // Se encontró una posición válida
                    }

                    attempts++;
                } while (!spawnPositionFound && attempts < maxAttempts); // Continúa hasta encontrar una posición o agotar intentos

                // Si no se encontró una posición válida, genera en cualquier área
                if (!spawnPositionFound)
                {
                    // Selecciona un área de spawn aleatoria sin verificar la distancia al jugador
                    selectedArea = spawnAreas[Random.Range(0, spawnAreas.Length)];

                    randomSpawnPosition = new Vector2(
                        Random.Range(selectedArea.position.x - selectedArea.width / 2, selectedArea.position.x + selectedArea.width / 2), // Posición X aleatoria
                        Random.Range(selectedArea.position.y - selectedArea.height / 2, selectedArea.position.y + selectedArea.height / 2) // Posición Y aleatoria
                    );
                }

                // Instancia el enemigo en la posición aleatoria
                Instantiate(enemyPrefabs[randomEnemyIndex], randomSpawnPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnDelay); // Espera el tiempo definido antes de generar el siguiente enemigo
        }
    }

    private void OnDrawGizmos()
    {
        // Dibuja las áreas de spawn en el editor
        Gizmos.color = Color.red; // Color del Gizmo
        foreach (SpawnArea area in spawnAreas)
        {
            Gizmos.DrawWireCube(area.position, new Vector3(area.width, area.height, 1)); // Dibuja el área de spawn
        }
    }
}
