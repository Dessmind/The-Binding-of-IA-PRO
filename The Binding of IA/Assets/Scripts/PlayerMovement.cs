using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private GameObject projectilePrefab; // Prefab del proyectil
    [SerializeField] private float projectileSpeed = 5f; // Velocidad del proyectil
    [SerializeField] private float fireRate = 0.5f; // Intervalo de disparo en segundos
    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private Vector2 lastDirection; // �ltima direcci�n de movimiento
    private Animator playerAnimator;
    private bool isAlive = true;

    private bool canShootInEightDirections = false; // Estado del power-up
    private float nextFireTime = 0f; // Tiempo para el pr�ximo disparo
    private float originalProjectileSpeed; // Almacena la velocidad del proyectil original

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        lastDirection = Vector2.up; // Inicializa la direcci�n de disparo hacia arriba
        originalProjectileSpeed = projectileSpeed; // Guarda la velocidad del proyectil original
    }

    private void Update()
    {
        if (!isAlive) return; // Si no est� vivo, no procesar el movimiento

        // Obtener la entrada del usuario usando solo WASD
        float moveX = Input.GetAxisRaw("Horizontal"); // Este ser� controlado por A y D
        float moveY = Input.GetAxisRaw("Vertical"); // Este ser� controlado por W y S
        moveInput = new Vector2(moveX, moveY).normalized;

        // Actualizar la �ltima direcci�n de movimiento solo si el jugador se mueve
        if (moveInput != Vector2.zero)
        {
            lastDirection = moveInput; // Actualiza la �ltima direcci�n si el jugador se mueve
        }

        // Configurar los par�metros del Animator
        playerAnimator.SetFloat("Horizontal", moveX);
        playerAnimator.SetFloat("Vertical", moveY);
        playerAnimator.SetFloat("Speed", moveInput.magnitude);

        // Espejear el personaje seg�n la direcci�n del movimiento
        if (moveX > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1); // Mirar a la derecha
        }
        else if (moveX < -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Mirar a la izquierda
        }

        // Disparar solo con las teclas de flecha
        Vector2 shootDirection = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow)) shootDirection.y += 1;
        if (Input.GetKey(KeyCode.DownArrow)) shootDirection.y -= 1;
        if (Input.GetKey(KeyCode.LeftArrow)) shootDirection.x -= 1;
        if (Input.GetKey(KeyCode.RightArrow)) shootDirection.x += 1;

        // Si hay una direcci�n de disparo v�lida
        if (shootDirection != Vector2.zero && Time.time >= nextFireTime)
        {
            Shoot(shootDirection.normalized); // Disparar en la direcci�n indicada
            nextFireTime = Time.time + fireRate; // Actualiza el tiempo del pr�ximo disparo
        }
    }


    private void FixedUpdate()
    {
        if (!isAlive) return; // Si no est� vivo, no mover

        // Mover al jugador en funci�n de la entrada
        playerRb.velocity = moveInput * speed; // Cambia la velocidad directamente
    }

    // M�todo para disparar en la direcci�n proporcionada
    private void Shoot(Vector2 direction)
    {
        if (canShootInEightDirections)
        {
            // Dispara en 8 direcciones
            ShootInEightDirections();
        }
        else
        {
            // Instanciar el proyectil en la posici�n del jugador y en la direcci�n proporcionada
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            projectileRb.velocity = direction * projectileSpeed; // Asigna velocidad al proyectil
        }
    }

    // M�todo para disparar en 8 direcciones
    private void ShootInEightDirections()
    {
        // Dispara en 8 direcciones
        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45f; // Calcula el �ngulo para cada direcci�n
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)); // Calcula la direcci�n
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            projectileRb.velocity = direction * projectileSpeed; // Asigna velocidad al proyectil
        }
    }

    // M�todo para activar el power-up de disparo r�pido
    public void ActivateRapidFire(float duration, float multiplier)
    {
        fireRate /= multiplier; // Acelera la tasa de disparo
        projectileSpeed *= 2; // Aumenta la velocidad del proyectil
        Invoke("DeactivateRapidFire", duration); // Desactivar despu�s de un tiempo
    }

    // M�todo para desactivar el power-up de disparo r�pido
    private void DeactivateRapidFire()
    {
        fireRate *= 3; // Regresar a la tasa de disparo normal (asumiendo que el multiplicador era 2)
        projectileSpeed = originalProjectileSpeed; // Restablece la velocidad del proyectil original
    }

    // M�todo para activar el power-up
    public void ActivatePowerUp(float duration)
    {
        canShootInEightDirections = true; // Permitir disparar en 8 direcciones
        Invoke("DeactivatePowerUp", duration); // Desactivar despu�s de un tiempo
    }

    // M�todo para desactivar el power-up
    private void DeactivatePowerUp()
    {
        canShootInEightDirections = false; // Regresar al disparo normal
    }

    // M�todo para cambiar el estado del jugador
    public void SetAlive(bool alive)
    {
        isAlive = alive; // Establece el estado del jugador

        if (!isAlive)
        {
            playerRb.velocity = Vector2.zero; // Detiene el movimiento al morir
        }
    }

}
