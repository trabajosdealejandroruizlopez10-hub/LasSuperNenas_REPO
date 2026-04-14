using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int vida = 2;

    public void RecibirDaño(int cantidad)
    {
        vida -= cantidad;
        if (vida <= 0) Morir();
    }

    void Morir()
    {
        if (EnemySpawner.Instance != null)
            EnemySpawner.Instance.RegistrarMuerte(this.gameObject);
        Destroy(gameObject);
    }
}
