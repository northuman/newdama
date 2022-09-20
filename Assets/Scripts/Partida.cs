using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Partida : MonoBehaviour {

    // Mulligan = 0, Mantenimiento = 1, Robo = 2, Principal = 4, Combate = 5, Principal2 = 6, Fin = 7
    bool turnoJugador = true; //Si false, turno oponente
    int faseActual = 0; 

    Baraja barajaJugador;
    GameObject manoJugador;

    Button botonFases;
    GameObject cajaDialogo;
    
    void Start() {
        
        barajaJugador = GameObject.Find("Baraja 1").GetComponent<Baraja>();
        manoJugador = GameObject.Find("Mano Jugador");

        botonFases = GameObject.Find("Boton Fases").GetComponent<Button>();
        cajaDialogo = GameObject.Find("Caja de Dialogo");
        cajaDialogo.SetActive(false);

        barajaJugador.Barajar();

        StartCoroutine(RobarManoInicialJugador());    
    }

    IEnumerator RobarManoInicialJugador() {

        for(int i=7; i>0; i--) {

            yield return new WaitForSeconds(0.5f);
            barajaJugador.RobarCarta();
        }

        HacerPregunta("¿Desea hacer Mulligan?");

        yield return new WaitForSeconds(2);
        cajaDialogo.SetActive(true);
    }

    public void DevolverManoInicial() {

        StartCoroutine(DevolverCartasMulligan());
    }

    IEnumerator DevolverCartasMulligan() {

        for(int i=7; i>0; i--) {

            yield return new WaitForSeconds(0.5f);
            
            GameObject cartaEnMano = manoJugador.transform.GetChild(i-1).gameObject;

            barajaJugador.AnyadirCarta(CartaDesdeGameObject(cartaEnMano));

            Destroy(cartaEnMano);
        }
    }

    Carta CartaDesdeGameObject(GameObject cartaGameObject) {

        Carta carta;

        carta = cartaGameObject.GetComponent<MostrarDatosCarta>().carta;

        return carta;
    }

    void Update() {
        
        
    }

    void HacerPregunta(string pregunta) {

        //El hijo 0 de la Caja de Dialogo debe ser el Texto de la Pregunta
        TMPro.TMP_Text texto = cajaDialogo.transform.GetChild(0).gameObject.GetComponent<TMPro.TMP_Text>();
        texto.text = pregunta;
    }

    public void QuedarMano() {

        cajaDialogo.SetActive(false);
    }

    public void Mulligan() {


    }
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