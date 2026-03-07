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
    public enum Fases { INICIO, PRINCIPAL_1, COMBATE, PRINCIPAL_2, FINAL };
    public Jugador jugador;
    public Jugador oponente;
    List<Jugador> jugadores;
    bool turno = false; //false = turno jugador; true = turno oponente
    public Jugador jugadorActivo => turno ? oponente : jugador;
    int ganador = 0;
    public Fases faseActual; //veo en que momento de partida estamos.


    void Start()
    {
        AccionesPrevias();
        //iniciar turno
        IniciarTurno();
    }


    public void GenerarPrioridadJugador()
    {
        turno = Convert.ToBoolean(NumAleatorio(0, 1));
        turno = false; // fuerza el turno del jugador para probar

        jugadores = new List<Jugador> { jugador, oponente };
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
            Debug.Log("Carta de prueba añadida al campo del oponente: " + cartaPrueba.nombreCarta);
        }
    }

    //SISTEMA DE TURNOS Y FASES (MAQUINA DE ESTADOS)

    public void IniciarTurno()
    {
        Debug.Log("--- Comienza el turno de: " + (turno ? "Oponente" : "jugador") + " ---");
        faseActual = Fases.INICIO;
        FaseInicio();
    }

    public void AvanzarFase()
    {
        if (ganador != 0) return; //si el juego acabó no se hace nada

        switch (faseActual)
        {
            case Fases.INICIO:
                faseActual = Fases.PRINCIPAL_1;
                FasePrincipal(1);
                break;

            case Fases.PRINCIPAL_1:
                faseActual = Fases.COMBATE;
                FaseCombate();
                break;

            case Fases.COMBATE:
                faseActual = Fases.PRINCIPAL_2;
                FasePrincipal(2);
                break;

            case Fases.PRINCIPAL_2:
                Debug.Log(">>> Intentando entrar a fase final...");
                faseActual = Fases.FINAL;
                FaseFinal();
                break;

            case Fases.FINAL:
                faseActual = Fases.FINAL;
                IniciarTurno();
                break;
        }
    }

    // LOGICA DE CADA FASE
    public void FaseInicio()
    {
        Debug.Log("1. Fase de inicio (enderezco, mantenimiento, robo");
        //1. Enderezar cartas giradas
        //jugadorActivo.EnderezarCartas();

        //2. Mantenimiento (Upkeep)

        //3. Robar (Draw)
        bool falloRobar = jugadorActivo.RobarCarta(1);
        
        if (!falloRobar)
        {
            ganador = turno ? 1 : 2; //si no se puede robar, pierte.
            Debug.Log("Jugador " + ganador + " ha ganado por deckeo");
        }

        // Cuando acabe la animación de robar, el jugador debería poder darle al botón 
        // de "Avanzar Fase" para pasar a la Fase Principal 1.
    }

    public void FasePrincipal(int numeroFase)
    {
        Debug.Log($"{numeroFase} Fase principal {numeroFase} (jugar tierras, criaturas, conjuros)");
        // Aquí el juego se detiene y espera a que el jugador arrastre cartas a la mesa.
        // Solo avanzará cuando pulse el botón de "AvanzarFase()".

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

        Debug.Log("3. FASE DE COMBATE (Declarar Atacantes, Bloqueadoras, Daño)");
        // El combate es un mini-bucle complejo, pero la base está aquí.

    }


    public void FaseFinal()
    {
        Debug.Log("4. FASE FINAL (Paso final y limpieza)");
        Jugador jugadorActivo = turno ? oponente : jugador;

        // Paso Limpieza: Si hay más de 7 cartas, obligar a descartar
        if (jugadorActivo.mano.Count > 7)
        {
            Debug.Log("El jugador tiene demasiadas cartas. Debe descartar.");
            // Aquí activaríamos un estado en la UI para obligarle a descartar antes de cambiar de turno
        }

        // El turno termina automáticamente y llamará a AvanzarFase() para reiniciar el bucle
        AvanzarFase();
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
        };
    }

}
