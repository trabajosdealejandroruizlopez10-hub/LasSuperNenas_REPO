using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    public static TimerUI Instance;

    [Header("Texto del temporizador")]
    public TextMeshProUGUI textoTiempo;

    void Awake()
    {
        Instance = this;
    }

    public void ActualizarTiempo(float tiempo)
    {
        if (textoTiempo != null)
        {
            int segundos = Mathf.FloorToInt(tiempo);
            textoTiempo.text = segundos + "s";
        }
    }
}