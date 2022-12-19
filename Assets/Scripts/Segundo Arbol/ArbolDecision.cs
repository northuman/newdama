using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbolDecision : MonoBehaviour {

    private IA oponente;
    
    private NodoDecision raiz;

    private NodoDecision actual;

    private List<Accion> acciones;

    private void Start() {
        
        oponente = this.GetComponent<IA>();
        if(oponente) { Debug.Log(oponente.name); }
    }

    public ArbolDecision() {

        raiz = new NodoDecision();
        actual = raiz;

        acciones = new List<Accion>();
        acciones.Add(new AccionAtaque());
        acciones.Add(new AccionJugarHechizo());
        acciones.Add(new AccionUsarHabilidad());
    }

    public void TomarDecision() {

        
    }
}
