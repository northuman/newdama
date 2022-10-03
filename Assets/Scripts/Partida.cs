using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Partida : MonoBehaviour {

    const float TIEMPO_ROBO = 0.5f;

    // Mulligan = 0, Mantenimiento = 1, Robo = 2, Principal = 4, Combate = 5, Principal2 = 6, Fin = 7
    //bool turnoJugador = true; //Si es false, turno del oponente
    static int faseActual = 0;
    static int mulligan = 7;
    public static bool quedarMano = false;

    static Baraja barajaJugador;
    static GameObject manoJugador;

    static Button botonFases;
    static GameObject cajaDialogo;
    static GameObject botonAceptar;
    static GameObject botonCancelar;
    
    void Start() {
        
        EmpezarPartida();
    }

    void EmpezarPartida() {

        barajaJugador = GameObject.Find("Baraja 1").GetComponent<Baraja>();
        manoJugador = GameObject.Find("Mano Jugador");

        botonFases = GameObject.Find("Boton Fases").GetComponent<Button>();
        cajaDialogo = GameObject.Find("Caja de Dialogo");
        botonAceptar = GameObject.Find("Boton Aceptar");
        botonCancelar = GameObject.Find("Boton Cancelar");
        cajaDialogo.SetActive(false);

        barajaJugador.Barajar();

        Mulligan();
    }

    IEnumerator RobarCartas(int cantidad) {

        GameObject eventSystem = GameObject.Find("EventSystem");
        eventSystem.SetActive(false);

        for(int i=cantidad; i>0; i--) {

            yield return new WaitForSeconds(TIEMPO_ROBO);
            barajaJugador.RobarCarta();
        }

        if(mulligan == 0) { quedarMano = true; }

        if(!quedarMano) {

            yield return PreguntarMulligan();
        }

        eventSystem.SetActive(true);
    }

    //Mulligan --------------------------------------------------------------------------------------------------------

    void Mulligan() {

        StartCoroutine(RobarCartas(mulligan));
    }

    IEnumerator PreguntarMulligan() {

        yield return new WaitForSeconds(1f);

        ModificarCajaDialogo(1);

        cajaDialogo.SetActive(true);
    }

    public void DevolverManoInicial() {

        cajaDialogo.SetActive(false);

        StartCoroutine(DevolverCartas(mulligan));
    }

    IEnumerator DevolverCartas(int cantidad) {

        for(int i=cantidad; i>=1; i--) {

            yield return new WaitForSeconds(TIEMPO_ROBO);
            
            GameObject cartaEnMano = manoJugador.transform.GetChild(i-1).gameObject;

            barajaJugador.AnyadirCarta(CartaDesdeGameObject(cartaEnMano));

            Destroy(cartaEnMano);
        }

        barajaJugador.Barajar();

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
        faseActual = 1;

        botonAceptar.GetComponent<Button>().onClick.RemoveAllListeners();
        botonCancelar.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    //Mulligan --------------------------------------------------------------------------------------------------------

    // Mantenimiento --------------------------------------------------------------------------------------------------

    void Mantenimiento() {

        Debug.Log("Mantenimiento");
    }

    // Mantenimiento --------------------------------------------------------------------------------------------------

    //Dialogo ---------------------------------------------------------------------------------------------------------

    //Modificar Dialogo
    //Motivo 1 -> Mulligan

    void ModificarCajaDialogo(int motivo) {

        switch(motivo) {

            case 0:
                Debug.Log("Se ha muerto");
                break;

            case 1:

                TMPro.TMP_Text textoDialogo = cajaDialogo.transform.GetChild(0).gameObject.GetComponent<TMPro.TMP_Text>(); //Texto de Pregunta
                textoDialogo.text = "¿Desea hacer Mulligan a " + (mulligan-1) + "?";

                TMPro.TMP_Text textoAceptar = botonAceptar.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
                textoAceptar.text = "Hacer Mulligan";
                botonAceptar.GetComponent<Button>().onClick.AddListener(delegate { DevolverManoInicial(); });


                TMPro.TMP_Text textoCancelar = botonCancelar.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
                textoCancelar.text = "Quedar Mano";
                botonCancelar.GetComponent<Button>().onClick.AddListener(delegate { QuedarMano(); });

                break;
        }
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