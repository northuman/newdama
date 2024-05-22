using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartasJugadas : MonoBehaviour
{
    public GameObject ManoJugador;
    public GameObject ManoOponente;
    public GameObject CartaJugada;
    public Carta carta;
    public int fuerzaActual;
    public int resistenciaActual;
    public bool mareo;
    public bool girada;
    public List<Carta> encantamientos;
    public int perteneceAJugador;
    public GameObject j1;
    public GameObject j2;

    public CartasJugadas(Carta cartp){
        carta = cartp;
        fuerzaActual = cartp.fuerza;
        resistenciaActual = cartp.resistencia;
        mareo = true;
        girada = false;
        encantamientos = new List<Carta>();
    }

    // Start is called before the first frame update
    void Start()
    {
        j1 = GameObject.Find("Jugador");
        j2 = GameObject.Find("Oponente");

        ManoJugador = GameObject.Find("ManoJugador");
        ManoOponente = GameObject.Find("ManoOponente");

        if(perteneceAJugador== j2.GetComponent<Jugador>().id)
        {
            CartaJugada.transform.Rotate(180, 0, 0);
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(perteneceAJugador== j1.GetComponent<Jugador>().id)
        {
            CartaJugada.transform.SetParent(ManoJugador.transform);

        }
        else if(perteneceAJugador== j2.GetComponent<Jugador>().id)
        {
            CartaJugada.transform.SetParent(ManoOponente.transform);
            
            
        }
        //CartaJugada.transform.localScale = Vector3.one;
        //CartaJugada.transform.position = new Vector3(transform.position.x, transform.position.y, -48);
        //CartaJugada.transform.eulerAngles = new Vector3(25, 0, 0); 
    }
}
