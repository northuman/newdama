using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pila : MonoBehaviour {

    public static Stack<Arrastrable> pila = new Stack<Arrastrable>();
    public static int cantidadCartas = 0;

    public void ResolverEfecto() {

        if(pila.Count > 0) {

            Arrastrable carta = pila.Pop();
            if(carta) {
                
                carta.ColocarCarta();
            }

            else
                Debug.Log("Se ha perdido la carta de la pila");
        }
    }

    public static void AnyadirCarta(Arrastrable carta) {

        pila.Push(carta);
        cantidadCartas++;
    }
}
