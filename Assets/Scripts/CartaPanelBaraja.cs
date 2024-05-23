using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*Script auxiliar para controlar que las cartas del mazo siempre estén del revés
*/
public class CartaPanelBaraja : MonoBehaviour
{
    public GameObject ReversoCarta;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ReversoCarta.SetActive(true);
    }
}
