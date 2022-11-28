using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : Jugador {

    private List<Arrastrable> tierrasEnMano;

    public void TierrasEnMano() {

        tierrasEnMano = new List<Arrastrable>();
        Arrastrable carta = null;

        for(int i=0; i<goMano.transform.childCount; i++) {

            carta = goMano.transform.GetChild(i).GetComponent<Arrastrable>();

            if(carta && carta.tipoCarta == Arrastrable.TipoCarta.TIERRA) {

                tierrasEnMano.Add(carta);
            }
        }

        Debug.Log("Tierras en mano: " + tierrasEnMano.Count);
    }
    
    public bool JugarTierra() {

        if(tierrasEnMano.Count > 0) {

            int tierraElegida = Random.Range(0, tierrasEnMano.Count-1);
            tierrasEnMano[tierraElegida].transform.SetParent(this.goTierras.transform);
            Debug.Log("La tierra que se intento jugar esta en" + tierrasEnMano[tierraElegida].transform.parent.name);
            tierraDelTurnoJugada = true;
        }

        return tierraDelTurnoJugada;
    }
}
