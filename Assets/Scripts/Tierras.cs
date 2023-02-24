using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void AnyadirMana(Carta carta) { //Arreglar

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

    public bool SuficienteMana(Activado efecto) { 

        bool manaSuficiente = true;
        int[] auxMana = (int[])mana.Clone();

        //Restar todo el mana especifico
        for(int i=1; i<5; i++) {

            if(auxMana[i] < efecto.costeActivacion[i]) {

                manaSuficiente = false;
                break;
            }

            else {

                auxMana[i] -= efecto.costeActivacion[i];
            }
        }

        //Comprobar si se puede pagar el mana generico
        if(manaSuficiente) {

            int manaGenerico = efecto.costeActivacion[0];
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

    //IA -----------------------------------------------------------------------------------

    public List<Arrastrable> TierrasSimplesEnMesa() {

        List<Arrastrable> simplesEnMesa = new List<Arrastrable>();
        Arrastrable arrastrable = null;
        Carta carta = null;

        for(int i=0; i<tierras.Count; i++) {

            arrastrable = tierras[i];
            carta = arrastrable.GetCarta();

            if(arrastrable && arrastrable.tipoCarta == Arrastrable.TipoCarta.TIERRA && !arrastrable.cartaGirada) {

                if(carta.cantidadMana.SequenceEqual(carta.cantidadMana2))
                    simplesEnMesa.Add(arrastrable);
            }
        }

        return simplesEnMesa;
    }

    public List<Arrastrable> TierrasDoblesEnMesa() {

        List<Arrastrable> doblesEnMesa = new List<Arrastrable>();
        Arrastrable arrastrable = null;
        Carta carta = null;

        for(int i=0; i<tierras.Count; i++) {

            arrastrable = tierras[i];
            carta = arrastrable.GetCarta();

            if(arrastrable && arrastrable.tipoCarta == Arrastrable.TipoCarta.TIERRA && !arrastrable.cartaGirada) {

                if(!carta.cantidadMana.SequenceEqual(carta.cantidadMana2))
                    doblesEnMesa.Add(arrastrable);
            }
        }

        return doblesEnMesa;
    }

    public bool Jugable(Carta carta) {

        bool jugable = false;
        int[] coste = (int[])carta.costeMana.Clone();
        List<Arrastrable> basicas = TierrasSimplesEnMesa();
        List<Arrastrable> dobles = TierrasDoblesEnMesa();
        Carta tierra = null;

        //Pagar coste especifico con tierras basicas

        for(int i=1; i<coste.Length; i++) {

            for(int j=0; j<basicas.Count; j++) {

                tierra = basicas[j].GetCarta();

                if(coste[i] == 0) { break; }
                else if(tierra.cantidadMana[i] > 0) {

                    coste[i] -= tierra.cantidadMana[i];
                    basicas.Remove(basicas[j]);
                }
            }
        }

        bool manaEspecificoPagado = true;
        bool manaGenericoPagado = false;

        for(int i=1; i<coste.Length; i++) {

            if(coste[i] > 0) {

                manaEspecificoPagado = false;
            }
        }

        //Pagar el coste especifico restante con dobles

        if(!manaEspecificoPagado) {

            for(int i=1; i<coste.Length; i++) {

                for(int j=0; j<dobles.Count; j++) {

                    tierra = dobles[j].GetCarta();

                    if(coste[i] == 0) { break; }
                    else if(tierra.cantidadMana[i] > 0 || tierra.cantidadMana2[i] > 0) {

                        coste[i] -= tierra.cantidadMana[i];
                        dobles.Remove(dobles[j]);
                    }
                }
            }

            manaEspecificoPagado = true;

            for(int i=1; i<coste.Length; i++) {

                if(coste[i] > 0) {

                    manaEspecificoPagado = false;
                }
            }
        }

        //Si se ha pagado todo el coste especifico

        if(manaEspecificoPagado) {

            //Pagar Generico empezando por basicas

            for(int i=0; i<basicas.Count; i++) {

                tierra = basicas[i].GetCarta();

                if(tierra.cantidadMana[3] == 2) {

                    coste[0] -= 2;
                    basicas.Remove(basicas[i]);
                }

                else {

                    coste[0]--;
                    basicas.Remove(basicas[i]);
                }

                if(coste[0] == 0) { 
                    
                    manaGenericoPagado = true;
                    break; 
                }
            }

            if(!manaGenericoPagado) {

                foreach(Arrastrable arr in dobles) {

                    tierra = arr.GetCarta();

                    coste[0]--;
                    dobles.Remove(arr);

                    if(coste[0] == 0) {

                        manaGenericoPagado = true;
                        break;
                    }
                }
            }
        }

        if(manaGenericoPagado && manaEspecificoPagado) {

            jugable = true;
        }

        return jugable;
    }

    public void PagarCoste(Carta carta) {

        int[] coste = (int[])carta.costeMana.Clone();
        List<Arrastrable> basicas = TierrasSimplesEnMesa();
        List<Arrastrable> dobles = TierrasDoblesEnMesa();
        Carta tierra = null;

        //Pagar coste especifico con tierras basicas

        for(int i=1; i<coste.Length; i++) {

            for(int j=0; j<basicas.Count; j++) {

                tierra = basicas[j].GetCarta();

                if(coste[i] == 0) { break; }
                else if(tierra.cantidadMana[i] > 0) {

                    coste[i] -= tierra.cantidadMana[i];
                    basicas[j].GirarCarta();
                    basicas.Remove(basicas[j]);
                }
            }
        }

        bool manaEspecificoPagado = true;
        bool manaGenericoPagado = false;

        for(int i=1; i<coste.Length; i++) {

            if(coste[i] > 0) {

                manaEspecificoPagado = false;
            }
        }

        //Pagar el coste especifico restante con dobles

        if(!manaEspecificoPagado) {

            for(int i=1; i<coste.Length; i++) {

                for(int j=0; j<dobles.Count; j++) {

                    tierra = dobles[j].GetCarta();

                    if(coste[i] == 0) { break; }
                    else if(tierra.cantidadMana[i] > 0 || tierra.cantidadMana2[i] > 0) {

                        coste[i] -= tierra.cantidadMana[i];
                        dobles[j].GirarCarta();
                        dobles.Remove(dobles[j]);
                    }
                }
            }

            manaEspecificoPagado = true;

            for(int i=1; i<coste.Length; i++) {

                if(coste[i] > 0) {

                    manaEspecificoPagado = false;
                }
            }
        }

        //Si se ha pagado todo el coste especifico

        if(manaEspecificoPagado) {

            //Pagar Generico empezando por basicas

            for(int i=0; i<basicas.Count; i++) {

                tierra = basicas[i].GetCarta();

                if(tierra.cantidadMana[3] == 2) {

                    coste[0] -= 2;
                    basicas[i].GirarCarta();
                    basicas.Remove(basicas[i]);
                }

                else {

                    coste[0]--;
                    basicas[i].GirarCarta();
                    basicas.Remove(basicas[i]);
                }

                if(coste[0] == 0) { 
                    
                    manaGenericoPagado = true;
                    break; 
                }
            }

            if(!manaGenericoPagado) {

                for(int i=0; i<dobles.Count; i++) {

                    tierra = dobles[i].GetCarta();

                    coste[0]--;
                    dobles[i].GirarCarta();
                    dobles.Remove(dobles[i]);

                    if(coste[0] == 0) {

                        manaGenericoPagado = true;
                        break;
                    }
                }
            }
        }
    }

    //IA -----------------------------------------------------------------------------------
}
