using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("Prefabs")]
    public GameObject prefabEnemigo;
    public GameObject prefabTrampa;
    public GameObject prefabObstaculo;

    [Header("Configuración de spawn")]
    public float distanciaSpawnDelante = 20f;

    [Header("Dificultad")]
    public float tiempoBaseEntreOleadas = 3f;
    public float tiempoMinimoEntreOleadas = 0.8f;
    public float metrosTope = 500f;

    private List<GameObject> enemigosVivos = new List<GameObject>();
    private Transform jugador;

    // Los tres carriles del pasillo
    private float[] carriles = { -3f, 0f, 3f };

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        jugador = GameObject.FindWithTag("Player").transform;
        StartCoroutine(BucleDeSpawn());
    }

    IEnumerator BucleDeSpawn()
    {
        while (!GameManager.Instance.isGameOver)
        {
            float metros = GameManager.Instance.metersRun;

            // Calcula el tiempo entre oleadas según metros, con tope
            float progreso = Mathf.Clamp01(metros / metrosTope);
            float tiempoEspera = Mathf.Lerp(tiempoBaseEntreOleadas, tiempoMinimoEntreOleadas, progreso);

            yield return new WaitForSeconds(tiempoEspera);

            SpawnOleada();
        }
    }

    void SpawnOleada()
    {
        float metros = GameManager.Instance.metersRun;
        float progreso = Mathf.Clamp01(metros / metrosTope);

        // Cuántos carriles ocupar (nunca los 3 a la vez)
        int carrilesOcupados = metros < 100f ? 1 : (metros < 300f ? 2 : 2);

        // Mezcla los carriles aleatoriamente
        List<float> carrilesDisponibles = new List<float>(carriles);
        MezclarLista(carrilesDisponibles);

        // Elige qué tipo de objeto spawnear según progreso
        for (int i = 0; i < carrilesOcupados; i++)
        {
            float x = carrilesDisponibles[i];
            float z = jugador.position.z + distanciaSpawnDelante;

            Vector3 posicion = new Vector3(x, 0.5f, z);

            float tipoAleatorio = Random.value;

            if (tipoAleatorio < 0.5f)
            {
                // Enemigo — aparece desde el principio
                SpawnEnemigo(posicion);
            }
            else if (tipoAleatorio < 0.75f && metros > 50f)
            {
                // Trampa — aparece a partir de 50 metros
                SpawnTrampa(posicion);
            }
            else if (metros > 150f)
            {
                // Obstáculo — aparece a partir de 150 metros
                SpawnObstaculo(posicion);
            }
            else
            {
                // Si no toca obstáculo todavía, spawnea enemigo
                SpawnEnemigo(posicion);
            }
        }
    }

    void SpawnEnemigo(Vector3 posicion)
    {
        GameObject enemigo = Instantiate(prefabEnemigo, posicion, Quaternion.identity);
        enemigosVivos.Add(enemigo);
    }

    void SpawnTrampa(Vector3 posicion)
    {
        posicion.y = 0.01f; // Ras del suelo
        Instantiate(prefabTrampa, posicion, Quaternion.identity);
    }

    void SpawnObstaculo(Vector3 posicion)
    {
        posicion.y = 1.5f; // Centrado verticalmente
        Instantiate(prefabObstaculo, posicion, Quaternion.identity);
    }

    public void RegistrarMuerte(GameObject enemigo)
    {
        enemigosVivos.Remove(enemigo);
    }

    void MezclarLista(List<float> lista)
    {
        for (int i = lista.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            float temp = lista[i];
            lista[i] = lista[j];
            lista[j] = temp;
        }
    }
}