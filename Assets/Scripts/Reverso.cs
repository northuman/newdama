using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script para controlar el reverso de la carta
public class Reverso : MonoBehaviour
{
    public GameObject ReversoCarta;
    void Start()
    {
        
    }

    void Update()
    {
        
        if(MostrarCarta.staticReverso == true)
        {
            ReversoCarta.SetActive(true);
        }
        else
        {
            ReversoCarta.SetActive(false);
        }
        
    }
}
