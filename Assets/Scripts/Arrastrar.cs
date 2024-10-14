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


    public void OnBeginDrag(PointerEventData eventData)
    {   
        GameObject j1 = GameObject.Find("Jugador");
        int idCarta = eventData.pointerDrag.gameObject.GetComponent<MostrarCarta>().id;
        
        parentToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        
        //if(j1.GetComponent<Jugador>().jugarCarta(buscarCartaenMano(idCarta, j1))){
        //}
    }

    public int buscarCartaenMano(int idCarta, GameObject jugador){
        for(int i = 0; i < jugador.GetComponent<Jugador>().mano.Count; i++){
            if(idCarta == jugador.GetComponent<Jugador>().mano[i].id){
                return i;
            }
        }
        return -1;
    }
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }



    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
