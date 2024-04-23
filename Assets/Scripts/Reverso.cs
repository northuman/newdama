using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reverso : MonoBehaviour
{
    public GameObject ReversoCarta;
    void Start()
    {
        
    }

    //Comprueba si la carta esta girada o no
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
