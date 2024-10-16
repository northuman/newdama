using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UtilCartas;


public class Partida : MonoBehaviour
{
    public enum OrdenJugadores {JUGADOR, OPONENTE};
    public Jugador jugador;
    public Jugador oponente;
    List<Jugador> jugadores;
    bool turno = false; //0 Turno jugador; 1 Turno oponente
    int ganador = 0;

    public void generarPrioridadJugador()
    {
        turno = Convert.ToBoolean(numAleatorio(0,1));
        turno = false; // fuerza el turno del jugador para probar

        jugadores = new List<Jugador>();

        //Primero jugador
        if(!turno){
            jugadores.Add(jugador);
            jugadores.Add(oponente);
            Debug.Log("Jugador es primero");
        //Primero oponente
        }else{
            jugadores.Add(oponente);
            jugadores.Add(jugador);
            Debug.Log("Oponente es primero");
        }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        // esto no lo puedo hacer en el start, tengo que hacer una función que rellene las barajas
        // start y update se tienen que quedar solo con llamadas a otras funciones

        Debug.Log("Start Partida");
        generarPrioridadJugador();
        jugadores[0].rellenarBaraja();
        jugadores[1].rellenarBaraja();
        jugadores[0].crearBarajaPartida();
        jugadores[1].crearBarajaPartida();
        jugadores[0].robarCarta(7);
        jugadores[1].robarCarta(7);   

    }

    public void partida()
    {
        while(ganador==0/*jugador.vida >= 0 && oponente.vida >= 0 && jugador.biblioteca.Count >= 0 && oponente.biblioteca.Count >= 0*/){
            //Fase inicio
            faseInicio();
            if(ganador==0){
                //Fase principal 1

                //Fase combate

                //Fase principal 2

                //Fase final
            }
        };
    }

    public void faseInicio()
    {
        //Enderezar cartas giradas
        //jugadores[0].enderezoInicial();

        //Mantenimiento (efectos etc)


        bool falloRobar;
        //Robar 1, si no puede pierde
        falloRobar = jugadores[0].robarCarta(1);
        if(falloRobar){
            if(!turno) ganador = 2;
            else       ganador = 1;
        } 
    }

    public void fasePrincipal(){
        //Jugar carta
    }

    public void faseCombate(){
        //Activar habilidades principio combate

        //Declaracion atacantes jugador0->jugador1

        //Instantaneos Primero defensor

        //Declaracion bloqueadores jugador1

        //Instantaneos Primero defensor

        //Asignar danyo

        //Instantaneos Primero defensor

        //Resolver efectos fin de combate e instantaneos

    }

    public void pasoLimpieza(){
        //Reducir mano a 7 si lo supera
        //Eliminar desde el final de la mano
        while(jugadores[0].mano.Count > 7){
            jugadores[0].eliminarCartaMano(jugadores[0].mano.Count);
        }
        //Reiniciar efectos y danyo en cartas
        jugadores[0].reiniciarEstadisticasBatalla();
    }

    public void faseFinal(){
        //Resolver efectos comienzo paso final

        //Paso limpieza
        pasoLimpieza();

        //Cambiar turno
        turno = !turno;
        jugadores.Intercambio(0,1);
    }

    //
    void Update()
    {
        
    }
}
