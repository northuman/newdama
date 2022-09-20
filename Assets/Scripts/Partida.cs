using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Partida : MonoBehaviour {

    Button botonFases;
    bool turnoJugador = true; //Si false, turno oponente

    // Mulligan = 0, Mantenimiento = 1, Robo = 2, Principal = 4
    //Combate = 5, Principal2 = 6, Fin = 7
    int faseActual = 0; 

    Baraja barajaJugador;
    
    void Start() {
        
        botonFases = GameObject.Find("Boton Fases").GetComponent<Button>();
        barajaJugador = GameObject.Find("Baraja 1").GetComponent<Baraja>();

        barajaJugador.Barajar();

        for(int i=7; i>0; i--) {

            barajaJugador.RobarCarta();
            EsperarSegundos(10);
        }
    }

    IEnumerator EsperarSegundos(int seg) {

        yield return new WaitForSeconds(seg);
    }

    void Update() {
        
        switch(faseActual) {

            case 0:
                //Devolver las cartas al mazo
                //Robar 7 nuevas
                break;
            default:
                Debug.Log("Se ha roto");
                break;
        }
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