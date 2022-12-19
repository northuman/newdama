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

        return false;
    }

    private bool EsTurnoIA() {

        return false;
    }

    private int CalcularDanyoInfligido() {

        return 0;
    }

    private int CalcularVidaGanadaOponente() {

        return 0;
    }
}
