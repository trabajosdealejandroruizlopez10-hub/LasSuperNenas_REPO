using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    [Header("Configuración de chunks")]
    public GameObject chunkPrefab;
    public int chunksVisibles = 5;
    public float chunkLongitud = 20f;

    private Queue<GameObject> chunksActivos = new Queue<GameObject>();
    private Transform jugador;
    private float zUltimoChunk = 0f;

    void Start()
    {
        jugador = GameObject.FindWithTag("Player").transform;

        for (int i = 0; i < chunksVisibles; i++)
            GenerarChunk();
    }

    void Update()
    {
        if (jugador == null) return;

        if (jugador.position.z + (chunksVisibles * chunkLongitud) > zUltimoChunk)
        {
            GenerarChunk();
            EliminarChunkAntiguo();
        }
    }

    void GenerarChunk()
    {
        Vector3 posicion = new Vector3(0f, 0f, zUltimoChunk);

        GameObject nuevoChunk = Instantiate(chunkPrefab, posicion, Quaternion.identity);
        chunksActivos.Enqueue(nuevoChunk);

        zUltimoChunk += chunkLongitud;
    }

    void EliminarChunkAntiguo()
    {
        if (chunksActivos.Count > chunksVisibles + 2)
        {
            GameObject viejo = chunksActivos.Dequeue();
            Destroy(viejo);
        }
    }
}