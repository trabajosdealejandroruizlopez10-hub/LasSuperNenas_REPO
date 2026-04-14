using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("Configuración")]
    public GameObject[] prefabsEnemigos;
    public float distanciaSpawnDelante = 25f;
    public int maxEnemigosSimultaneos = 10;

    [Header("Dificultad")]
    public float tiempoBaseEntreOleadas = 4f;

    private List<GameObject> enemigosVivos = new List<GameObject>();
    private Transform jugador;

    void Awake() { Instance = this; }

    void Start()
    {
        jugador = GameObject.FindWithTag("Player").transform;
        StartCoroutine(BucleDeSpawn());
    }

    IEnumerator BucleDeSpawn()
    {
        while (!GameManager.Instance.isGameOver)
        {
            float tiempoEspera = Mathf.Max(1f,
                tiempoBaseEntreOleadas - (GameManager.Instance.metersRun * 0.01f));
            yield return new WaitForSeconds(tiempoEspera);

            if (enemigosVivos.Count < maxEnemigosSimultaneos)
                SpawnOleada();
        }
    }

    void SpawnOleada()
    {
        int cantidad = Mathf.Min(
            3 + Mathf.FloorToInt(GameManager.Instance.metersRun / 50f),
            maxEnemigosSimultaneos
        );

        for (int i = 0; i < cantidad; i++)
        {
            float xAleatorio = Random.Range(-5f, 5f);
            Vector3 posSpawn = new Vector3(
                jugador.position.x + xAleatorio,
                jugador.position.y,
                jugador.position.z + distanciaSpawnDelante
            );

            int tipoEnemigo = Random.Range(0, prefabsEnemigos.Length);
            GameObject enemigo = Instantiate(prefabsEnemigos[tipoEnemigo], posSpawn, Quaternion.identity);
            enemigosVivos.Add(enemigo);
        }
    }

    public void RegistrarMuerte(GameObject enemigo)
    {
        enemigosVivos.Remove(enemigo);
    }
}
