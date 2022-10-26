using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public static GameObject pila;
    public static GameObject manoJugador;
    public static GameObject tierrasJugador;
    public static GameObject criaturasJugador;

    void Start() {
        
        pila = GameObject.Find("Pila");
        manoJugador = GameObject.Find("Mano Jugador");
        tierrasJugador = GameObject.Find("Tierras Jugador");
        criaturasJugador = GameObject.Find("Criaturas Jugador");
    }
    
    public void OnPointerEnter(PointerEventData datosEvento) {

        //Debug.Log("OnPointerEnter to " + gameObject.name);

        if(datosEvento.pointerDrag == null) { return; }

        Arrastrable carta = datosEvento.pointerDrag.GetComponent<Arrastrable>();

        if(carta != null) {


        }
    }

    public void OnPointerExit(PointerEventData datosEvento) {

        //Debug.Log("OnPointerExit to " + gameObject.name);

        if(datosEvento.pointerDrag == null) { return; }

        Arrastrable carta = datosEvento.pointerDrag.GetComponent<Arrastrable>();

        if(carta != null && carta.padrePlaceholder == this.transform) {


        }
    }

    public void OnDrop(PointerEventData datosEvento) {

        //Debug.Log(datosEvento.pointerDrag.name + " fue soltado sobre " + gameObject.name);

        Arrastrable carta = datosEvento.pointerDrag.GetComponent<Arrastrable>();

        if(carta != null && Partida.faseActual != Partida.Fase.MULLIGAN) {

            if(carta.propietario.permitidoJugarCartas) {

                if(gameObject.name == "Mano Jugador" && carta.padreOriginal == manoJugador.transform) {

                    carta.padreOriginal = manoJugador.transform;
                }

                else if(carta.tipoCarta == Arrastrable.TipoCarta.TIERRA && (Partida.faseActual != Partida.Fase.PRINCIPAL || 
                Partida.faseActual != Partida.Fase.PRINCIPAL2 )) {

                    carta.padreOriginal = tierrasJugador.transform;
                }

                else if(carta.tipoCarta == Arrastrable.TipoCarta.CRIATURA) {

                    bool jugarCarta = Partida.jugador.tierras.SuficienteMana(carta.gameObject.GetComponent<MostrarDatosCarta>().carta);

                    if(jugarCarta) {

                        carta.padreOriginal = pila.transform;
                        Pila.pila.Push(carta);
                    }
                }

                else {

                    carta.AnyadirCartaPila();
                }
            }
        }
    }
}
