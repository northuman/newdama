using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * Controla la navegación del menú de inicio.
 * Gestiona el cambio entre PanelInicio y PanelOpciones dentro del mismo Canvas.
 * Los botones y paneles se asignan desde el inspector.
 */
public class MenuInicio : MonoBehaviour
{
    [SerializeField] private Transicion transicion;
    [SerializeField] private RectTransform contenedorBotones;
    [SerializeField] private GameObject panelOpciones;

    private const string EscenaCampania = "StoryScene";
    private const string EscenaArena = "Juego";

    private void Start()
    {
        ConfigurarBotones();
    }

    private void ConfigurarBotones()
    {
        if (contenedorBotones == null)
        {
            Debug.LogError("[MenuInicio] contenedorBotones no asignado en el inspector.");
            return;
        }

        AsignarBoton("Campania", IrACampania);
        AsignarBoton("Arena", IrAArena);
        AsignarBoton("Opciones", AbrirOpciones);
        AsignarBoton("Salir", Salir);
    }

    private void AsignarBoton(string nombre, UnityAction accion)
    {
        Button boton = contenedorBotones.Find(nombre)?.GetComponent<Button>();
        if (boton == null)
        {
            Debug.LogWarning($"[MenuInicio] Botón '{nombre}' no encontrado en {contenedorBotones.name}.");
            return;
        }

        boton.onClick.RemoveAllListeners();
        boton.onClick.AddListener(accion);
    }

    private void IrACampania()
    {
        if (transicion != null)
            transicion.LoadScene(EscenaCampania);
        else
            SceneManager.LoadScene(EscenaCampania);
    }

    private void IrAArena()
    {
        if (transicion != null)
            transicion.LoadScene(EscenaArena);
        else
            SceneManager.LoadScene(EscenaArena);
    }

    private void AbrirOpciones()
    {
        contenedorBotones.gameObject.SetActive(false);
        if (panelOpciones != null)
            panelOpciones.SetActive(true);
        else
            Debug.LogWarning("[MenuInicio] panelOpciones no asignado en el inspector.");
    }

    public void CerrarOpciones()
    {
        if (panelOpciones != null)
            panelOpciones.SetActive(false);
        contenedorBotones.gameObject.SetActive(true);
    }

    private void Salir()
    {
        Application.Quit();
    }

    // Llamado desde eventos persistentes del inspector (botones de Atribucion)
    public void AbrirEnlace(string url)
    {
        if (!string.IsNullOrEmpty(url))
            Application.OpenURL(url);
    }
}
