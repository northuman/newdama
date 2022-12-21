using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbolDecision : MonoBehaviour {

    private IA oponente;
    
    private NodoDecision raiz;

    private NodoDecision actual;

    private List<Accion> acciones;

    private void Start() {

        raiz = new NodoDecision();
        actual = raiz;
        oponente = GetComponent<IA>();
        AccionAtaque aA = new AccionAtaque();

        acciones = new List<Accion>();
        acciones.Add(aA);
        acciones.Add(new AccionJugarHechizo());
        acciones.Add(new AccionUsarHabilidad());

        SetOponente();
        aA.EsValido();
    }

    public ArbolDecision() {

    }

    public void TomarDecision() {

        
    }

    public void SetOponente() {

        foreach(Accion accion in acciones) {

            accion.SetOponente(oponente);
        }
    }
}
