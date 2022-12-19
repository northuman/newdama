using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArbolDecisiones;

public class JugarCriatura : Nodo {
    
    IA oponente = null;

    public JugarCriatura(IA _oponente) {

        oponente = _oponente;
    }

    public override EstadoNodo Evaluar() {
        
        //Obtener Criaturas
        //Obtener Mana Disponible
        //Comprobar si puedo jugarlas una por una

        if(Partida.turno == Partida.Turno.OPONENTE && Partida.faseActual == Partida.Fase.PRINCIPAL) {

            Arrastrable criatura = oponente.PuedoJugarCriatura();

            if(criatura) {

                oponente.siguienteCarta = criatura;
                oponente.JugarCartaTras(2f);
            }

            if(!oponente.siguienteCarta)
            estado = EstadoNodo.FALLO;
        }

        return estado;
    }
}