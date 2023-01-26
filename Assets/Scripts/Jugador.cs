using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jugador : MonoBehaviour {

    const float TIEMPO_ROBO = 0.5f;

    public bool permitidoJugarCartas = false;

    public GameObject contadorVida;
    public GameObject goMano;
    public GameObject goTierras;
    public GameObject goCriaturas;
    public GameObject goCementerio;

    public Partida partida;
    public Baraja baraja;
    public Tierras tierras;
    public Cementerio cementerio;

    public int vida = 20;
    public int mulligan = 7;
    public int criaturasActivas = 0;
    public bool tierraDelTurnoJugada = false;

    public void Start() {

        baraja.propietario = this;
        partida = GameObject.Find("Partida").GetComponent<Partida>();
        if(partida == null) { Debug.Log("Partida No Encontrada"); }
    }

    public void Barajar() {

        baraja.Barajar();
    }

    public void RobarCarta() {

        baraja.RobarCarta();
    }

    public void EnderezarTierras() {

        for(int i=0; i<goTierras.transform.childCount; i++) {

            Arrastrable arrastrable = goTierras.transform.GetChild(i).GetComponent<Arrastrable>();
            if(arrastrable.cartaGirada) { arrastrable.EnderezarCarta(); }
            arrastrable.cartaGirada = false;
        }
    }

    public void EnderezarCriaturas() {

        criaturasActivas = 0;

        for(int i=0; i<goCriaturas.transform.childCount; i++) {

            Arrastrable arrastrable = goCriaturas.transform.GetChild(i).GetComponent<Arrastrable>();
            if(arrastrable.cartaGirada) { arrastrable.EnderezarCarta(); }
            criaturasActivas++;
            arrastrable.atacando = false;
            arrastrable.mareo = false;
            arrastrable.cartaGirada = false;
        }
    }

    public void DevolverCartaAlMazo(Carta carta) {

        baraja.AnyadirCarta(carta);
    }

    public void AnyadirMana(Carta carta) {

        tierras.AnyadirMana(carta);
    }

    public int ManaRestante() {

        int manaDisponible = 0;

        for(int i=0; i<goTierras.transform.childCount; i++) {

            Arrastrable arrastrable = goTierras.transform.GetChild(i).GetComponent<Arrastrable>();

            if(!arrastrable.cartaGirada) { manaDisponible++; }
        }

        return manaDisponible;
    }

    public void ActualizarVida() {

        TMPro.TMP_Text text = contadorVida.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
        //contadorVida.GetComponent<TMPro.TMP_Text>();
        if(text) {

            text.text = vida.ToString();
        }
    }

    public void RestarVidaPorMana() {

        vida -= tierras.ManaSinUsar();
        tierras.ReiniciarContador();
        ActualizarVida();
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

    public IEnumerator RobarCartas(int cantidad) {

        for(int i=cantidad; i>0; i--) {

            yield return new WaitForSeconds(TIEMPO_ROBO);
            RobarCarta();
        }

        if(mulligan == 0) { 
            
            Partida.quedarMano = true;
        }

        if(!Partida.quedarMano) {

            yield return partida.PreguntarMulligan();
        }
    }

    public void DevolverManoInicial() {

        Partida.cajaDialogo.SetActive(false);

        StartCoroutine(DevolverCartas(mulligan));
    }

    IEnumerator DevolverCartas(int cantidad) {

        for(int i=cantidad; i>=1; i--) {

            yield return new WaitForSeconds(TIEMPO_ROBO);
            
            GameObject cartaEnMano = goMano.transform.GetChild(i-1).gameObject;

            DevolverCartaAlMazo(Partida.CartaDesdeGameObject(cartaEnMano));

            Destroy(cartaEnMano);
        }

        Barajar();

        Partida.botonAceptar.GetComponent<Button>().onClick.RemoveAllListeners();
        Partida.botonCancelar.GetComponent<Button>().onClick.RemoveAllListeners();

        yield return RobarCartas(--mulligan);
    }

    public int CriaturasEnMesa() {

        return goCriaturas.transform.GetChildCount();
    }

    public int CriaturasAtacando() {

        int criaturasAtacando = 0;

        for(int i=0; i<goCriaturas.transform.childCount; i++) {

            Arrastrable arrastrable = goCriaturas.transform.GetChild(i).GetComponent<Arrastrable>();
            if(arrastrable.atacando) { criaturasAtacando++; }
        }

        return criaturasAtacando;
    }
}
