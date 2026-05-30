using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 2;
    public GameObject healthPickupPrefab;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (healthPickupPrefab != null)
        {
            Instantiate(healthPickupPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.TakeDamage(1);
        }
    }
}