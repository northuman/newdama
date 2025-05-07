using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Script para seleccionar una carta y guargar su información
public class Seleccionar : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    GameObject cartaSeleccionada = null;
    public GameObject panelFront;
    GameObject copiaDeCarta;
    public Transform parentToReturnTo = null;
    Vector3 nuevaEscala;
    bool rightClick = false;
    

    public void OnPointerDown (PointerEventData eventData)
    {
        //click derecho
        if(eventData.button == PointerEventData.InputButton.Right){
            if(this.gameObject.GetComponent<MostrarCarta>().enabled){

                cartaSeleccionada = this.gameObject;
                rightClick = true;
                parentToReturnTo = this.transform.parent;

                EnsenyarCarta(cartaSeleccionada, true);
            }
        }
    }
    public void OnPointerUp (PointerEventData eventData) 
    {
        if (cartaSeleccionada != null){
            if(rightClick){
                EnsenyarCarta(cartaSeleccionada, false);
            }
        }
    }   
    public void OnPointerClick(PointerEventData eventData)
    {
        //Si se pulsa el click izquierdo
        if(eventData.button == 0){
            //Guardo el gameObject seleccionado
            cartaSeleccionada = eventData.pointerClick;
            if(cartaSeleccionada.GetComponent<MostrarCarta>().tipo == "Tierra")
            {
                //Si es una tierra se le da la vuelta
               GameObject.Find("Jugador").gameObject.GetComponent<Jugador>().GirarTierra(cartaSeleccionada); 
            }
        }
    }
    public void OnPointerEnter (PointerEventData eventData){}
    public void OnPointerExit (PointerEventData eventData){} 

    public void EnsenyarCarta(GameObject carta, bool click){
        if(carta.GetComponent<Reverso>().enabled == false){
            if(click){
                //instancio la carta
                copiaDeCarta = Instantiate(carta,panelFront.transform, false);
                copiaDeCarta.transform.localScale = nuevaEscala;
            }
            else{
                Destroy(copiaDeCarta);
            }
        }
    }

    
    void Start(){
        panelFront = GameObject.Find("PanelFrontal");
        nuevaEscala = new Vector3(1.5f, 1.5f,0);
    }
    
    void Update(){}
}
