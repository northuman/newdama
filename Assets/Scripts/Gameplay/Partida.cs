using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UtilCartas;

/*
* Controla el flujo de la partida, aquí va el sistema de turnos.
* 
*/

public class Partida : MonoBehaviour
{
    public enum OrdenJugadores { JUGADOR, OPONENTE };
    public Jugador jugador;
    public Jugador oponente;
    List<Jugador> jugadores;
    bool turno = false; //0 Turno jugador; 1 Turno oponente
    int ganador = 0;

    public void GenerarPrioridadJugador()
    {
        turno = Convert.ToBoolean(NumAleatorio(0, 1));
        turno = false; // fuerza el turno del jugador para probar

        jugadores = new List<Jugador>();

        //Primero jugador
        if (!turno)
        {
            jugadores.Add(jugador);
            jugadores.Add(oponente);
            //Primero oponente
        }
        else
        {
            jugadores.Add(oponente);
            jugadores.Add(jugador);
        }
    }

    void Start()
    {
        AccionesPrevias();
    }

    public void AccionesPrevias()
    {
        GenerarPrioridadJugador();

        for (int i = 0; i < 2; i++)
        {
            jugadores[i].RellenarBaraja();
            jugadores[i].CrearBarajaPartida();
            jugadores[i].RobarCarta(7);
        }

        //FORZAR CARTA DEL OPONENTE PARA PROBAR ---------------------
        ForzarCartaOponente();
    }

    public void ForzarCartaOponente()
    {
        if (oponente.mano.Count > 0)
        {
            var cartaPrueba = jugadores[1].mano[0];
            oponente.mano.RemoveAt(0);
            oponente.GetComponent<CartasJugadas>();
            Debug.Log("Carta de prueba añadida al campo del oponente: " + cartaPrueba.nombreCarta);
        }
    }

    public void BuclePartida() //SE DEBE DE TENER EN CUENTA EL ATRIBUTO DESTELLO (que puede usarse en cualquier momento/ cualquier fase)
    {
        while (ganador == 0/*jugador.vida >= 0 && oponente.vida >= 0 && jugador.biblioteca.Count >= 0 && oponente.biblioteca.Count >= 0*/)
        {
            //Fase inicio
            FaseInicio();
            if (ganador == 0)
            {
                //Fase principal 1

                //Fase combate

                //Fase principal 2

                //Fase final
            }
        }
        ;
    }

    public void FaseInicio()
    {
        //Enderezar cartas giradas
        //jugadores[0].enderezoInicial();

        //Mantenimiento (efectos etc)


        bool falloRobar;
        //Robar 1, si no puede pierde
        falloRobar = jugadores[0].RobarCarta(1);
        if (falloRobar)
        {
            if (!turno) ganador = 2;
            else ganador = 1;
        }
    }

    public void FasePrincipal()
    {
        //Jugar carta

        
    }

    public void FaseCombate()
    {
        //Activar habilidades principio combate

        //Declaracion atacantes jugador0->jugador1

        //Instantaneos Primero defensor

        //Declaracion bloqueadores jugador1

        //Instantaneos Primero defensor

        //Asignar danyo

        //Instantaneos Primero defensor

        //Resolver efectos fin de combate e instantaneos

    }

    public void PasoLimpieza()
    {
        //Reducir mano a 7 si lo supera
        //Eliminar desde el final de la mano
    }

    public void FaseFinal()
    {
        //Resolver efectos comienzo paso final

        //Paso limpieza
        PasoLimpieza();

        //Cambiar turno
        turno = !turno;
        jugadores.Intercambio(0, 1);
    }

}
