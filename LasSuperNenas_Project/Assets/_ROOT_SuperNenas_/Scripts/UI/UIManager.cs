using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Referencias UI")]
    public TextMeshProUGUI textoMetros;
    public TextMeshProUGUI textoVida;

    public GameObject panelGameOver;
    public GameObject panelVictoria;

    public TextMeshProUGUI textoMetrosFinales;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        if (textoMetros != null)
        {
            textoMetros.text = "Metros: " + Mathf.FloorToInt(GameManager.Instance.metersRun);
        }

        if (textoVida != null)
        {
            textoVida.text = "Vida: " + GameManager.Instance.playerHealth;
        }
    }

    public void MostrarGameOver()
    {
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
        }

        if (textoMetrosFinales != null)
        {
            textoMetrosFinales.text = "Llegaste a: " +
                Mathf.FloorToInt(GameManager.Instance.metersRun) + " metros";
        }
    }

    public void MostrarVictoria()
    {
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
        }
    }
}