using UnityEngine;
using TMPro;

public class GestorFlavour : MonoBehaviour
{
    public static GestorFlavour instancia;
    public TextMeshProUGUI textoDelPanel;

    void Awake()
    {
        // 1. Nos registramos como el gestor oficial
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject); // Si ya hay uno, nos destruimos para no duplicar
        }
    }

    void Start()
    {
        // 2. ¡TRUCO! Nos ocultamos nosotros mismos al arrancar el juego.
        // Como Awake() ya se ejecutó antes, la 'instancia' ya existe y es segura.
        gameObject.SetActive(false);
    }

    public void MostrarFlavour(string texto)
    {
        textoDelPanel.text = texto;
        gameObject.SetActive(true); // Nos encendemos para mostrar el mensaje
    }

    public void CerrarPanel()
    {
        gameObject.SetActive(false);
    }
}