using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
* Controla un comportamiento cuando se hace click sobre un objeto.
*/
public class Seleccionar : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    GameObject cartaSeleccionada = null;
    public Transform parentToReturnTo = null;
    Vector3 nuevaEscala;
    bool rightClick = false;
    public UIElements funcionUI;
    

    public void OnPointerDown (PointerEventData eventData)
    {
        //click derecho
        if(eventData.button == PointerEventData.InputButton.Right){
            if(this.gameObject.GetComponent<MostrarCarta>().enabled){
                
                cartaSeleccionada = this.gameObject;
                rightClick = true;
                parentToReturnTo = this.transform.parent;

                funcionUI.EnsenyarTexto(cartaSeleccionada, true);
            }
        }
    }
    public void OnPointerUp (PointerEventData eventData) 
    {
        if (cartaSeleccionada != null){
            if(rightClick){
                funcionUI.EnsenyarTexto(cartaSeleccionada, false);
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
               GameObject.Find("Jugador").GetComponent<Jugador>().GirarTierra(cartaSeleccionada); 
            }
        }
    }

    void Start(){
        
        funcionUI = GameObject.Find("UI").GetComponent<UIElements>();
        
    }
}
