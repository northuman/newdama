using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IA : Jugador {

    const float TIEMPO_ESPERA = 2f;

    public Jugador jugador;

    public IEnumerator JugarPrincipal() {

        JugarTierra();
        yield return new WaitForSeconds(TIEMPO_ESPERA);
        JugarCriatura();
        yield return new WaitForSeconds(TIEMPO_ESPERA);
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

        //Partida.pasarFase = true;
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

        Partida.pasarFase = true;
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

        EntraDanyoSuficiente();

        if(EntraDanyoSuficiente()) {

            AtacarConTodo();
        }

        else if(estadisticasIA[0] >= estadisticasJugador[1] && estadisticasIA[1] > estadisticasJugador[0]) {

            foreach(Arrastrable criaturaIA in criaturasIA) {

                foreach(Arrastrable criaturaJug in criaturasJugador) {

                    if (criaturaIA.GetCarta().fuerzaTemp >= criaturaJug.GetCarta().resistenciaTemp &&
                        criaturaIA.GetCarta().resistenciaTemp > criaturaJug.GetCarta().fuerzaTemp) {

                        atacantes.Add(criaturaIA);
                    }

                    else if(CantidadCriaturasActivas() > jugador.CantidadCriaturasEnMesa()) {

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

            if(atacantes.Count == 0 && (CantidadCriaturasActivas()/jugador.CantidadCriaturasEnderezadas()) >= 3) {

                AtacarConTodo();
            }
        }

        else {

            Debug.Log("Me da miedito atacar");
        }
    }

    public bool EntraDanyoSuficiente() {

        Debug.Log("Entro a calcularlo");

        //Contamos cuantas criaturas tiene el jugador
        //Contamos cuantas tiene la IA
        //Restamos ese numero
        //Si la cantidad de criaturas que no pueden ser bloqueadas tienen el danyo suficiente pegamos con todo
        //GG

        int diferenciaCriaturas = CantidadCriaturasActivas() - jugador.CantidadCriaturasEnderezadas();

        Debug.Log("La diferencia de criaturas es de: " + diferenciaCriaturas);

        if(diferenciaCriaturas > 0) {

            List<Arrastrable> criaturasIA = OrdenarCriaturasEstadisticas(CriaturasEnMesa());

            int danyoAsegurado = 0;

            for(int i=0; i<diferenciaCriaturas; i++) {

                danyoAsegurado += criaturasIA[criaturasIA.Count-1-i].GetCarta().fuerzaTemp;
            }

            if(danyoAsegurado > jugador.vida) {

                return true;
            }
        }

        return false;
    }

    public bool AtaqueDirectoFavorable(Carta cartaIA, Carta cartaJugador) {

        return (cartaIA.fuerza >= cartaJugador.resistencia && cartaIA.resistencia > cartaJugador.fuerza);
    }

    //Combate ---------------------------------------------------------------------------------------------------

    //Bloqueo ---------------------------------------------------------------------------------------------------

    public void Bloqueadores() {

        int atacantes = ContarAtacantes();
        
        if(atacantes > 0) {

            Debug.Log("Hay " + atacantes + " atacantes");
        }

        Partida.continuarFase = true;
    }

    public int ContarAtacantes() {

        int atacantes = 0;

        foreach(Transform child in jugador.goCriaturas.transform) {

            Arrastrable criatura = child.GetComponent<Arrastrable>();
            if(criatura.atacando) { atacantes++; }
        }

        return atacantes;
    }

    //Bloqueo ---------------------------------------------------------------------------------------------------
}




