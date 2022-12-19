using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccionJugarHechizo : Accion {

    public override bool EsValido() {

        return TengoHechizosUtilizables() && TengoSuficienteMana();
    }

    public override int Evaluar() {

        int puntuacion = CalcularBeneficios();
        return puntuacion;
    }

    private bool TengoHechizosUtilizables() {

        return false;
    }

    private bool TengoSuficienteMana() {

        return false;
    }

    private int CalcularBeneficios() {

        return 0;
    }
}
