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

            float progreso = Mathf.Clamp01(metros / metrosTope);
            float tiempoEspera = Mathf.Lerp(tiempoBaseEntreOleadas, tiempoMinimoEntreOleadas, progreso);

            yield return new WaitForSeconds(tiempoEspera);

            SpawnOleada();
        }
    }

    void SpawnOleada()
    {
        float metros = GameManager.Instance.metersRun;

        int carrilesOcupados = metros < 100f ? 1 : 2;

        List<float> carrilesDisponibles = new List<float>(carriles);
        MezclarLista(carrilesDisponibles);

        for (int i = 0; i < carrilesOcupados; i++)
        {
            float x = carrilesDisponibles[i];
            float z = jugador.position.z + distanciaSpawnDelante;

            Vector3 posicion = new Vector3(x, 0.5f, z);

            float tipoAleatorio = Random.value;

            if (tipoAleatorio < 0.5f)
            {
                SpawnEnemigo(posicion);
            }
            else if (tipoAleatorio < 0.75f && metros > 50f)
            {
                SpawnTrampa(posicion);
            }
            else if (metros > 150f)
            {
                SpawnObstaculo(posicion);
            }
            else
            {
                SpawnEnemigo(posicion);
            }
        }
    }

    void SpawnEnemigo(Vector3 posicion)
    {
        
        GameObject enemigo = Instantiate(prefabEnemigo, posicion, Quaternion.Euler(0f, 180f, 0f));
        enemigosVivos.Add(enemigo);
    }

    void SpawnTrampa(Vector3 posicion)
    {
        posicion.y = 0.01f;
        Instantiate(prefabTrampa, posicion, Quaternion.identity);
    }

    void SpawnObstaculo(Vector3 posicion)
    {
        posicion.y = 1.5f;
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