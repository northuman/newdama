using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IA : Jugador {

    public bool manaSuficienteJugarCriatura = false;
    public bool tierraEnMano = true;
    public Arrastrable siguienteCarta = null;

    public void PasarFase() {

        Partida.pasarFase = true;
    }

    public void ContinuarFase() {

        Partida.continuarFase = true;
    }

    public List<Arrastrable> TierrasEnMano() {

        List<Arrastrable> tierrasEnMano = new List<Arrastrable>();
        Arrastrable carta = null;

        for(int i=0; i<goMano.transform.childCount; i++) {

            carta = goMano.transform.GetChild(i).GetComponent<Arrastrable>();

            if(carta && carta.tipoCarta == Arrastrable.TipoCarta.TIERRA) {

                tierrasEnMano.Add(carta);
            }
        }

        return tierrasEnMano;
    }

    public List<Arrastrable> TierrasSimplesEnMano() {

        List<Arrastrable> simplesEnMano = new List<Arrastrable>();
        Arrastrable arrastrable = null;
        Carta carta = null;

        for(int i=0; i<goMano.transform.childCount; i++) {

            arrastrable = goMano.transform.GetChild(i).GetComponent<Arrastrable>();
            carta = arrastrable.GetCarta();

            if(arrastrable && arrastrable.tipoCarta == Arrastrable.TipoCarta.TIERRA) {

                if(carta.cantidadMana.SequenceEqual(carta.cantidadMana2))
                    simplesEnMano.Add(arrastrable);
            }
        }

        return simplesEnMano;
    }

    public List<Arrastrable> TierrasDoblesEnMano() {

        List<Arrastrable> doblesEnMano = new List<Arrastrable>();
        Arrastrable arrastrable = null;
        Carta carta = null;

        for(int i=0; i<goMano.transform.childCount; i++) {

            arrastrable = goMano.transform.GetChild(i).GetComponent<Arrastrable>();
            carta = arrastrable.GetCarta();

            if(arrastrable && arrastrable.tipoCarta == Arrastrable.TipoCarta.TIERRA) {

                if(!carta.cantidadMana.SequenceEqual(carta.cantidadMana2))
                    doblesEnMano.Add(arrastrable);
            }
        }

        return doblesEnMano;
    }

    public void JugarTierraTras(float seg) {

        Invoke("JugarTierra", seg);
    }

    public void JugarTierra() {

        List<Arrastrable> tierrasMano = TierrasEnMano();

        if(tierrasMano.Count > 0) {

            int tierraElegida = Random.Range(0, tierrasMano.Count-1);
            tierrasMano[tierraElegida].transform.SetParent(this.goTierras.transform);
            tierras.tierras.Add(tierrasMano[tierraElegida]);

            tierraDelTurnoJugada = true;
            tierrasMano.RemoveAt(tierraElegida);

            if(tierrasMano.Count > 0) { tierraEnMano = true; }
        }

        else {

            tierraEnMano = false;
        }
    }

    public List<Arrastrable> CriaturasEnMano() {

        List<Arrastrable> criaturasEnMano = new List<Arrastrable>();
        Arrastrable carta = null;

        for(int i=0; i<goMano.transform.childCount; i++) {

            carta = goMano.transform.GetChild(i).GetComponent<Arrastrable>();

            if(carta && carta.tipoCarta == Arrastrable.TipoCarta.CRIATURA) {

                criaturasEnMano.Add(carta);
            }
        }

        return criaturasEnMano;
    }

    public List<Arrastrable> OrdenarCriaturasPorCoste(List<Arrastrable> criaturas) {

        Arrastrable aux = null;

        for(int i=0; i<criaturas.Count; i++) {

            for(int j=0; j<criaturas.Count-i; i++) {

                if(criaturas[j].GetCarta().CosteTotal() > criaturas[j+1].GetCarta().CosteTotal()) {

                    aux = criaturas[j];
                    criaturas[j] = criaturas[j+1];
                    criaturas[j+1] = aux;
                }
            }
        }

        return criaturas;
    }

    public Arrastrable PuedoJugarCriatura() {

        Arrastrable puedoJugar = null;
        List<Arrastrable> criaturasEnMano = OrdenarCriaturasPorCoste(CriaturasEnMano());
        Carta criatura = null;

        foreach(Arrastrable arrastrable in criaturasEnMano) {

            criatura = arrastrable.GetCarta();
            if(tierras.Jugable(criatura)) {

                Debug.Log("Es jugable");
                puedoJugar = arrastrable;
                break;
            }
            //puedoJugar = arrastrable;
            //break;
        }

        return puedoJugar;
    }

    public void JugarCartaTras(float seg) {

        Invoke("JugarCarta", seg);
    }

    public void JugarCarta() {

        Debug.Log("Entro a Pagar Coste");

        tierras.PagarCoste(siguienteCarta.GetCarta());
        siguienteCarta.transform.SetParent(goCriaturas.transform);
        siguienteCarta = null;
    }

    public void DeclararAtacantes() {

        foreach(Transform carta in goCriaturas.transform) {

            Arrastrable criatura = carta.GetComponent<Arrastrable>();

            if(!criatura.mareo && !criatura.cartaGirada) {

                criatura.Atacar();
            }
        }
    }
}
