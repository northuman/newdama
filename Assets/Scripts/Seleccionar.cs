using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
            Debug.Log("Selecciono " + cartaSeleccionada.GetComponent<MostrarCarta>().nombreCarta +" con Id " + cartaSeleccionada.GetComponent<MostrarCarta>().id);
        
        }
    }
    public void OnPointerEnter (PointerEventData eventData)
    {}
    public void OnPointerExit (PointerEventData eventData)
    {} 



    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
         
    }
}
