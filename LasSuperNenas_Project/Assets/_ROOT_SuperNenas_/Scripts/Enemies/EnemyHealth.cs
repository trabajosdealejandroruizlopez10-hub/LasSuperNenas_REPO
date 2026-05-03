using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Vida")]
    public int vida = 2;

    [Header("VFX al morir")]
    public GameObject vfxMuerte;
    public float tiempoDestruirVFX = 2f;

    public void TakeDamage(int daño)
    {
        vida -= daño;

        if (vida <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        
        if (vfxMuerte != null)
        {
            GameObject vfx = Instantiate(vfxMuerte, transform.position, Quaternion.identity);

            
            Destroy(vfx, tiempoDestruirVFX);
        }

        
        if (EnemySpawner.Instance != null)
        {
            EnemySpawner.Instance.RegistrarMuerte(gameObject);
        }

        
        Destroy(gameObject);
    }
}