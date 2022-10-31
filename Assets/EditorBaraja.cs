using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorBaraja : MonoBehaviour {

    public GameObject botonNuevaBaraja;
    public GameObject botonListo;
    public GameObject botonVolver;

    public static GameObject baraja;

    public ListaBarajas listaBarajas;
    public static bool creandoBaraja = false;

    void Start() {

        botonListo.SetActive(false);
    }

    public void VolverMenu() {

        SceneManager.LoadScene("Menu");
    }

    public void CrearBaraja() {

        botonListo.SetActive(true);
        botonVolver.SetActive(false);
        botonNuevaBaraja.SetActive(false);
        creandoBaraja = true;

        baraja = new GameObject("Baraja");
        baraja.AddComponent<Baraja>();
    }

    public static void AnyadirCartaBaraja(Carta carta) {

        Baraja baraja = EditorBaraja.baraja.GetComponent<Baraja>();
        baraja.cartas.Add(carta);

        GameObject cartaAnyadida = new GameObject(carta.nombreCarta);
        //cartaAnyadida.AddComponent<
    }
}
