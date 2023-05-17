using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IA : Jugador {

    const float TIEMPO_ESPERA = 2f;

    public Jugador jugador;
    public GameObject goPila;
    public bool finJugarCriaturas = false;

    public IEnumerator JugarPrincipal() {

        JugarTierra();
        yield return new WaitForSeconds(TIEMPO_ESPERA);
        //JugarEncantamiento();
        //yield return new WaitForSeconds(TIEMPO_ESPERA);
        JugarInstantaneo();
        yield return new WaitForSeconds(TIEMPO_ESPERA);
        StartCoroutine(JugarCriaturas());
        yield return new WaitUntil(GetFinJugarCriaturas);
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

            tierra.cartaEnMano = false;
            tierra.MostrarCarta();
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

    public IEnumerator JugarCriaturas() {

        //Ver cuanto mana tengo
        //Ver cuantas criaturas tengo en la mano cuyo coste puedo pagar
        //Jugar una de esas criaturas

        int manaDisponible = ManaRestante();

        Arrastrable criatura = CriaturaJugable(manaDisponible);
        
        while(criatura != null) {

            PagarCoste(criatura.GetCarta().CosteTotal());
            criatura.MostrarCarta();
            criatura.padreOriginal = goCriaturas.transform;
            criatura.transform.SetParent(goCriaturas.transform);
            criatura.mareo = true;
            criatura.cartaEnMano = false;
            criaturaJugada = criatura;
            manaDisponible = ManaRestante();
            criatura = CriaturaJugable(manaDisponible);
            yield return new WaitForSeconds(1.0f);
        }

        finJugarCriaturas = true;
    }

    public bool GetFinJugarCriaturas() {

        return finJugarCriaturas;
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

    //Encantamientos --------------------------------------------------------------------------------------------

    public void JugarEncantamiento() {

        int manaDisponible = ManaRestante();

        Arrastrable encantamiento = EncantamientoJugable(manaDisponible);

        if(encantamiento) {

            PagarCoste(encantamiento.GetCarta().CosteTotal());
            encantamiento.padreOriginal = goEncantamientos.transform;
            encantamiento.transform.SetParent(goEncantamientos.transform);
            encantamiento.cartaEnMano = false;
            encantamiento.MostrarCarta();
        }
    }

    public Arrastrable EncantamientoJugable(int mana) {

        foreach(Transform child in goMano.transform) {

            Arrastrable a = child.GetComponent<Arrastrable>();

            if(a.tipoCarta == Arrastrable.TipoCarta.ENCANTEMIENTO) {

                if(a.GetCarta().CosteTotal() <= mana) {

                    return a;
                }
            }
        }

        return null;
    }

    //Encantamientos --------------------------------------------------------------------------------------------

    //Instantaneos ----------------------------------------------------------------------------------------------

    public void JugarInstantaneo() {

        int manaDisponible = ManaRestante();

        Arrastrable encantamiento = InstantaneoJugable(manaDisponible);

        if(encantamiento) {

            PagarCoste(encantamiento.GetCarta().CosteTotal());
            encantamiento.padreOriginal = goPila.transform;
            encantamiento.transform.SetParent(goPila.transform);
            encantamiento.cartaEnMano = false;
            encantamiento.MostrarCarta();
        }

        //for para comprobar efectos y mandar al cementerio
    }

    public Arrastrable InstantaneoJugable(int mana) {

        foreach(Transform child in goMano.transform) {

            Arrastrable a = child.GetComponent<Arrastrable>();

            if(a.tipoCarta == Arrastrable.TipoCarta.INSTANTANEO) {

                if(a.GetCarta().CosteTotal() <= mana) {

                    return a;
                }
            }
        }

        return null;
    }

    //Instantaneos ----------------------------------------------------------------------------------------------

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

                    else if(CantidadCriaturasActivas() >= jugador.CantidadCriaturasEnderezadas()) {

                        if (criaturaIA.fuerzaTemp >= criaturaJug.resistenciaTemp &&
                            criaturaIA.resistenciaTemp >= criaturaJug.fuerzaTemp) {

                            atacantes.Add(criaturaIA);
                        }

                        else {

                            Debug.Log("PASO POR AQUI");
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

    public void OrdenarBloqueadores(Arrastrable a) {

        a.bloqueadaPor.Sort((p1, p2) => p1.resistenciaTemp.CompareTo(p2.resistenciaTemp));
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

            //if(danyoTotal < vida) {

                foreach(Transform child in jugador.goCriaturas.transform) {

                    if(BuscarBloqueadorIdeal(child.GetComponent<Arrastrable>())) {

                        sinBloquear.Remove(child.GetComponent<Arrastrable>());
                    }
                }

                foreach(Arrastrable criatura in sinBloquear) {

                    foreach(Transform child in goCriaturas.transform) {

                        Arrastrable a1 = child.GetComponent<Arrastrable>();

                        foreach(Transform child2 in goCriaturas.transform) {

                            Arrastrable a2 = child2.GetComponent<Arrastrable>();

                            if(a1 != a2) {

                                if(!a1.bloqueando && !a2.bloqueando 
                                && a1.fuerzaTemp + a2.fuerzaTemp >= criatura.resistenciaTemp
                                && a1.resistenciaTemp + a2.resistenciaTemp > criatura.fuerzaTemp) {

                                    a1.bloqueando = true;
                                    a2.bloqueando = true;
                                    criatura.bloqueadaPor.Add(a1);
                                    criatura.bloqueadaPor.Add(a2);
                                    Partida.momentoCombate = Partida.Combate.ORDEN_BLOQUEADORES;
                                }
                            }
                        }
                    }
                }
            //}

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
            
            if(!arrastrable.cartaGirada && !arrastrable.bloqueando
            && arrastrable.fuerzaTemp >= arrastrableOponente.resistenciaTemp 
            && arrastrable.resistenciaTemp > arrastrableOponente.fuerzaTemp) {

                Debug.Log(arrastrable.GetCarta().nombreCarta + " BLOQUEA A " + arrastrableOponente.GetCarta().nombreCarta);

                arrastrable.bloqueando = true;
                //arrastrableOponente.bloqueadaPor.Add(arrastrable);
                arrastrableOponente.AnyadirBloqueador(arrastrable);
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




