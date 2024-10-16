using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs; // Prefabs de enemigos que se pueden generar
    [SerializeField] private float spawnDelay = 2f; // Intervalo de tiempo para generar nuevos enemigos
    [SerializeField] private float minSpawnDistance = 3f; // Distancia m�nima desde el jugador para spawnear enemigos

    // Define las �reas de spawn
    [System.Serializable]
    public struct SpawnArea
    {
        public Vector2 position; // Posici�n del centro del �rea
        public float width; // Ancho del �rea
        public float height; // Alto del �rea
    }

    [SerializeField] private SpawnArea[] spawnAreas; // Arreglo de �reas de spawn
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
            // Verifica si el jugador est� vivo
            if (playerHealth.GetCurrentHealth() > 0)
            {
                // Selecciona un prefab de enemigo aleatorio
                int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);

                // Selecciona un �rea de spawn aleatoria
                SpawnArea selectedArea = spawnAreas[Random.Range(0, spawnAreas.Length)];

                // Genera una posici�n aleatoria dentro de los l�mites del �rea seleccionada
                Vector2 randomSpawnPosition;
                const int maxAttempts = 100; // M�ximo de intentos para encontrar una posici�n v�lida
                int attempts = 0; // Contador de intentos
                bool spawnPositionFound = false; // Flag para indicar si se encontr� una posici�n v�lida

                // Repite la generaci�n de la posici�n hasta que est� lejos del jugador o se agoten los intentos
                do
                {
                    randomSpawnPosition = new Vector2(
                        Random.Range(selectedArea.position.x - selectedArea.width / 2, selectedArea.position.x + selectedArea.width / 2), // Posici�n X aleatoria
                        Random.Range(selectedArea.position.y - selectedArea.height / 2, selectedArea.position.y + selectedArea.height / 2) // Posici�n Y aleatoria
                    );

                    // Verifica si la distancia al jugador es suficiente
                    if (Vector2.Distance(randomSpawnPosition, player.position) >= minSpawnDistance)
                    {
                        spawnPositionFound = true; // Se encontr� una posici�n v�lida
                    }

                    attempts++;
                } while (!spawnPositionFound && attempts < maxAttempts); // Contin�a hasta encontrar una posici�n o agotar intentos

                // Si no se encontr� una posici�n v�lida, genera en cualquier �rea
                if (!spawnPositionFound)
                {
                    // Selecciona un �rea de spawn aleatoria sin verificar la distancia al jugador
                    selectedArea = spawnAreas[Random.Range(0, spawnAreas.Length)];

                    randomSpawnPosition = new Vector2(
                        Random.Range(selectedArea.position.x - selectedArea.width / 2, selectedArea.position.x + selectedArea.width / 2), // Posici�n X aleatoria
                        Random.Range(selectedArea.position.y - selectedArea.height / 2, selectedArea.position.y + selectedArea.height / 2) // Posici�n Y aleatoria
                    );
                }

                // Instancia el enemigo en la posici�n aleatoria
                Instantiate(enemyPrefabs[randomEnemyIndex], randomSpawnPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnDelay); // Espera el tiempo definido antes de generar el siguiente enemigo
        }
    }

    private void OnDrawGizmos()
    {
        // Dibuja las �reas de spawn en el editor
        Gizmos.color = Color.red; // Color del Gizmo
        foreach (SpawnArea area in spawnAreas)
        {
            Gizmos.DrawWireCube(area.position, new Vector3(area.width, area.height, 1)); // Dibuja el �rea de spawn
        }
    }
}
