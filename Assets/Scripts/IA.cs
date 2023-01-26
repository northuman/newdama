using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IA : Jugador {

    public Jugador jugador;

    public void JugarPrincipal() {

        JugarTierra();
        JugarCriatura();
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

    public void PagarCoste(int n) {

        foreach(Transform child in goTierras.transform) {

            Arrastrable a = child.GetComponent<Arrastrable>();

            if(!a.cartaGirada) {

                a.GirarCarta();
                n--;
            }

            if(n <= 0) { break; }
        }
    }

    //Criaturas -------------------------------------------------------------------------------------------------

    public void JugarCriatura() {

        //Ver cuanto mana tengo
        //Ver cuantas criaturas tengo en la mano cuyo coste puedo pagar
        //Jugar una de esas criaturas

        int manaDisponible = ManaRestante();
        
        Arrastrable criatura = CriaturaJugable(manaDisponible);

        if(criatura) {

            PagarCoste(criatura.GetCarta().CosteTotal());
            criatura.padreOriginal = goCriaturas.transform;
            criatura.transform.SetParent(goCriaturas.transform);
            criatura.mareo = true;
        }

        Partida.pasarFase = true;
    }

    public Arrastrable CriaturaJugable(int mana) {

        foreach(Transform child in goMano.transform) {

            Arrastrable a = child.GetComponent<Arrastrable>();

            if(a.tipoCarta == Arrastrable.TipoCarta.CRIATURA) {

                if(a.GetCarta().CosteTotal() <= mana) {

                    return a;
                }
            }
        }

        return null;
    }

    //Criaturas -------------------------------------------------------------------------------------------------

    //Combate ---------------------------------------------------------------------------------------------------

    public void Combate() {

        if(jugador.CriaturasEnMesa() == 0) {

            AtacarConTodo();
        }

        //Partida.pasarFase = true;
        //Partida.continuarFase = true;
    }

    public void AtacarConTodo() {

        foreach(Transform child in goCriaturas.transform) {

            Arrastrable a = child.GetComponent<Arrastrable>();

            if(!a.mareo) {

                a.Atacar();
            }
        }
    }

    //Combate ---------------------------------------------------------------------------------------------------
}




