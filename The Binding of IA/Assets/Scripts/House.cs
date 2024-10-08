using UnityEngine;

public class House : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si la colisión es con una bala
        if (collision.CompareTag("Bullets") || collision.CompareTag("EnemyBullets"))
        {
            // Destruir la bala al chocar con la casa
            Destroy(collision.gameObject);
        }
    }
}
