using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Acciones de juego reutilizables para botones de UI.
 * No gestiona estado de pantallas — eso es responsabilidad de MenuInicio.
 */
public class accionBotones : MonoBehaviour
{
    [SerializeField] private Transicion transicion;

    public void CargarEscena(string nombreEscena)
    {
        if (transicion != null)
            transicion.LoadScene(nombreEscena);
        else
            SceneManager.LoadScene(nombreEscena);
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void AbrirEnlace(string url)
    {
        if (!string.IsNullOrEmpty(url))
            Application.OpenURL(url);
    }
}
