using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IA : Jugador {

    const float TIEMPO_ESPERA = 2f;

    public Jugador jugador;

    public void JugarPrincipal() {

        JugarTierra();
        //yield return new WaitForSeconds(TIEMPO_ESPERA);
        JugarCriatura();
        //yield return new WaitForSeconds(TIEMPO_ESPERA);
        Combate();
    }

    //

    public new IEnumerator RobarCartas(int cantidad) {

        for(int i=cantidad; i>0; i--) {

            yield return new WaitForSeconds(TIEMPO_ROBO);
            RobarCarta();
        }
    }

    //

    //Tierras ---------------------------------------------------------------------------------------------------

    /*public void JugarTierra() {

        //Partimos de que todas seran basicas

        //Mirar si tengo tierra en mano
        //Jugarla
        
    }*/

    public void JugarTierra() {

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

        if(criaturasActivas > 0) {

            if(jugador.CantidadCriaturasEnMesa() == 0) {

                AtacarConTodo();
            }

            else {

                AtaqueSelectivo();
            }
        }
    }

    public void AtacarConTodo() {

        //Intentar guardar alguna criatura para bloquear en caso de poca vida

        foreach(Transform child in goCriaturas.transform) {

            Arrastrable a = child.GetComponent<Arrastrable>();

            if(!a.mareo) {

                a.Atacar();
            }
        }
    }

    public void AtaqueSelectivo() {

        List<Arrastrable> criaturasIA = OrdenarCriaturasEstadisticas(CriaturasEnMesa());
        List<Arrastrable> criaturasJugador = OrdenarCriaturasEstadisticas(jugador.CriaturasEnMesa());

        List<Arrastrable> atacantes = new List<Arrastrable>();
        int[] estadisticasIA = EstadisticasSumadas();
        int[] estadisticasJugador = jugador.EstadisticasSumadas();

        if(estadisticasIA[0] >= estadisticasJugador[1] && estadisticasIA[1] > estadisticasJugador[0]) {

            foreach(Arrastrable criaturaIA in criaturasIA) {

                foreach(Arrastrable criaturaJug in criaturasJugador) {

                    if (criaturaIA.GetCarta().fuerzaTemp >= criaturaJug.GetCarta().resistenciaTemp &&
                        criaturaIA.GetCarta().resistenciaTemp > criaturaJug.GetCarta().fuerzaTemp) {

                        atacantes.Add(criaturaIA);
                    }

                    else if(CantidadCriaturasEnMesa() > jugador.CantidadCriaturasEnMesa()) {

                        if (criaturaIA.GetCarta().fuerzaTemp >= criaturaJug.GetCarta().resistenciaTemp &&
                            criaturaIA.GetCarta().resistenciaTemp >= criaturaJug.GetCarta().fuerzaTemp) {

                            atacantes.Add(criaturaIA);
                        }
                    }

                    else { break; }
                }
            }

            foreach(Arrastrable criaturaIA in atacantes) {

                if(!criaturaIA.mareo) {

                    criaturaIA.Atacar();
                }
            }
        }

        else {

            Debug.Log("Me salto el if");
        }
    }

    public bool AtaqueDirectoFavorable(Carta cartaIA, Carta cartaJugador) {

        return (cartaIA.fuerza >= cartaJugador.resistencia && cartaIA.resistencia > cartaJugador.fuerza);
    }

    //Combate ---------------------------------------------------------------------------------------------------

    /*

    Caso 1:
    El jugador no tiene criaturas 
    atacar con todo

    Caso 2:
    Ambos una criatura
        la del oponente mas fuerte -> ataca
        la del jugador mas fuerte -> nada
        ambas iguales -> nada

    Caso 3:
    Muchas criaturas, a simplificar
        la suma de ataques del oponente es mayor a la de las resistencias -> con todo
        la suma es menor -> nada
        ambas iguales -> solo las mas fuertes

    */
}




