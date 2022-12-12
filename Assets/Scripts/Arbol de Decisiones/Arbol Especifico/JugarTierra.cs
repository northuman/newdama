using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArbolDecisiones;

public class JugarTierra : Nodo {

    IA oponente = null;

    public JugarTierra(IA _oponente) {

        oponente = _oponente;
        estado = EstadoNodo.FALLO;
    }

    public override EstadoNodo Evaluar() {

        if(Partida.turno == Partida.Turno.OPONENTE && Partida.faseActual == Partida.Fase.PRINCIPAL
            && !oponente.tierraDelTurnoJugada && oponente.tierraEnMano) {

            if(estado == EstadoNodo.FALLO) {

                oponente.JugarTierraTras(2f);
                estado = EstadoNodo.EXITO;
            }
        }

        if(oponente.tierraDelTurnoJugada || (!oponente.tierraDelTurnoJugada && !oponente.tierraEnMano)) {

            estado = EstadoNodo.FALLO;
        }

        return estado;
    }
}
