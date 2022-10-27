using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour {

    public bool permitidoJugarCartas = false;

    public GameObject goTierras;
    public GameObject contadorVida;
    public GameObject goCriaturas;
    public GameObject goCementerio;

    public Baraja baraja;
    public Tierras tierras;
    public Cementerio cementerio;
    public int vida = 20;

    void Start() {

        baraja.propietario = this;
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

    public List<Arrastrable> GetArrastrables() {

        List<Arrastrable> arrastrables = new List<Arrastrable>();

        for(int i=0; i<goCriaturas.transform.childCount; i++) {

            arrastrables.Add(goCriaturas.transform.GetChild(i).GetComponent<Arrastrable>());
        }

        return arrastrables;
    }

    public void MandarCementerio() {

        List<Arrastrable> arrastrables = GetArrastrables();

        for(int i=0; i<arrastrables.Count; i++) {

            arrastrables[i].IrCementerio();
        }
    }
}
