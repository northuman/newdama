using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Accion {

    protected IA oponente;

    public abstract bool EsValido();

    public abstract int Evaluar();

    public void SetOponente(IA p_oponente) {

        oponente = p_oponente;
    }
}
