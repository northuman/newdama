using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*Script auxiliar para controlar que las cartas del mazo siempre estén del revés
*/
public class CartaPanelBaraja : MonoBehaviour
{
    public GameObject ReversoCarta;

    void Update()
    {
        ReversoCarta.SetActive(true);
    }
}
