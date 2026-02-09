using UnityEngine;
using UnityEngine.EventSystems;

public class DobleClicCarta : MonoBehaviour, IPointerClickHandler
{
    private float ultimoClic = 0f;
    private float tiempoParaDobleClic = 0.3f; // Tiempo máximo entre clics (ajústalo si quieres)

    public void OnPointerClick(PointerEventData eventData)
    {
        // Si ha pasado muy poco tiempo desde el último clic... ¡es doble clic!
        if (Time.time - ultimoClic < tiempoParaDobleClic)
        {
            Debug.Log("¡Doble clic detectado!");
            MostrarElFlavour();
            ultimoClic = 0f; // Reseteamos para que no cuente como triple clic
        }
        else
        {
            // Es el primer clic de la secuencia
            ultimoClic = Time.time;
        }
    }

    void MostrarElFlavour()
    {
        // 1. Buscamos los datos de ESTA carta (están en el objeto padre)
        MostrarDatosCarta datos = GetComponentInParent<MostrarDatosCarta>();

        // 2. Si encontramos los datos y el flavour no está vacío...
        if (datos != null && datos.carta != null && !string.IsNullOrEmpty(datos.carta.flavour))
        {
            // 3. ...le decimos al Gestor que muestre el texto en el panel lateral
            GestorFlavour.instancia.MostrarFlavour(datos.carta.flavour);
        }
        else
        {
            Debug.LogWarning("No se pudo encontrar el flavour de esta carta.");
        }
    }
}