using UnityEngine;

public class TankEnemy : Enemy
{
    [SerializeField] private float maxSpeed = 6.0f; // Velocidad m�xima del tanque
    [SerializeField] private float acceleration = 0.5f; // Aceleraci�n del tanque
    [SerializeField] private float evadeDistance = 3.0f; // Distancia para detectar obst�culos
    [SerializeField] private float evadeForce = 10.0f; // Fuerza de evasi�n

    private float currentSpeed = 0.0f; // Velocidad actual
    private Transform player; // Referencia al jugador
    private Rigidbody2D rb; // Referencia al Rigidbody2D
    private int baseDamage = 1; // Da�o base que se restablecer�

    // Referencia al Animator
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>(); // Obtener la referencia al Animator
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            // Movimiento hacia el jugador
            Vector2 direction = (player.position - transform.position).normalized;

            // Aumenta gradualmente la velocidad hasta llegar a la m�xima
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime);

            // Detecta si hay obst�culos en la direcci�n del movimiento
            RaycastHit2D hitForward = Physics2D.Raycast(transform.position, direction, evadeDistance, LayerMask.GetMask("Walls"));
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, 90) * direction, evadeDistance, LayerMask.GetMask("Walls"));
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, -90) * direction, evadeDistance, LayerMask.GetMask("Walls"));

            if (hitForward.collider != null || hitLeft.collider != null || hitRight.collider != null)
            {
                Evade(hitForward, hitLeft, hitRight, direction); // Llama al m�todo de evasi�n
            }
            else
            {
                // Aplica el movimiento al Rigidbody2D
                rb.velocity = direction * currentSpeed;

                // Actualiza las animaciones
                UpdateAnimations(direction);
            }

            // Espejear el tanque seg�n la posici�n del jugador
            FlipTank();
        }
    }

    private void UpdateAnimations(Vector2 direction)
    {
        // Actualiza el par�metro Speed
        animator.SetFloat("Speed", currentSpeed); // Asigna la velocidad actual

        // Actualiza los par�metros de movimiento
        animator.SetFloat("Horizontal", direction.x); // Asigna la direcci�n horizontal
        animator.SetFloat("Vertical", direction.y); // Asigna la direcci�n vertical
    }

    private void Evade(RaycastHit2D hitForward, RaycastHit2D hitLeft, RaycastHit2D hitRight, Vector2 direction)
    {
        // Si hay un obst�culo frente
        if (hitForward.collider != null)
        {
            // Eval�a si hay espacio a la izquierda o a la derecha
            if (hitLeft.collider == null)
            {
                // Evadir a la izquierda
                rb.AddForce(Vector2.Perpendicular(direction) * evadeForce, ForceMode2D.Impulse);
            }
            else if (hitRight.collider == null)
            {
                // Evadir a la derecha
                rb.AddForce(-Vector2.Perpendicular(direction) * evadeForce, ForceMode2D.Impulse);
            }
            else
            {
                // Si no hay espacio a la izquierda o derecha, el tanque se detiene
                currentSpeed = 0.0f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si choca con el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Aplica da�o basado en la velocidad actual
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                int impactDamage = baseDamage; // Inicia con el da�o base

                // Determina el da�o seg�n la velocidad
                if (currentSpeed >= 6.0f)
                {
                    impactDamage = 3; // Da�o m�ximo
                }
                else if (currentSpeed >= 2.0f)
                {
                    impactDamage = 2; // Da�o medio
                }

                if (impactDamage > 0)
                {
                    playerHealth.TakeDamage(impactDamage);
                }
            }

            // Reinicia la velocidad del tanque al chocar con el jugador
            currentSpeed = 0.0f;
        }

        // Reinicia la velocidad al chocar con paredes
        if (collision.gameObject.CompareTag("Walls"))
        {
            currentSpeed = 0.0f; // Reinicia la velocidad
            // Restablece el da�o a 1 cuando el tanque choca con un obst�culo
            baseDamage = 1;
        }
    }

    private void FlipTank()
    {
        // Espejear el tanque seg�n la posici�n del jugador
        if (player.position.x < transform.position.x)
        {
            // Si el jugador est� a la izquierda del tanque, voltearlo
            transform.localScale = new Vector3(-2, 2, 2);
        }
        else
        {
            // Si el jugador est� a la derecha del tanque, devolver la escala a la original
            transform.localScale = new Vector3(2, 2, 2);
        }
    }
}
