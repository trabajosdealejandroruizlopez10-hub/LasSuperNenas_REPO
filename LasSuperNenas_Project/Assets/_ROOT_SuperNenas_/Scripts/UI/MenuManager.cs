using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleMenu : MonoBehaviour
{
    public GameObject panelModo;
    public GameObject botonesMenu;

    public void MostrarPanel()
    {
        panelModo.SetActive(true);
        botonesMenu.SetActive(false);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del trauma yay!");
        Application.Quit();
    }
}