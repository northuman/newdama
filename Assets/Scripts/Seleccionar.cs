using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Script para seleccionar una carta y guargar su información
public class Seleccionar : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    GameObject cartaSeleccionada = null;
    
    
    public void OnPointerDown (PointerEventData eventData)
    {}
    public void OnPointerUp (PointerEventData eventData) 
    {}
    public void OnPointerClick(PointerEventData eventData)
    {
        //Si se pulsa el click izquierdo
        if(eventData.button == 0){
            //Guardo el gameObject seleccionado
            cartaSeleccionada = eventData.pointerClick.gameObject;
            if(cartaSeleccionada.GetComponent<MostrarCarta>().tipo == "Tierra")
            {
               GameObject.Find("Jugador").gameObject.GetComponent<Jugador>().girarTierra(cartaSeleccionada); 
            }
        }
    }
    public void OnPointerEnter (PointerEventData eventData)
    {}
    public void OnPointerExit (PointerEventData eventData)
    {} 



    //
    void Start()
    {
        
    }
    //
    void Update()
    {
         
    }
}
