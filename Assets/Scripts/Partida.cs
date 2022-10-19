using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Partida : MonoBehaviour {

    const float TIEMPO_ROBO = 0.5f;
    const float ENTRETIEMPO = 1f;

    public enum Fase { MULLIGAN, MANTENIMIENTO, ROBO, PRINCIPAL, COMBATE, PRINCIPAL2, FIN };
    public enum Turno { JUGADOR, OPONENTE };

    public static Fase faseActual = Fase.MULLIGAN;
    public static Turno turno = Turno.JUGADOR;
    static int mulligan = 7;
    public static bool quedarMano = false;
    public static bool pasarFase = false;
    public static bool continuarFase = false;
    //public static bool jugarCartas = false;

    public static Jugador jugador;
    public static Jugador oponente;

    static GameObject manoJugador;

    static Button botonFases;
    static GameObject cajaDialogo;
    static GameObject botonAceptar;
    static GameObject botonCancelar;
    
    void Start() {
        
        EmpezarPartida();
    }

    void EmpezarPartida() {

        jugador = GameObject.Find("Jugador").GetComponent<Jugador>();
        oponente = GameObject.Find("Oponente").GetComponent<Jugador>();
        manoJugador = GameObject.Find("Mano Jugador");

        botonFases = GameObject.Find("Boton Fases").GetComponent<Button>();
        cajaDialogo = GameObject.Find("Caja de Dialogo");
        botonAceptar = GameObject.Find("Boton Aceptar");
        botonCancelar = GameObject.Find("Boton Cancelar");
        cajaDialogo.SetActive(false);

        jugador.Barajar();

        StartCoroutine(EmpezarCicloPartida());
    }

    IEnumerator RobarCartas(int cantidad) {

        for(int i=cantidad; i>0; i--) {

            yield return new WaitForSeconds(TIEMPO_ROBO);
            jugador.RobarCarta();
        }

        if(mulligan == 0) { 
            
            quedarMano = true;
        }

        if(!quedarMano) {

            yield return PreguntarMulligan();
        }
    }

    // Ciclo Partida --------------------------------------------------------------------------------------------------

    IEnumerator EmpezarCicloPartida() {
  
        Mulligan(); 

        yield return new WaitUntil(GetQuedarMano);
        
        //while(vidaJugador > 0 && vidaOponente > 0) {

            StartCoroutine(Mantenimiento());
            yield return new WaitUntil(GetPasarFase);
            StartCoroutine(FasePrincipal());
            yield return new WaitUntil(GetPasarFase);
            StartCoroutine(FaseCombate());
            yield return new WaitUntil(GetPasarFase);
            Ataque();
            
            //break;
        //}
    }

    bool GetQuedarMano() {
        
        return quedarMano; 
    }

    public void SetPasarFaseTrue() {

        pasarFase = true;
    }

    // Ciclo Partida --------------------------------------------------------------------------------------------------

    // Fase Combate ---------------------------------------------------------------------------------------------------

    void Ataque() {

        GameObject criaturas = DropZone.criaturasJugador;

        for(int i=0; i<criaturas.transform.childCount; i++) {

            Transform criatura = criaturas.transform.GetChild(i);
            Arrastrable arrastrable = criatura.gameObject.GetComponent<Arrastrable>();

            if(arrastrable.cartaGirada) {

                arrastrable.InflingirDanyo();
            }
        }
    }

    IEnumerator FaseCombate() {

        pasarFase = false;

        Debug.Log("Fase Combate");
        faseActual = Fase.COMBATE;
        yield return new WaitForSeconds(ENTRETIEMPO);

        //Hacer brillar criaturas que puedan
    }

    // Fase Combate ---------------------------------------------------------------------------------------------------

    // Fase Principal -------------------------------------------------------------------------------------------------

    IEnumerator FasePrincipal() {
        
        pasarFase = false;

        Debug.Log("Fase Principal");
        faseActual = Fase.PRINCIPAL;
        yield return new WaitForSeconds(ENTRETIEMPO);

        if(turno == Turno.JUGADOR) {

            jugador.permitidoJugarCartas = true;
        }
    }

    bool GetPasarFase() {

        return pasarFase;
    }

    // Fase Principal -------------------------------------------------------------------------------------------------

    // Mantenimiento --------------------------------------------------------------------------------------------------

    IEnumerator Mantenimiento() {

        Debug.Log("Mantenimiento");
        yield return new WaitForSeconds(ENTRETIEMPO);

        if(turno == Turno.JUGADOR) {

            StartCoroutine(PreguntarJugarCarta());
        }
        
        yield return new WaitUntil(GetContinuarFase);

        EnderezarTierras();
    }

    void EnderezarTierras() {

        Debug.Log("Enderezar Tierras");
        faseActual = Fase.ROBO;
    }

    public void ContinuarTurno() {

        cajaDialogo.SetActive(false);
        continuarFase = true;
    }

    bool GetContinuarFase() {

        return continuarFase;
    }

    public void JugarInterrupcion() {

        jugador.permitidoJugarCartas = true;
        cajaDialogo.SetActive(false);
    }

    // Mantenimiento --------------------------------------------------------------------------------------------------

    //Mulligan --------------------------------------------------------------------------------------------------------

    void Mulligan() {

        StartCoroutine(RobarCartas(mulligan));
    }

    public void DevolverManoInicial() {

        cajaDialogo.SetActive(false);

        StartCoroutine(DevolverCartas(mulligan));
    }

    IEnumerator DevolverCartas(int cantidad) {

        for(int i=cantidad; i>=1; i--) {

            yield return new WaitForSeconds(TIEMPO_ROBO);
            
            GameObject cartaEnMano = manoJugador.transform.GetChild(i-1).gameObject;

            jugador.DevolverCartaAlMazo(CartaDesdeGameObject(cartaEnMano));

            Destroy(cartaEnMano);
        }

        jugador.Barajar();

        botonAceptar.GetComponent<Button>().onClick.RemoveAllListeners();
        botonCancelar.GetComponent<Button>().onClick.RemoveAllListeners();

        yield return RobarCartas(--mulligan);
    }

    Carta CartaDesdeGameObject(GameObject cartaGameObject) {

        Carta carta;

        carta = cartaGameObject.GetComponent<MostrarDatosCarta>().carta;

        return carta;
    }

    public void QuedarMano() {

        cajaDialogo.SetActive(false);
        quedarMano = true;
        faseActual = Fase.MANTENIMIENTO;

        botonAceptar.GetComponent<Button>().onClick.RemoveAllListeners();
        botonCancelar.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    //Mulligan --------------------------------------------------------------------------------------------------------

    //Dialogo ---------------------------------------------------------------------------------------------------------

    //Modificar Dialogo
    //Motivo 1 -> Mulligan

    void ModificarCajaDialogo(int motivo) {

        TMPro.TMP_Text textoDialogo = cajaDialogo.transform.GetChild(0).gameObject.GetComponent<TMPro.TMP_Text>(); //Texto de Pregunta
        TMPro.TMP_Text textoAceptar = botonAceptar.transform.GetChild(0).GetComponent<TMPro.TMP_Text>(); //Boton Aceptar
        TMPro.TMP_Text textoCancelar = botonCancelar.transform.GetChild(0).GetComponent<TMPro.TMP_Text>(); //Boton Cancelar

        switch(motivo) {

            case 0:
                Debug.Log("Se ha muerto");
                break;

            case 1:

                textoDialogo.text = "¿Desea hacer Mulligan a " + (mulligan-1) + "?";

                textoAceptar.text = "Hacer Mulligan";
                botonAceptar.GetComponent<Button>().onClick.AddListener(delegate { DevolverManoInicial(); });
 
                textoCancelar.text = "Quedar Mano";
                botonCancelar.GetComponent<Button>().onClick.AddListener(delegate { QuedarMano(); });

                break;

            case 2:

                textoDialogo.text = "¿Desea jugar alguna carta?";

                textoAceptar.text = "Jugar Carta";
                botonAceptar.GetComponent<Button>().onClick.AddListener(delegate { JugarInterrupcion(); });

                textoCancelar.text = "Continuar";
                botonCancelar.GetComponent<Button>().onClick.AddListener(delegate { ContinuarTurno(); });

                break;
        }
    }

    IEnumerator PreguntarMulligan() {

        yield return new WaitForSeconds(ENTRETIEMPO);

        ModificarCajaDialogo(1);

        cajaDialogo.SetActive(true);
    }

    IEnumerator PreguntarJugarCarta() {

        yield return new WaitForSeconds(ENTRETIEMPO);

        ModificarCajaDialogo(2);

        cajaDialogo.SetActive(true);
    }

    //Dialogo ---------------------------------------------------------------------------------------------------------
}

/*
1. Elegir quién va primero 
2. Mulligan
3. Fase de Mantenimiento
4. Fase de Robo
5. Fase Principal
6. Fase de Combate
7. Segunda Principal
8. Fin de Turno
9. Volver a la 3 para el otro jugador
*/