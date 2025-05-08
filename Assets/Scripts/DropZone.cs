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
    GameObject jugador1 = null;
    GameObject jugador2 = null;
    public enum TipoDropZone {MANO, TIERRAS, BATALLA}
    public TipoDropZone tipoZona;
    
    /*
    * MANO : admite todos los tipos de carta
    * TIERRAS : admite solo tierras
    * BATALLA : admite crriaturas e instantaneos
    */


    public void OnPointerEnter(PointerEventData eventData){}

    public void OnDrop(PointerEventData eventData)
    {

        Arrastrar arrastrando = eventData.pointerDrag.GetComponent<Arrastrar>();
        if (arrastrando != null)
        {
            cartaSeleccionada = eventData.pointerDrag;
            string tipo = eventData.pointerDrag.GetComponent<MostrarCarta>().tipo;
            //int id= eventData.pointerDrag.GetComponent<MostrarCarta>().id;
            if(ValidarTipo(tipo) && ValidarMana(cartaSeleccionada))
            {
                arrastrando.parentToReturnTo = this.transform;
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData){}

    //comprueba si la carta se puede colocar en el panel
    public bool ValidarTipo(string tipo) 
    {
        bool validar = false;
        switch(tipoZona)
        {   
            //esto está activado de momento para poder mover las cartas libremente 
            case TipoDropZone.MANO: 
                validar = false;
                break;
            case TipoDropZone.TIERRAS:
                if(tipo.Equals("Tierra"))
                {
                    validar = true;
                }
                break;
            case TipoDropZone.BATALLA:
                if(tipo.Equals("Criatura") || tipo.Equals("Instantáneo"))
                {   
                    validar = true;
                }
                break;
        }
        return validar;
    }

    public bool ValidarMana(GameObject carta){
        bool ok = false;
        if(carta != null){
            string tipo = carta.GetComponent<MostrarCarta>().tipo;
            int perteneceA = carta.GetComponent<CartasJugadas>().perteneceAJugador;
            if(tipo.Equals("Criatura")){
                if(perteneceA==1){
                    if(jugador1.GetComponent<Jugador>().ComprobarMana(carta)){
                        jugador1.GetComponent<Jugador>().RestarMana(carta);
                        ok = true;
                    }
                }
                else if(perteneceA==2){
                    if(jugador2.GetComponent<Jugador>().ComprobarMana(carta)){
                        jugador2.GetComponent<Jugador>().RestarMana(carta);
                        ok = true;
                    }
                }
            }else{
                ok = true;
            }          
        }
        return ok;
    }

    void Start()
    {
        jugador1 = GameObject.Find("Jugador");
        jugador2 = GameObject.Find("Oponente");
    }
}
