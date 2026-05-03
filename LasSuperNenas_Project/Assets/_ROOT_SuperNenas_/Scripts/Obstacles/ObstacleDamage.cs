using UnityEngine;

public class ObstacleDamage : MonoBehaviour
{
    [Header("Daño")]
    public int daño = 1;

    [Header("Configuración")]
    public bool destruirAlImpactar = true;

    private bool yaGolpeo = false;

    void OnTriggerEnter(Collider other)
    {
        if (yaGolpeo) return;

        if (other.CompareTag("Player"))
        {
            yaGolpeo = true;

            // Hacer daño
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TakeDamage(daño);
            }

            // Destruir obstáculo
            if (destruirAlImpactar)
            {
                Destroy(gameObject);
            }
        }
    }
}
