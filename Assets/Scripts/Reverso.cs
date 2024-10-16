using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script para controlar el reverso de la carta
public class Reverso : MonoBehaviour
{
    public GameObject ReversoCarta;
    public bool cartaGirada;


    /*
    Comprueba si la carta está girada o no, y le da la vuelta.
    Cambia la propiedad de girada de la carta
    */
    public void GirarCarta()
    {
        if(this.enabled == false)   //si está boca arriba
        {
            this.enabled = true;    
            ReversoCarta.SetActive(true);   //se pone boca abajo
            cartaGirada = true;
            ReversoCarta.GetComponentInParent<CartasJugadas>().girada = cartaGirada;
        }
        else    //si está boca abajo
        {
            this.enabled = false;
            ReversoCarta.SetActive(false);  //boca arriba
            cartaGirada = false;
            ReversoCarta.GetComponentInParent<CartasJugadas>().girada = cartaGirada;
        }
    }

    /*
    COntrola que las cartas del jugador salgan boca arriba al iniciar la partida
    */
    private void IniciarConReverso()
    {
        int perteneceA = -1;
        if(ReversoCarta.GetComponentInParent<CartasJugadas>() != null){
            perteneceA = ReversoCarta.GetComponentInParent<CartasJugadas>().perteneceAJugador;
            
            if(perteneceA == 1)
            {
                GirarCarta();
            }

        }
    }
    void Start()
    {
        IniciarConReverso();
    }

    void Update()
    {
        
    }
}
