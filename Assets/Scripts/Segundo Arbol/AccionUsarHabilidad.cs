using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccionUsarHabilidad : Accion {

    public override bool EsValido() {
        
        return TengoHabilidadesUtilizables() && TengoSuficientesRecursos();
    }

    public override int Evaluar() {
        
        int puntuacion = CalcularBeneficios();
        return puntuacion;
    }

    private bool TengoHabilidadesUtilizables() {

        return false;
    }

    private bool TengoSuficientesRecursos() {

        return false;
    }

    private int CalcularBeneficios() {

        return 0;
    }
}
