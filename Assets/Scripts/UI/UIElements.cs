using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
* Script para los elementos de la interfaz
*/

public class UIElements : MonoBehaviour
{
    public int vida;
    public int[] manaJugador;
    public int manaB;
    public int manaN;
    public int manaR;
    public int manaV;

    public GameObject jugador1;
    public GameObject panelMana;

    public TMP_Text vida1;
    public TMP_Text manaBlanco;
    public TMP_Text manaNegro;
    public TMP_Text manaRojo;
    public TMP_Text manaVerde;
    

    /*
    Separa el array de maná en valores individuales para que se pueda mostrar en la interfaz
    */
    public void SepararMana()
    { 
        if (jugador1.GetComponent<Jugador>().mana != null){
            
            manaB = jugador1.GetComponent<Jugador>().mana[0];
            manaN = jugador1.GetComponent<Jugador>().mana[1];
            manaR = jugador1.GetComponent<Jugador>().mana[2];
            manaV = jugador1.GetComponent<Jugador>().mana[3];
        }
    }

    /*
    Este método hace que se muestre el maná de forma dinámica. 
    Cuando es 0 no se muestra la interfaz
    */
    public void ShowMana()
    {
        if(manaB == 0){
            panelMana.transform.Find("Blanco").gameObject.SetActive(false);
        }else{
            panelMana.transform.Find("Blanco").gameObject.SetActive(true);
        }
        if(manaN == 0){
            panelMana.transform.Find("Negro").gameObject.SetActive(false);
        }else{
            panelMana.transform.Find("Negro").gameObject.SetActive(true);
        }
        if(manaR == 0){
            panelMana.transform.Find("Rojo").gameObject.SetActive(false);
        }else{
            panelMana.transform.Find("Rojo").gameObject.SetActive(true);
        }
        if(manaV == 0){
            panelMana.transform.Find("Verde").gameObject.SetActive(false);
        }else{
            panelMana.transform.Find("Verde").gameObject.SetActive(true);
        }
        
        //Asigna los valores a las casillas de la interfaz
        manaBlanco.text = "" + manaB;
        manaNegro.text = "" + manaN;
        manaRojo.text = "" + manaR;
        manaVerde.text = "" + manaV;
    }
    
    void Start()
    {
        jugador1 = GameObject.Find("Jugador");
    }

    
    void Update()
    {
        //Los elementos de la interfaz se actualizan cada frame
        vida = jugador1.GetComponent<Jugador>().vida;
        vida1.text = "" + vida;
        SepararMana();
        ShowMana();
        
    }
}
