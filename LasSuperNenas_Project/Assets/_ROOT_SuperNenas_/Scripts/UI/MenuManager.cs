using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimpleMenu : MonoBehaviour
{
    public GameObject panelModo;
    public Button botonJugar;
    public Button botonSalir;
    public Button botonOpcines;
    public void MostrarPanel()
    {
        panelModo.SetActive(true);
        botonJugar.interactable = false;
        botonSalir.interactable = false;
        botonOpcines.interactable = false;

    }

    public void CerrarPanel()
    {
        panelModo.SetActive(false);
    }

    public void JugarEscena1()
    {
        Debug.Log("Escena 1");
        SceneManager.LoadScene(1);
    }

    public void JugarEscena2()
    {
        Debug.Log("Escena 2");
        SceneManager.LoadScene(2);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del trauma");
        Application.Quit();
    }
}