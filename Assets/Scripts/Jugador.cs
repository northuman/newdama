using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour {

    public bool permitidoJugarCartas = false;
    public bool turno = true;
    public Baraja baraja;
    public Tierras tierras;

    void Start() {

        baraja = GameObject.Find("Baraja 1").GetComponent<Baraja>();
        tierras = GameObject.Find("Tierras Jugador").GetComponent<Tierras>();
    }

    public void Barajar() {

        baraja.Barajar();
    }

    public void RobarCarta() {

        baraja.RobarCarta();
    }

    public void DevolverCartaAlMazo(Carta carta) {

        baraja.AnyadirCarta(carta);
    }

    public void AnyadirMana(Carta carta) {

        tierras.AnyadirMana(carta);
    }
}
