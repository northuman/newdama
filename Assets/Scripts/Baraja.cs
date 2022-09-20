using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baraja : MonoBehaviour {

    public List<Carta> cartas = new List<Carta>();
    public GameObject prefabCarta;
    GameObject cartaRobada;

    public void Barajar() {

        int j;
        Carta aux;

        //Fisher-Yates para desordenar el array
        for(int i = cartas.Count-1; i > 0; i--) {

            j = Random.Range(0, i);
            aux = cartas[i];
            cartas[i] = cartas[j];
            cartas[j] = aux;
        }
    }

    public void RobarCarta() {

        Carta datosCartaRobada = null;

        if(cartas.Count == 0) {return;}

        datosCartaRobada = cartas[0];
        cartas.RemoveAt(0);

        //Crear un prefab
        cartaRobada = Instantiate(prefabCarta, new Vector3(0, 0, 0), Quaternion.identity);

        //Cambiar los datos de la carta
        cartaRobada.GetComponent<MostrarDatosCarta>().carta = datosCartaRobada;

        //Colocar en la mano del jugador
        cartaRobada.transform.SetParent(GameObject.Find("Mano Jugador").transform);
    }
}
