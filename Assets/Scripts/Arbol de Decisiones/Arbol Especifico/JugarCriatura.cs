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

        List<Arrastrable> criaturas = oponente.CriaturasEnMano();
        Debug.Log("Criaturas en mano: " + criaturas.Count);
        criaturas = oponente.DesordenarCriaturasEnMano(criaturas);

        bool suficienteMana = false;

        foreach(Arrastrable criatura in criaturas) {

            suficienteMana = oponente.tierras.SuficienteManaIA(criatura.GetCarta());

            if(suficienteMana) {
                
                oponente.JugarCarta(criatura);
                break;
            }
        }

        if(!suficienteMana) { Debug.Log("No hay mana suficiente"); }

        return estado;
    }
}
