using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    public static AmmoUI Instance;

    [Header("Texto de munición")]
    public TextMeshProUGUI textoBalas;

    void Awake()
    {
        Instance = this;
    }

    public void ActualizarMunicion(int balasActuales, int balasMaximas)
    {
        if (textoBalas != null)
        {
            textoBalas.text = balasActuales + " / " + balasMaximas;
        }
    }
}