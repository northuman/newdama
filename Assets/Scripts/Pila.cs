using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pila : MonoBehaviour {

    public static Stack<Arrastrable> pila = new Stack<Arrastrable>();
    public static int cantidadCartas = 0;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ResolverEfecto() {

        Arrastrable carta = pila.Pop();
        if(carta) {

            //Comprobar si hay mana disponible para jugarla

            //Si hay se juega
            carta.ColocarCarta();
        }

        else
            Debug.Log("Se ha perdido la carta de la pila");
    }
}
