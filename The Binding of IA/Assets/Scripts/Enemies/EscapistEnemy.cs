using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapistEnemy : Enemy
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float evadeRange = 2.0f;
    [SerializeField] private float evadeForce = 5.0f;
    [SerializeField] private float arrivalThreshold = 0.1f;
    [SerializeField] private List<Transform> movementPoints;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 5.0f;
    [SerializeField] private float shootingInterval = 2.0f;
    [SerializeField] private float predictionTime = 1.0f;

    private Transform player;
    private Rigidbody2D rb;
    private Transform currentTarget;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        AssignInitialTarget();
        StartCoroutine(ShootAtPlayer());
        StartCoroutine(PeriodicMove());
    }

    private void FixedUpdate()
    {
        Vector2 evadeDirection = CheckForObstacles();
        if (evadeDirection != Vector2.zero)
        {
            // Aplica la fuerza de esquive
            rb.velocity = evadeDirection * evadeForce;
        }
        else
        {
            MoveTowardsTarget(moveSpeed);
        }
    }

    private void AssignInitialTarget()
    {
        if (movementPoints.Count > 0)
        {
            currentTarget = movementPoints[Random.Range(0, movementPoints.Count)];
            transform.position = currentTarget.position;
        }
    }

    private IEnumerator ShootAtPlayer()
    {
        while (true)
        {
            // Siempre intenta disparar al jugador
            Vector2 playerVelocity = player.GetComponent<Rigidbody2D>().velocity;
            Vector2 playerFuturePosition = (Vector2)player.position + playerVelocity * predictionTime;

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

            Vector2 direction = (playerFuturePosition - (Vector2)transform.position).normalized;
            projectileRb.velocity = direction * projectileSpeed;

            yield return new WaitForSeconds(shootingInterval);
        }
    }

    private Transform FindFarthestPoint()
    {
        Transform farthestPoint = movementPoints[0];
        float maxDistance = Vector2.Distance(player.position, farthestPoint.position);

        foreach (var point in movementPoints)
        {
            float distance = Vector2.Distance(player.position, point.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestPoint = point;
            }
        }
        return farthestPoint;
    }

    private void MoveTowardsTarget(float speed)
    {
        if (currentTarget == null) return;

        if (Vector2.Distance(transform.position, currentTarget.position) > arrivalThreshold)
        {
            Vector2 direction = (currentTarget.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
        else
        {
            StopMovement();
        }
    }

    private Vector2 CheckForObstacles()
    {
        Collider2D[] obstacles = Physics2D.OverlapCircleAll(transform.position, evadeRange);

        foreach (var obstacle in obstacles)
        {
            if (obstacle.CompareTag("Obstacles"))
            {
                Vector2 directionAwayFromObstacle = (transform.position - obstacle.transform.position).normalized;
                return directionAwayFromObstacle;
            }
        }

        return Vector2.zero;
    }

    private void StopMovement()
    {
        rb.velocity = Vector2.zero;
    }

    private IEnumerator PeriodicMove()
    {
        while (true)
        {
            currentTarget = FindFarthestPoint();
            yield return new WaitForSeconds(3f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isAlive)
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Aplicar daño al jugador
                TakeDamage(1); // Aplicar daño al enemigo
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, evadeRange);
    }
}
