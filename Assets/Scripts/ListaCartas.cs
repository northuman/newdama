using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListaCartas : MonoBehaviour {

    public List<Carta> todas = new List<Carta>();
    public GameObject vistaPrevia;
    public GameObject prefabCarta;
    int principio = 0;
    int final = 7;

    void Start() {

        CargarCartas();
    }

    void CargarCartas() {

        foreach(Transform child in vistaPrevia.transform) {

            GameObject.Destroy(child.gameObject);
        }

        Carta datosCarta = null;

        for(int i=principio; i<todas.Count && i<=final; i++) {

            datosCarta = todas[i];

            GameObject carta = Instantiate(prefabCarta, new Vector3(0, 0, 0), Quaternion.identity);

            carta.transform.localScale = carta.transform.localScale * 1.5f;

            carta.GetComponent<MostrarDatosCarta>().carta = datosCarta;

            carta.transform.SetParent(vistaPrevia.transform);
        }
    }

    public void PaginaSiguiente() {

        if(final < todas.Count) {

            principio += 8;
            final += 8;
            CargarCartas();
        }

        else {

            Debug.Log("No hay más cartas");
        }
    }

    public void PaginaAnterior() {

        if(principio > 0) {

            principio -= 8;
            final -= 8;
            CargarCartas();
        }

        else {

            Debug.Log("Esta es la primera página");
        }
    }
}
