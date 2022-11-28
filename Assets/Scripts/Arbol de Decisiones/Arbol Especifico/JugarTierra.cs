using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArbolDecisiones;

public class JugarTierra : Nodo {

    IA oponente = null;

    public JugarTierra(IA _oponente) {

        oponente = _oponente;
    }

    public override EstadoNodo Evaluar() {

        bool jugarTierra = false;

        oponente.TierrasEnMano();
        
        if(Partida.turno == Partida.Turno.OPONENTE && Partida.faseActual == Partida.Fase.PRINCIPAL) {

            if(!oponente.tierraDelTurnoJugada) {

                jugarTierra = oponente.JugarTierra();
            }
        }

        if(jugarTierra) { estado = EstadoNodo.EXITO; }
        else { estado = EstadoNodo.FALLO; }

        return estado;
    }
}
