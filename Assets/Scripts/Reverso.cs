using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script para controlar el reverso de la carta
public class Reverso : MonoBehaviour
{
    public GameObject ReversoCarta;

    public void GirarCarta()
    {
        if(this.enabled == false)
        {
            this.enabled = true;
            ReversoCarta.SetActive(true);
        }
        else
        {
            this.enabled = false;
            ReversoCarta.SetActive(false);
        }
    }

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
