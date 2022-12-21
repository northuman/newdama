using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccionAtaque : Accion {

    public override bool EsValido() {
        
        return TengoAtacantesDisponibles() && EsTurnoIA();
    }

    public override int Evaluar() {
        
        int puntuacion = CalcularDanyoInfligido() - CalcularVidaGanadaOponente();
        return puntuacion;
    }

    private bool TengoAtacantesDisponibles() {

        bool atacantesDisponibles = false;

        if(oponente.AtacantesDisponibles()) {

            atacantesDisponibles = true;
        }

        return atacantesDisponibles;
    }

    private bool EsTurnoIA() {

        bool turnoIA = false;

        if(Partida.turno == Partida.Turno.OPONENTE) { turnoIA = true; }

        return turnoIA;
    }

    private int CalcularDanyoInfligido() {

        //Calcular el danyo total que las criaturas de la IA haran al atacar

        return oponente.DanyoTotal();
    }

    private int CalcularVidaGanadaOponente() {

        //Calcular la vida que ganaria el oponente al bloquear o al morir criaturas

        //int vidaGanada = jugador.VidaGanada();

        return 0;
    }
}
