using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour {

    public bool permitidoJugarCartas = false;
    public Baraja baraja;
    public GameObject goTierras;
    public GameObject contadorVida;
    public GameObject criaturas;
    public Tierras tierras;
    public int vida = 20;

    void Start() {

        //baraja = GameObject.Find("Baraja 1").GetComponent<Baraja>();
        //tierras = GameObject.Find("Tierras Jugador").GetComponent<Tierras>();
        tierras = goTierras.GetComponent<Tierras>();
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

    public void ActualizarVida() {

        TMPro.TMP_Text text = contadorVida.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
        //contadorVida.GetComponent<TMPro.TMP_Text>();
        if(text) {

            text.text = vida.ToString();
        }
    }
}
