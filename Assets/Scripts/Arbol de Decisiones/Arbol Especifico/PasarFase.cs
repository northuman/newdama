using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArbolDecisiones;

public class PasarFase : Nodo {

    IA oponente = null;

    public PasarFase(IA _oponente) {

        oponente = _oponente;
    }

    public override EstadoNodo Evaluar() {

        estado = EstadoNodo.FALLO;

        if(Partida.turno == Partida.Turno.OPONENTE) {

            if(Partida.faseActual == Partida.Fase.PRINCIPAL ||
                Partida.faseActual == Partida.Fase.PRINCIPAL2) {

                if(oponente.tierraDelTurnoJugada) {

                    oponente.PasarFase();
                    estado = EstadoNodo.EXITO;
                }
            }

            else if(Partida.faseActual == Partida.Fase.COMBATE) {

                oponente.ContinuarFase();
                estado = EstadoNodo.EXITO;
            }
        }

        else { estado = EstadoNodo.FALLO; }

        return estado;
    }
}
