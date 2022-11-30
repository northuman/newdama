using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArbolDecisiones;

public class Esperar : Nodo {

    public Esperar() {}

    public override EstadoNodo Evaluar() {

        bool esperando = false;

        if(Partida.turno != Partida.Turno.OPONENTE) {

            esperando = true;
        }

        if(esperando) { estado = EstadoNodo.EXITO; Debug.Log("Esperando"); }
        else { estado = EstadoNodo.FALLO; }

        return estado;
    }
}
