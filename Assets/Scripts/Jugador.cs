using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jugador : MonoBehaviour {

    public const float TIEMPO_ROBO = 0.5f;

    public bool permitidoJugarCartas = false;

    public GameObject contadorVida;
    public GameObject goMano;
    public GameObject goTierras;
    public GameObject goCriaturas;
    public GameObject goCementerio;
    public GameObject goEncantamientos;

    public Partida partida;
    public Baraja baraja;
    public Tierras tierras;
    public Cementerio cementerio;
    public Jugador oponente;

    public int vida = 20;
    public int mulligan = 7;
    public int criaturasActivas = 0;
    public bool tierraDelTurnoJugada = false;

    public bool descartando = false;
    public bool equipando = false;
    public Arrastrable cartaEquipada = null;
    public Arrastrable equipo = null;
    public int cartasPorDescartar = 0;

    public int[] mejoraEstadisticasPropias;
    public int[] bajadaEstadisticasPropias;
    public int[] mejoraEstadisticasOponente;
    public int[] bajadaEstadisticasOponente;
    public int[] reduccionIncolora;
    public bool auraJugada = false;
    public Arrastrable criaturaJugada = null;
    
    public Efecto clickable = null;

    private float updateTime = 0.0f;

    private void Start() {

        baraja.propietario = this;
        partida = GameObject.Find("Partida").GetComponent<Partida>();
        if(partida == null) { Debug.Log("Partida No Encontrada"); }
    }

    private void Update() {
        
        updateTime += Time.deltaTime;

        if (updateTime > 1.0f && auraJugada) {

            updateTime = 0.0f;
            AplicarBuffosPropios();
        }
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

        if(text) {

            text.text = vida.ToString();
        }
    }

    public void RecibirDanyo(int cantidad) {

        if(cantidad > 0) {

            vida -= cantidad;
            ActualizarVida();
        }
    }

    public void GanarVidas(int cantidad) {

        if(cantidad > 0) {

            vida += cantidad;
            ActualizarVida();
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

    public IEnumerator DescartarCartas(Efecto efecto) {

        if(efecto.cantidad > 0) {
            cartasPorDescartar = efecto.cantidad;
            descartando = true;
        }
        yield return new WaitUntil(SeguirDescartando);
        efecto.resuelto = true;
    }

    public void DescartarCarta() {

        cartasPorDescartar--;
        Debug.Log("CartasPorDescartar: " + cartasPorDescartar);
        if(cartasPorDescartar <= 0) { descartando = false; }
        if(clickable) {

            clickable.resuelto = true;
            clickable = null;
        }

    }

    public bool SeguirDescartando() {

        return !descartando;
    }

    public IEnumerator DarPalabrasClave(Efecto efecto) {

        if(!efecto.objetivo) {

            equipando = true;
            yield return new WaitUntil(EquipandoCarta);
            efecto.objetivo = cartaEquipada;
            cartaEquipada.DarPalabrasClave(efecto);
            cartaEquipada = null;
        }
    }

    public IEnumerator EquiparCarta(Efecto efecto) {

        if(!efecto.objetivo) {

            equipando = true;
            yield return new WaitUntil(EquipandoCarta);
            efecto.objetivo = cartaEquipada;
            equipo.EscribirNombreEquipada(efecto);
            cartaEquipada = null;
            equipo = null;
        }

        else {

            Debug.Log("ALGO EXTRANYO PASABA");
        }
    }

    public bool EquipandoCarta() {

        return !equipando;
    }

    public void AnyadirAura(Aura aura) {
        
        auraJugada = true;
        if(aura.mejoraEstadisticasPropias.Length == 2) {

            ResetearEstadisticasPropias();
            //ResetearEstadisticasOponente();
            mejoraEstadisticasPropias[0] += aura.mejoraEstadisticasPropias[0];
            mejoraEstadisticasPropias[1] += aura.mejoraEstadisticasPropias[1];
        }

        if(aura.reduccionIncolora.Length == 4) {

            for(int i=0; i<4; i++) {

                if(aura.reduccionIncolora[i]) {

                    reduccionIncolora[i] += aura.cantidad;
                }
            }
        }
    }

    public void ResetearEstadisticasPropias() {

        foreach(Transform child in goCriaturas.transform) {

            Arrastrable a = child.GetComponent<Arrastrable>();
            a.ResetearEstadisticas();
        }
    }

    public void ResetearEstadisticasOponente() {

        foreach(Transform child in oponente.goCriaturas.transform) {

            Arrastrable a = child.GetComponent<Arrastrable>();
            a.ResetearEstadisticas();
        }
    }

    public void AplicarBuffosPropios() {

        foreach(Transform child in goCriaturas.transform) {

            Arrastrable a = child.GetComponent<Arrastrable>();

            if(a && !a.buffoJugadorAplicado) {

                if(mejoraEstadisticasPropias.Length == 2)
                    a.AplicarBuffo(mejoraEstadisticasPropias);
                if(bajadaEstadisticasPropias.Length == 2)
                    a.AplicarBuffo(bajadaEstadisticasPropias);
                a.ActualizarEstadisticas();
                a.buffoJugadorAplicado = true;
            }
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

    public int CantidadCriaturasEnMesa() {

        return goCriaturas.transform.childCount;
    }

    public int CantidadCriaturasEnderezadas() {

        int enderezadas = 0;

        foreach(Transform child in goCriaturas.transform) {

            if(!child.GetComponent<Arrastrable>().cartaGirada) {

                enderezadas++;
            }
        }

        Debug.Log("Enderezadas Jugador: " + enderezadas);

        return enderezadas;
    }

    public int CantidadCriaturasActivas() {

        int activas = 0;

        foreach(Transform child in goCriaturas.transform) {

            if(!child.GetComponent<Arrastrable>().cartaGirada && !child.GetComponent<Arrastrable>().mareo) {

                activas++;
            }
        }

        Debug.Log("Activas IA: " + activas);

        return activas;
    }

    public Arrastrable CriaturaEnMesa() {

        return goCriaturas.transform.GetChild(0).GetComponent<Arrastrable>();
    }

    public List<Arrastrable> CriaturasEnMesa() {

        List<Arrastrable> criaturas = new List<Arrastrable>();

        foreach(Transform child in goCriaturas.transform) {

            criaturas.Add(child.GetComponent<Arrastrable>());
        }

        return criaturas;
    }

    public List<Arrastrable> OrdenarCriaturasEstadisticas(List<Arrastrable> criaturas) {

        Arrastrable aux = null;

        for(int i=0; i<(criaturas.Count)-1; i++) {

            for(int j=0; j<(criaturas.Count)-i-1; j++) {

                if(criaturas[j].fuerzaTemp < criaturas[j+1].fuerzaTemp) {

                    aux = criaturas[j];
                    criaturas[j] = criaturas[j+1];
                    criaturas[j+1] = aux;
                }
            }
        }

        return criaturas;
    }

    public List<Arrastrable> CriaturasAtacando() {

        List<Arrastrable> atacantes = new List<Arrastrable>();

        foreach(Transform child in goCriaturas.transform) {

            if(child.GetComponent<Arrastrable>().atacando)
            atacantes.Add(child.GetComponent<Arrastrable>());
        }

        return OrdenarCriaturasEstadisticas(atacantes);
    }

    public int CantidadCriaturasAtacando() {

        int criaturasAtacando = 0;

        for(int i=0; i<goCriaturas.transform.childCount; i++) {

            Arrastrable arrastrable = goCriaturas.transform.GetChild(i).GetComponent<Arrastrable>();
            if(arrastrable.atacando) { criaturasAtacando++; }
        }

        return criaturasAtacando;
    }

    public int[] EstadisticasSumadas() {

        int[] estadisticas = new int[2];
        List<Arrastrable> criaturas = CriaturasEnMesa();

        foreach(Arrastrable criatura in criaturas) {

            estadisticas[0] += criatura.fuerzaTemp;
            estadisticas[1] += criatura.resistenciaTemp;
        }

        return estadisticas;
    }
}
