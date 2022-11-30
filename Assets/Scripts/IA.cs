using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : Jugador {

    public bool manaSuficienteJugarCriatura = false;

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

    public bool JugarTierra(List<Arrastrable> tierras) {

        if(tierras.Count > 0) {

            int tierraElegida = Random.Range(0, tierras.Count-1);
            tierras[tierraElegida].transform.SetParent(this.goTierras.transform);
            tierraDelTurnoJugada = true;
        }

        return tierraDelTurnoJugada;
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

    public bool JugarCriatura(List<Arrastrable> criaturas) {

        bool jugarCriatura = false;

        int j;
        Arrastrable aux;

        for(int i = criaturas.Count-1; i > 0; i--) {

            j = Random.Range(0, i);
            aux = criaturas[i];
            criaturas[i] = criaturas[j];
            criaturas[j] = aux;
        }

        if(criaturas.Count > 0) {

            for(int i=0; i<criaturas.Count; i++) {

                Carta carta = criaturas[i].GetComponent<MostrarDatosCarta>().carta;

                if(this.tierras.SuficienteMana(carta)) {

                    criaturas[i].transform.SetParent(this.goCriaturas.transform);
                    jugarCriatura = true;
                    break;
                }   
            }
        }

        return jugarCriatura;
    }
}
