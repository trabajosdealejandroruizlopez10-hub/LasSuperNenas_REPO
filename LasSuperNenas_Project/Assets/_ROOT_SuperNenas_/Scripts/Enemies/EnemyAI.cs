using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float velocidad = 3f;
    public int dañoAlJugador = 1;
    public float distanciaAtaque = 1.5f;

    private Transform jugador;

    void Start()
    {
        jugador = GameObject.FindWithTag("Player").transform;
        velocidad += GameManager.Instance.metersRun * 0.005f;
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver) return;
        if (jugador == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            jugador.position,
            velocidad * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, jugador.position) < distanciaAtaque)
        {
            GameManager.Instance.TakeDamage(dañoAlJugador);
            Destroy(gameObject);
        }
    }
}
