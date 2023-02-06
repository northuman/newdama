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

        Partida.faseActual = Partida.Fase.COMBATE;

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

        else if(jugador.CantidadCriaturasEnderezadas() == 0) {

            if(!ReciboDanyoSuficiente()) {

                AtacarConTodo();
            }
        }

        else if(estadisticasIA[0] >= estadisticasJugador[1] && estadisticasIA[1] > estadisticasJugador[0]) {

            foreach(Arrastrable criaturaIA in criaturasIA) {

                foreach(Arrastrable criaturaJug in criaturasJugador) {

                    if (criaturaIA.fuerzaTemp >= criaturaJug.resistenciaTemp &&
                        criaturaIA.resistenciaTemp > criaturaJug.fuerzaTemp) {

                        atacantes.Add(criaturaIA);
                    }

                    else if(CantidadCriaturasActivas() > jugador.CantidadCriaturasEnMesa()) {

                        if (criaturaIA.fuerzaTemp >= criaturaJug.resistenciaTemp &&
                            criaturaIA.resistenciaTemp >= criaturaJug.fuerzaTemp) {

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

            /*if(jugador.CantidadCriaturasEnderezadas() == 0) {

                AtacarConTodo();
            }

            else if(atacantes.Count == 0 && (CantidadCriaturasActivas()/jugador.CantidadCriaturasEnderezadas()) >= 3) {

                AtacarConTodo();
            }*/
        }

        else {

            Debug.Log("Me da miedito atacar");
        }
    }

    public bool EntraDanyoSuficiente() {

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

                danyoAsegurado += criaturasIA[criaturasIA.Count-1-i].fuerzaTemp;
            }

            if(danyoAsegurado > jugador.vida) {

                return true;
            }
        }

        return false;
    }

    public bool ReciboDanyoSuficiente() {

        Debug.Log("CANTIDAD CRIATURAS EN MESA JUGADOR: " + jugador.CantidadCriaturasEnMesa());
        Debug.Log("CANTIDAD CRIATURAS ENDEREZADAS IA: " + CantidadCriaturasEnderezadas());

        int diferenciaCriaturas = CantidadCriaturasEnderezadas() - jugador.CantidadCriaturasEnMesa();

        Debug.Log("La diferencia de criaturas es de: " + diferenciaCriaturas);

        if(diferenciaCriaturas > 0) {

            List<Arrastrable> criaturas = OrdenarCriaturasEstadisticas(jugador.CriaturasEnMesa());

            int danyoAsegurado = 0;

            if(diferenciaCriaturas > 0) {

                for(int i=0; i<diferenciaCriaturas && i<criaturas.Count; i++) {

                    danyoAsegurado += criaturas[criaturas.Count-1-i].fuerzaTemp;
                }

                if(danyoAsegurado > vida) {

                    return true;
                }
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

            int danyoTotal = ContarDanyo();
            Debug.Log("Danyo total: " + danyoTotal);

            List<Arrastrable> sinBloquear = jugador.CriaturasAtacando();
            List<Arrastrable> setForDestruction = new List<Arrastrable>();

            if(danyoTotal < vida) {

                foreach(Transform child in jugador.goCriaturas.transform) {

                    if(BuscarBloqueadorIdeal(child.GetComponent<Arrastrable>())) {

                        sinBloquear.Remove(child.GetComponent<Arrastrable>());
                    }
                }

                foreach(Arrastrable criatura in sinBloquear) {

                    Debug.Log("INTENTO BLOQUEOS MULTIPLES");

                    foreach(Transform child in goCriaturas.transform) {

                        Arrastrable a1 = child.GetComponent<Arrastrable>();

                        foreach(Transform child2 in goCriaturas.transform) {

                            Arrastrable a2 = child2.GetComponent<Arrastrable>();

                            if(a1 != a2) {

                                Debug.Log("PAREJA DISPONIBLE");
                                Debug.Log("SUMA FUERZAS: " + (a1.fuerzaTemp + a2.fuerzaTemp));
                                Debug.Log("SUMA RESISTENCIAS: " + (a1.resistenciaTemp + a2.resistenciaTemp));

                                if(!a1.bloqueando && !a2.bloqueando 
                                && a1.fuerzaTemp + a2.fuerzaTemp >= criatura.resistenciaTemp
                                && a1.resistenciaTemp + a2.resistenciaTemp > criatura.fuerzaTemp) {

                                    a1.bloqueando = true;
                                    a2.bloqueando = true;
                                    criatura.bloqueadaPor.Add(a1);
                                    criatura.bloqueadaPor.Add(a2);
                                    Partida.momentoCombate = Partida.Combate.ORDEN_BLOQUEADORES;
                                    //Partida.ModificarBoton();
                                }

                                else {

                                    Debug.Log("APRENDE A CONTAR");
                                }
                            }

                            else {

                                Debug.Log("SON LA MISMA");
                            }
                        }
                    }
                }
            }

            //Sumar fuerza total de los atacantes
            
            //Si es menor que la vida de la IA
            //Buscar bloqueador ideal
            //Si no hay, buscar doble bloqueo bueno
            //Si tampoco hay dejar pasar a menos que sea letal

            //Si es mayor que la vida
            //Bloquear seguro al mas fuerte con el mas debil
            //Sumar de nuevo el ataque de los restantes y repetir
        }

        Partida.pasarFase = false;
        //Partida.continuarFase = true;
    }

    public bool BuscarBloqueadorIdeal(Arrastrable arrastrableOponente) {

        foreach(Transform child in goCriaturas.transform) {

            Arrastrable arrastrable = child.GetComponent<Arrastrable>();
            
            if(!arrastrable.bloqueando && arrastrable.fuerzaTemp >= arrastrableOponente.resistenciaTemp 
            && arrastrable.resistenciaTemp > arrastrableOponente.fuerzaTemp) {

                Debug.Log(arrastrable.GetCarta().nombreCarta + " BLOQUEA A " + arrastrableOponente.GetCarta().nombreCarta);

                arrastrable.bloqueando = true;
                arrastrableOponente.bloqueadaPor.Add(arrastrable);
                return true;
            }
        }

        return false;
    }

    public int ContarDanyo() {

        int danyo = 0;

        foreach(Transform child in jugador.goCriaturas.transform) {

            Arrastrable criatura = child.GetComponent<Arrastrable>();

            if(criatura.atacando)
            danyo += criatura.fuerzaTemp;
        }

        return danyo;
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




