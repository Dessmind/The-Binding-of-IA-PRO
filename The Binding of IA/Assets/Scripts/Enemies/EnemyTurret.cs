using System.Collections;
using UnityEngine;

public class ShootAI : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float timeBetweenShoots = 1f;
    private bool playerDetected = false;
    private Coroutine shootingCoroutine;
    private Coroutine cooldownCoroutine;

    [Header("Field of View Settings")]
    [SerializeField] private float viewRadius = 5f;
    [SerializeField] private float viewAngle = 45f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Detection Settings")]
    [SerializeField] private float detectionCooldown = 3f;
    private Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    void Update()
    {
        CheckPlayerInFieldOfView();
    }

    private void CheckPlayerInFieldOfView()
    {
        // Comprueba si el jugador está dentro del radio de detección
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, playerLayer);

        bool playerInView = false;

        foreach (var target in targetsInViewRadius)
        {
            Vector2 directionToTarget = (target.transform.position - transform.position).normalized;
            float angleToTarget = Vector2.Angle(transform.right, directionToTarget);

            // Verifica si el jugador está dentro del ángulo de visión
            if (angleToTarget < viewAngle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);

                // Verifica que no haya obstáculos entre el enemigo y el jugador
                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleLayer))
                {
                    playerInView = true;

                    if (!playerDetected)
                    {
                        playerDetected = true;
                        if (shootingCoroutine == null)
                        {
                            shootingCoroutine = StartCoroutine(Shoot());
                        }
                    }

                    if (cooldownCoroutine != null)
                    {
                        StopCoroutine(cooldownCoroutine);
                        cooldownCoroutine = null;
                    }
                    return;
                }
            }
        }

        // Si el jugador está fuera de la vista, iniciar el cooldown
        if (playerDetected && !playerInView && cooldownCoroutine == null)
        {
            cooldownCoroutine = StartCoroutine(StopShootingAfterCooldown());
        }
    }

    private IEnumerator StopShootingAfterCooldown()
    {
        yield return new WaitForSeconds(detectionCooldown);

        // Si después del cooldown el jugador sigue fuera del campo de visión
        if (!playerDetected)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
        playerDetected = false;
    }

    private IEnumerator Shoot()
    {
        while (true) // Mantener el Coroutine activo indefinidamente
        {
            if (playerDetected)
            {
                Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(timeBetweenShoots);
            }
            else
            {
                yield return null; // Espera un frame para continuar el ciclo si no hay detección
            }
        }
    }

    // Dibujar el campo de visión en el editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 leftBoundary = DirectionFromAngle(-viewAngle / 2);
        Vector3 rightBoundary = DirectionFromAngle(viewAngle / 2);

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);
    }

    private Vector3 DirectionFromAngle(float angleInDegrees)
    {
        float angleInRadians = (transform.eulerAngles.z + angleInDegrees) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
    }
}
