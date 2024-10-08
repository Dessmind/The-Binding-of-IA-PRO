using UnityEngine;

public class TankEnemy : Enemy
{
    [SerializeField] private float maxSpeed = 6.0f;
    [SerializeField] private float acceleration = 0.5f;
    [SerializeField] private float evadeDistance = 3.0f;
    [SerializeField] private float evadeForce = 10.0f;

    private float currentSpeed = 0.0f;
    private Transform player;
    private Rigidbody2D rb;
    private int baseDamage = 1;
    private Animator animator;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void FixedUpdate()
    {
        // Solo se mueve si el enemigo está vivo y el jugador también lo está
        if (isAlive && playerHealth != null && playerHealth.IsAlive())
        {
            Vector2 direction = (player.position - transform.position).normalized;
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime);

            int obstaclesLayer = LayerMask.GetMask("Obstacles");
            RaycastHit2D hitForward = Physics2D.Raycast(transform.position, direction, evadeDistance, obstaclesLayer);
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, 90) * direction, evadeDistance, obstaclesLayer);
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, -90) * direction, evadeDistance, obstaclesLayer);

            if (hitForward.collider != null || hitLeft.collider != null || hitRight.collider != null)
            {
                Evade(hitForward, hitLeft, hitRight, direction);
            }
            else
            {
                rb.velocity = direction * currentSpeed;
                UpdateAnimations(direction);
            }

            FlipTank();
        }
        else
        {
            rb.velocity = Vector2.zero;
            currentSpeed = 0.0f;
            animator.SetFloat("Speed", 0.0f);
        }
    }

    private void UpdateAnimations(Vector2 direction)
    {
        animator.SetFloat("Speed", currentSpeed);
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
    }

    private void Evade(RaycastHit2D hitForward, RaycastHit2D hitLeft, RaycastHit2D hitRight, Vector2 direction)
    {
        if (hitForward.collider != null)
        {
            if (hitLeft.collider == null)
            {
                rb.AddForce(Vector2.Perpendicular(direction) * evadeForce, ForceMode2D.Impulse);
            }
            else if (hitRight.collider == null)
            {
                rb.AddForce(-Vector2.Perpendicular(direction) * evadeForce, ForceMode2D.Impulse);
            }
            else
            {
                currentSpeed = 0.0f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                int impactDamage = baseDamage;

                if (currentSpeed >= 10.0f) impactDamage = 3;
                else if (currentSpeed >= 5f) impactDamage = 2;

                if (impactDamage > 0) playerHealth.TakeDamage(impactDamage);
            }

            currentSpeed = 0.0f;
        }

        if (collision.gameObject.CompareTag("Obstacles"))
        {
            currentSpeed = 0.0f;
            baseDamage = 1;
        }
    }

    private void FlipTank()
    {
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-3, 3, 3);
        else
            transform.localScale = new Vector3(3, 3, 3);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, evadeDistance);

        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction * evadeDistance);

            Vector2 leftDirection = Quaternion.Euler(0, 0, 90) * direction;
            Vector2 rightDirection = Quaternion.Euler(0, 0, -90) * direction;

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)leftDirection * evadeDistance);
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)rightDirection * evadeDistance);
        }
    }
}
