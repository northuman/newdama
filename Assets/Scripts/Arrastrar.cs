using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
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
  
    /*
    Primero se comprueba si la carta pertenece al jugador.
    Se guarda la escala actual de la carta, se guarda el parent actual de la carta.
    Se cambia el parent al superior.
    Si no cumple la primera condicion se desactiva el script
    */
    public void OnBeginDrag(PointerEventData eventData)
    {   
        if(eventData.button == 0){
            GameObject j1 = GameObject.Find("Jugador");
            int perteneceA = eventData.pointerDrag.gameObject.GetComponent<CartasJugadas>().perteneceAJugador;
            if(perteneceA == 1){
                int idCarta = eventData.pointerDrag.gameObject.GetComponent<MostrarCarta>().id;
                
                parentToReturnTo = this.transform.parent;
                this.transform.SetParent(this.transform.parent.parent);
                GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            else{
                this.enabled = false;
            }
        }
            
    }

    /*
    Se va cambiando la posicion de la carta conforme se arrastra.
    */
    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.button == 0){
            this.transform.position = eventData.position;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(eventData.button == 0){
            this.transform.SetParent(parentToReturnTo);
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
