using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IA : Jugador {

    public void JugarPrincipal() {

        JugarTierra();
        //Jugar Criaturas
        Combate();
    }

    //Tierras ---------------------------------------------------------------------------------------------------

    public void JugarTierra() {

        //Partimos de que todas seran basicas

        //Mirar si tengo tierra en mano
        //Jugarla
        Arrastrable tierra = BuscarTierra();
        if(tierra != null) {

            tierra.padreOriginal = goTierras.transform;
            tierra.transform.SetParent(goTierras.transform);
        }

        Partida.pasarFase = true;
    }

    public Arrastrable BuscarTierra() {

        foreach(Transform child in goMano.transform) {

            Arrastrable a = child.GetComponent<Arrastrable>();
            if(a.GetCarta().tipoCarta == "Tierra") {

                return a;
            }
        }

        return null;
    }

    //Tierras ---------------------------------------------------------------------------------------------------

    //Criaturas -------------------------------------------------------------------------------------------------

    public void JugarCriatura() {

        //Ver cuanto mana tengo
        //Ver cuantas criaturas tengo en la mano cuyo coste puedo pagar
        //Jugar una de esas criaturas

        int manaDisponible = ManaRestante();
    }

    //Criaturas -------------------------------------------------------------------------------------------------

    //Combate ---------------------------------------------------------------------------------------------------

    public void Combate() {

        Partida.pasarFase = true;
    }

    //Combate ---------------------------------------------------------------------------------------------------
}




