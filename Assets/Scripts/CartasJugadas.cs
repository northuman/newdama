using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartasJugadas : MonoBehaviour
{
    public GameObject Mano;
    public GameObject CartaJugada;
    public Carta carta;
    public int fuerzaActual;
    public int resistenciaActual;
    public bool mareo;
    public bool girada;
    public List<Carta> encantamientos;


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
        Mano = GameObject.Find("Mano");
        CartaJugada.transform.SetParent(Mano.transform);
        CartaJugada.transform.localScale = Vector3.one;
        CartaJugada.transform.position = new Vector3(transform.position.x, transform.position.y, -48);
        //CartaJugada.transform.eulerAngles = new Vector3(25, 0, 0);     
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
