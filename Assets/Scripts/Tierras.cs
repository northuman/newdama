using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tierras : MonoBehaviour {
   
    public List<Arrastrable> tierras = new List<Arrastrable>();
    public GameObject contadorMana;
    
    //total de mana
    int[] mana;

    //   in ro bl ve ne
    //int[0][0][0][0][0]
    void Start() {

        mana = new int[5];
    }

    public void JugarTierra(Arrastrable tierra) {

        tierras.Add(tierra);
    }

    public void AnyadirMana(Carta carta) {

        for(int i=0; i<5; i++) {

            mana[i] += carta.cantidadMana[i];
            TMPro.TMP_Text text = contadorMana.transform.GetChild(i).GetComponent<TMPro.TMP_Text>();
            text.text = mana[i].ToString();
        }
    }

    public void ActualizarMana() {

        if(contadorMana) {
        
            for(int i=0; i<5; i++) {

                TMPro.TMP_Text text = contadorMana.transform.GetChild(i).GetComponent<TMPro.TMP_Text>();
                text.text = mana[i].ToString();
            }
        }
    }

    public int ManaSinUsar() {

        int manaSinUsar = 0;

        for(int i=0; i<mana.Length; i++) {

            manaSinUsar += mana[i];
        }

        return manaSinUsar;
    }

    public void ReiniciarContador() {

        for(int i=0; i<mana.Length; i++) {

            mana[i] = 0;
        }

        ActualizarMana();
    }

    public bool SuficienteMana(Carta carta) {

        bool manaSuficiente = true;
        int[] auxMana = (int[])mana.Clone();

        //Restar todo el mana especifico
        for(int i=1; i<5; i++) {

            if(auxMana[i] < carta.costeMana[i]) {

                manaSuficiente = false;
                break;
            }

            else {

                auxMana[i] -= carta.costeMana[i];
            }
        }

        //Comprobar si se puede pagar el mana generico
        if(manaSuficiente) {

            int manaGenerico = carta.costeMana[0];
            int mayorCantidad = 0;
            int mayorPosicion = -1;

            while(manaGenerico > 0) {

                for(int i=0; i<5; i++) {

                    if(auxMana[i] > mayorCantidad) {

                        mayorPosicion = i;
                        mayorCantidad = auxMana[i];
                    }
                }

                if(mayorPosicion == -1) {

                    manaSuficiente = false;
                    break;
                }

                auxMana[mayorPosicion]--;
                manaGenerico--;
                mayorCantidad = 0;
                mayorPosicion = -1;
            }
        }

        if(manaSuficiente) {

            mana = (int[])auxMana.Clone();
            ActualizarMana();
        }

        return manaSuficiente;
    }

    public bool SuficienteMana(List<Arrastrable> cartas) {

        bool manaSuficiente = false;

        for(int i=0; i<cartas.Count; i++) {

            Carta carta = cartas[i].GetComponent<MostrarDatosCarta>().carta;

            if(SuficienteMana(carta)) {

                manaSuficiente = true;
                break;
            }
        }

        return manaSuficiente;
    }
}
