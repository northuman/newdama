using UnityEngine;
using UnityEngine.EventSystems; // Necesario para detectar el ratón
using System.Collections;

// Estas interfaces (IPointer...) son las que detectan cuando el ratón entra y sale
public class TooltipFlavour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ventanaFlavour; // El objeto que vamos a encender/apagar
    public float tiempoEspera = 3.0f; // 3 segundos

    private Coroutine contador; // Para guardar la cuenta atrás

    // Se ejecuta al meter el ratón en la carta
    public void OnPointerEnter(PointerEventData eventData)
    {

        Debug.Log("¡He notado el ratón!");

        // Empezamos la cuenta atrás
        contador = StartCoroutine(EsperarYMostrar());
    }

    // Se ejecuta al sacar el ratón de la carta
    public void OnPointerExit(PointerEventData eventData)
    {
        // Si sacas el ratón, cancelamos la cuenta atrás y escondemos la ventana
        if (contador != null) StopCoroutine(contador);
        ventanaFlavour.SetActive(false);
    }

    // El reloj interno
    IEnumerator EsperarYMostrar()
    {
        yield return new WaitForSeconds(tiempoEspera);
        // Pasados los 3 segundos, encendemos la ventana
        if (ventanaFlavour != null)
        {
            ventanaFlavour.SetActive(true);
        }
    }


}