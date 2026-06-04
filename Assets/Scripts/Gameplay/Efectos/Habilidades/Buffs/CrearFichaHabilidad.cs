using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearFichaHabilidad : IHabilidad
{
    private int cantidad;
    private int tipo;
    private string nombreCarta;
    private string color;
    private int fuerza;
    private int resistencia;
    private Carta cartaFicha;
    private GameObject go;

    public CrearFichaHabilidad(int cantidad, string nombreCarta, int tipo = 1, string color = "cualquiera", int fuerza = 1, int resistencia = 1)
    {
        this.cantidad = cantidad;
        this.tipo = tipo;
        this.nombreCarta = nombreCarta;
        this.color = color;
        this.fuerza = fuerza;
        this.resistencia = resistencia;
    }

    public void ActivarHabilidad(CartasJugadas cartajug, CartasJugadas oponente = null)
    {
        
        for (int i = 0; i < cantidad; i++)
        {
            cartaFicha = new Carta(nombreCarta, tipo, "", null, color, fuerza, resistencia, "", "");
            go = new GameObject("Ficha");
            CartasJugadas ficha= go.AddComponent<CartasJugadas>();
            ficha.Inicializar(cartaFicha);

            if (cartajug.perteneceAJugador == cartajug.j1.GetComponent<Jugador>().id)
            {
                cartajug.j1.GetComponent<Jugador>().batalla.Add(ficha);
            }
            else
            {
                cartajug.j2.GetComponent<Jugador>().batalla.Add(ficha);
            }
        }
    }
}