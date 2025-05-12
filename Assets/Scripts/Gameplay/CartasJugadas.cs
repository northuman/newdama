using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
*Script para controlar las cartas que se utilizan. 
*Cuando una Carta sale del mazo se convierte en CartaJugada para poder modificar sus estadísticas sin afectar a la carta original.
*/

public class CartasJugadas : MonoBehaviour
{
    public GameObject ManoJugador;
    public GameObject ManoOponente;
    public GameObject CartaJugada;
    public Carta carta;
    public int fuerzaActual;
    public int resistenciaActual;
    public bool mareo;
    public bool girada;
    public List<Carta> encantamientos;
    public int perteneceAJugador;
    public GameObject j1;
    public GameObject j2;


    public void Inicializar(Carta cartp)
    {
        carta = new Carta(cartp); 
        fuerzaActual = cartp.fuerza;
        resistenciaActual = cartp.resistencia;
        encantamientos = new List<Carta>();
        //mareo = false;
        //girada = false;
    }

    public void RotarCarta(){
        if (this != null){
            if(girada == false){
                this.transform.Rotate(0 ,0 ,-90);
                girada = true;
            }
            else{
                this.transform.Rotate(0,0,90);
                girada = false;
            }
        }
    }

    void Start()
    {
        //Asigno los jugadores
        j1 = GameObject.Find("Jugador");
        j2 = GameObject.Find("Oponente");
        //Asigno los paneles 
        ManoJugador = GameObject.Find("ManoJugador");
        ManoOponente = GameObject.Find("ManoOponente");

        //Rotar las cartas del oponente, innecesario porque no se ven
        if(perteneceAJugador== j2.GetComponent<Jugador>().id)
        {
            CartaJugada.GetComponent<Reverso>().transform.Rotate(0, 0, 180);      
        }

        //Coloca las cartas en la mano correspondiente según a quién pertenecen
        if(perteneceAJugador== j1.GetComponent<Jugador>().id)
        {
            CartaJugada.transform.SetParent(ManoJugador.transform);
        }
        else if(perteneceAJugador== j2.GetComponent<Jugador>().id)
        {
            CartaJugada.transform.SetParent(ManoOponente.transform);   
        }
    }
}
