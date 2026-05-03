using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleMenu : MonoBehaviour
{
    public void IrAEscena1()
    {
        SceneManager.LoadScene(1);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del trauma");
        Application.Quit();
    }
}
