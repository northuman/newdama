using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArbolDecisiones;

public class Atacar : Nodo {
    
    IA oponente = null;

    public Atacar(IA _oponente) {

        oponente = _oponente;
    }

    public override EstadoNodo Evaluar() {

        if(Partida.turno == Partida.Turno.OPONENTE) {

            if(Partida.faseActual == Partida.Fase.COMBATE &&
                Partida.momentoCombate == Partida.Combate.ATACANTES) {

                oponente.DeclararAtacantes();
                Partida.continuarFase = true;
                estado = EstadoNodo.FALLO;
            }

            else if(Partida.faseActual == Partida.Fase.COMBATE &&
                    Partida.momentoCombate == Partida.Combate.DANYO) {

                Partida.pasarFase = true;
                estado = EstadoNodo.EXITO;
            }

            else if (Partida.momentoCombate == Partida.Combate.CEMENTERIO) {

                Partida.pasarFase = true;
                estado = EstadoNodo.EXITO;
            }
        }
        
        return estado;
    }
}
