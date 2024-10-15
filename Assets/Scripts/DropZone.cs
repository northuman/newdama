using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
* Descripcion: DropZone cambia el parent de la carta para que al soltarla se coloque en el panel.
* Adicionalmente se comprueba si está permitido colocarla
* OnPointerEnter :
* OnDrop : Si se está arrastrando una carta, se guarda su tipo y su id. 
* Se llama a validarTipo. Si es válido cambio parentToReturnTo al panel de destino.
* OnPointerExit :
*/

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    GameObject cartaSeleccionada = null;
    public enum tipoDropZone {MANO, TIERRAS, BATALLA}
    
    /*
    * MANO : admite todos los tipos de carta
    * TIERRAS : admite solo tierras
    * BATALLA : admite crriaturas e instantaneos
    */

    public tipoDropZone tipoZona;

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log(eventData.pointerDrag.name + " dropea en " + gameObject.name);

        Arrastrar arrastrando = eventData.pointerDrag.GetComponent<Arrastrar>();
        if (arrastrando != null)
        {
            cartaSeleccionada = eventData.pointerDrag.gameObject;
            string tipo = eventData.pointerDrag.GetComponent<MostrarCarta>().tipo; 
            int id= eventData.pointerDrag.GetComponent<MostrarCarta>().id;

            //comprobando si el tipo es valido
            if(validarTipo(tipo))
            {
                arrastrando.parentToReturnTo = this.transform;
                //comprobando si tienes mana suficiente
                //if(cartaSeleccionada.GetComponent<Jugador>().CalcularCoste(cartaSeleccionada)){}
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {

    }

    //comprueba si la carta se puede colocar en el panel
    public bool validarTipo(string tipo) 
    {
        bool validar = false;
        switch(tipoZona)
        {
            case tipoDropZone.MANO: 
                validar = true;
                break;
            case tipoDropZone.TIERRAS:
                if(tipo.Equals("Tierra"))
                {
                    validar = true;

                }
                break;
            case tipoDropZone.BATALLA:
                if(tipo.Equals("Criatura") || tipo.Equals("Instantáneo"))
                {
                    validar = true;
                }
                break;
        }
        return validar;
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
