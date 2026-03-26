using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
* Descripcion: DropZone cambia el parent de la carta para que al soltarla se coloque en el panel.
* Adicionalmente se comprueba si está permitido colocarla
* OnDrop : Si se está arrastrando una carta, se guarda su tipo y su id. 
* Se llama a validarTipo. Si es válido cambio parentToReturnTo al panel de destino.
*/

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    GameObject cartaSeleccionada = null;
    GameObject jugador1 = null;
    GameObject jugador2 = null;
    Partida gestorPartida;
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

        if (eventData.pointerDrag.TryGetComponent<Arrastrar>(out var arrastrando))
        {
            cartaSeleccionada = eventData.pointerDrag;
            string tipo = eventData.pointerDrag.GetComponent<MostrarCarta>().tipo;

            if (ValidarTipo(tipo) && ValidarFase(tipo) && ValidarMana(cartaSeleccionada))
            {
                arrastrando.parentToReturnTo = this.transform;

                if(tipo.Equals("Tierra"))
                {
                    gestorPartida.tierrasJugadasEsteTurno = true; //marcamos que ya se ha jugado una tierra este turno.
                }
                
                /* PARA INICIALIZAR CARTA JUGADA*/
                var cj = cartaSeleccionada.GetComponent<CartasJugadas>();
                if (cj != null)
                {
                    var datos = cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta();
                    cj.Inicializar(datos);

                    gestorPartida.jugadorActivo.mano.Remove(datos); // Elimina la carta de la mano lógica del jugador activo
                }

                Debug.Log($"carta {tipo} jugada en {tipoZona}");
            }
            else
            {
                Debug.Log($"No se puede jugar la carta {tipo} en {tipoZona}");
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData){}

    public bool ValidarFase(string tipo)
        {
            // Los Instantáneos se saltan las reglas de fase (se pueden jugar siempre)
            if (tipo.Equals("Instantáneo")) 
            {
                return true;
            }

        // 2. Para el resto de cartas (Tierras, Criaturas, Conjuros...), 
        // DEBE ser el turno del jugador activo y DEBE ser una fase principal.

        if (tipo.Equals("Tierra"))
        {
            // Solo se puede jugar una tierra por turno, así que comprobamos si ya se ha jugado una.
            if (gestorPartida.tierrasJugadasEsteTurno)
            {
                Debug.LogWarning("No puedes jugar esta carta: Solo puedes jugar una tierra por turno.");
                return false;
            }
        }
            // Asumimos que gestorPartida.jugadorActivo es el jugador 1 (tú)
            bool esMiTurno = (gestorPartida.jugadorActivo.gameObject == jugador1);
            bool esFasePrincipal = (gestorPartida.faseActual == Partida.Fases.PRINCIPAL_1 || gestorPartida.faseActual == Partida.Fases.PRINCIPAL_2);

            if (esMiTurno && esFasePrincipal)
            {
                return true;
            }

            if (!esMiTurno) Debug.LogWarning("No puedes jugar esta carta: No es tu turno.");
            else if (!esFasePrincipal) Debug.LogWarning("No puedes jugar esta carta: Solo se puede en la Fase Principal.");
            
            return false;
        }

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

    /*
    * Recibe una carta como parámetro.
    * Si es una criatura entra al bucle.
    * Llama a ComprobarMana y calcula si se puede jugar la carta, si devuelve true se resta el maná.
    */
    public bool ValidarMana(GameObject carta)
    {
        bool ok = false;
        if (carta != null)
        {
            string tipo = carta.GetComponent<MostrarCarta>().tipo;
            int perteneceA = carta.GetComponent<CartasJugadas>().perteneceAJugador;
            if (tipo.Equals("Criatura")) //recordar que hay que añadir conjuros y encantamientos.
            {
                if (perteneceA == 1)
                {
                    if (jugador1.GetComponent<Jugador>().ComprobarMana(carta))
                    {
                        jugador1.GetComponent<Jugador>().RestarMana(carta);
                        ok = true;
                    }
                }
                else if (perteneceA == 2)
                {
                    if (jugador2.GetComponent<Jugador>().ComprobarMana(carta))
                    {
                        jugador2.GetComponent<Jugador>().RestarMana(carta);
                        ok = true;
                    }
                }
            }
            else
            {
                ok = true; //las tierras son gratis.
            }
        }
        return ok;
    }

    void Start()
    {
        jugador1 = GameObject.Find("Jugador");
        jugador2 = GameObject.Find("Oponente");

        gestorPartida = FindObjectOfType<Partida>(); // cerebro del juego para la escena.
    }
}
