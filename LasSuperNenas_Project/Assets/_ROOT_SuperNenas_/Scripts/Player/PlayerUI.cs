using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance;

    [Header("Barra de vida")]
    public Image barraVida; // Image tipo Filled
    public float velocidadCambio = 5f;

    [Header("Efecto daño")]
    public Image pantallaRoja;
    public float duracionFlash = 0.3f;

    private float vidaActualVisual = 1f;

    void Awake()
    {
        Instance = this;
    }

    public void ActualizarVida(float vidaNormalizada)
    {
        StopAllCoroutines();
        StartCoroutine(AnimarBarra(vidaNormalizada));
    }

    IEnumerator AnimarBarra(float objetivo)
    {
        float inicio = vidaActualVisual;
        float tiempo = 0f;

        while (tiempo < 1f)
        {
            tiempo += Time.deltaTime * velocidadCambio;
            vidaActualVisual = Mathf.Lerp(inicio, objetivo, tiempo);

            if (barraVida != null)
                barraVida.fillAmount = vidaActualVisual;

            yield return null;
        }

        vidaActualVisual = objetivo;
    }

    public void MostrarDaño()
    {
        if (pantallaRoja != null)
            StartCoroutine(FlashDaño());
    }

    IEnumerator FlashDaño()
    {
        pantallaRoja.color = new Color(1, 0, 0, 0.5f);

        yield return new WaitForSeconds(duracionFlash);

        pantallaRoja.color = new Color(1, 0, 0, 0f);
    }
}