using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tierras : MonoBehaviour {
   
    public List<Arrastrable> tierras = new List<Arrastrable>();
    public GameObject contadorMana;
    
    //total de mana
    int[] mana;

    //mana disponible este turno
    int[] manaTemp;

    //   in ro bl ve ne
    //int[0][0][0][0][0]
    void Start() {

        mana = new int[5];
        manaTemp = new int[5];
        contadorMana = GameObject.Find("Mana Jugador");
    }

    public void JugarTierra(Arrastrable tierra) {

        tierras.Add(tierra);
        Carta datosTierra = tierra.gameObject.GetComponent<MostrarDatosCarta>().carta;
        
        for(int i=0; i<5; i++) {

            mana[i] += datosTierra.cantidadMana[i];
        }
    }

    public void EnderezarTierras() {

        for(int i=0; i<5; i++) {

            manaTemp[i] = mana[i];
        }
    }

    public void AnyadirMana(Carta carta) {

        for(int i=0; i<5; i++) {

            manaTemp[i] += carta.cantidadMana[i];
            TMPro.TMP_Text text = contadorMana.transform.GetChild(i).GetComponent<TMPro.TMP_Text>();
            text.text = manaTemp[i].ToString();
        }
    }

    void ActualizarMana(int i) {

        switch(i) {

            case 0:

                

                break;
        }
    }
}
