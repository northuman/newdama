using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodoDecision {

    public Accion accion { get; set; }

    public NodoDecision padre { get; set; }

    public List<NodoDecision> hijos { get; set; }

    public NodoDecision() {

        hijos = new List<NodoDecision>();
    }

    public NodoDecision(Accion p_accion) {

        accion = p_accion;
        hijos = new List<NodoDecision>();
    }

    public void AnyadirHijo(NodoDecision hijo) {

        hijos.Add(hijo);
        hijo.padre = this;
    }
}
