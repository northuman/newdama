using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/*
* Descripcion: Script para arrastrar las cartas con el ratón. 
* eventData : es la variable que guarda la información del evento, en este caso la carta.
* OnBeginDrag : se guarda el objeto parent actual para que si el destino no es válido vuelva a su origen
*               se desactivan los raycast del canvas para que no interfieran con el arrastrado
* OnDrag : va cambiando la posición de la carta
* OnEndDrag : se cambia el parent de la carta. Esto se hace a la vez que DropZone comprueba si el destino es válido.
*             Si es válido se habrá guardado el nuevo parent en parentToReturnTo, si no será el original y la carta volverá a su sitio.
*             Se vuelven a activar los raycast.
*/

public class Arrastrar : MonoBehaviour, IBeginDragHandler,IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    private Vector3 escalaOriginal;
    [Range(0.5f, 10f)]
    private float escalaOnDrag =2f;
    private float velocidadZoom = 0.1f;
    private Coroutine zoomCoroutine;
  
    /*
    Primero se comprueba si la carta pertenece al jugador.
    Se guarda la escala actual de la carta, se guarda el parent actual de la carta.
    Se cambia el parent al superior.
    Si no cumple la primera condicion se desactiva el script
    */
    public void OnBeginDrag(PointerEventData eventData)
    {   
        if(eventData.button == 0)
        {
            int perteneceA = eventData.pointerDrag.GetComponent<CartasJugadas>().perteneceAJugador;
            if(perteneceA == 1)
            {
                escalaOriginal = transform.localScale;    
                parentToReturnTo = transform.parent;
                transform.SetParent(transform.parent.parent);
                GetComponent<CanvasGroup>().blocksRaycasts = false;

                //zoom a la carta
                if(zoomCoroutine != null)StopCoroutine(zoomCoroutine);
                zoomCoroutine = StartCoroutine(EscalarCarta(escalaOriginal * escalaOnDrag));
            }
            else
            {
                enabled = false;
            }
        }
    }
    /*
    Se va cambiando la posicion de la carta conforme se arrastra.
    */
    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.button == 0)
        {
            transform.position = eventData.position;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(eventData.button == 0)
        {
            transform.SetParent(parentToReturnTo);
            GetComponent<CanvasGroup>().blocksRaycasts = true;

            // reiniciar escala
            if (zoomCoroutine != null)StopCoroutine(zoomCoroutine);
            zoomCoroutine = StartCoroutine(EscalarCarta(escalaOriginal));
        }
    }

    //corrutina para escalar la carta cuando la arrastras
    private IEnumerator EscalarCarta(Vector3 escala)
    {
        Debug.Log("Escalando de " + transform.localScale + " a " + escala);

        Vector3 inicio = transform.localScale;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / velocidadZoom;
            transform.localScale = Vector3.Lerp(inicio, escala, t);
            yield return null;
        }
    }
}
