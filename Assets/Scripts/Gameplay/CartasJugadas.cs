using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
*Script para controlar las cartas que se utilizan. 
*Cuando una Carta sale del mazo se convierte en CartaJugada para poder modificar sus estadísticas sin afectar a la carta original.
*/

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
    public bool defensor; //si es defensor no puede atacar
    public bool vigilancia; //si tiene vigilancia no se gira al atacar
    public bool indestructible; //si es indestructible no puede morir
    public bool toqueMortal; //si tiene toque mortal, cualquier daño que haga es letal
    public bool destello; //si tiene destello se puede jugar en cualquier momento
    public bool vuelo; //si tiene vuelo solo puede ser bloqueada por cartas con vuelo
    public bool arrolla; //si tiene arrolla, el exceso de daño que haga al atacar se lo hace al jugador
    public bool vinculoVital; //si tiene vínculo vital, recupera vida igual al daño que haga
    public List<Carta> encantamientos;
    public int perteneceAJugador;
    public GameObject j1;
    public GameObject j2;


    public void Inicializar(Carta cartp)
    {
        carta = new Carta(cartp); 
        fuerzaActual = cartp.fuerza;
        resistenciaActual = cartp.resistencia;
        encantamientos = new List<Carta>();
        mareo = true;
        girada = false;
        defensor = false;
        vigilancia = false;

        Debug.Log($"[Inicializar] Carta '{cartp.nombreCarta}'");

        //Aplicar atributos iniciales
        foreach (var atributo in cartp.atributos) {
            atributo.aplicarAtributo(this);
        }
    }

    public void RotarCarta(){
        if (this != null){
            if(girada == false){
                this.transform.Rotate(0 ,0 ,-90);
                girada = true;
            }
            else{
                this.transform.Rotate(0,0,90);
                girada = false;
            }
        }
    }

    void Start()
    {
        //Asigno los jugadores
        j1 = GameObject.Find("Jugador");
        j2 = GameObject.Find("Oponente");
        //Asigno los paneles 
        ManoJugador = GameObject.Find("ManoJugador");
        ManoOponente = GameObject.Find("ManoOponente");

        //Rotar las cartas del oponente, innecesario porque no se ven
        if(perteneceAJugador== j2.GetComponent<Jugador>().id)
        {
            CartaJugada.GetComponent<Reverso>().transform.Rotate(0, 0, 180);      
        }

        //Coloca las cartas en la mano correspondiente según a quién pertenecen
        if(perteneceAJugador== j1.GetComponent<Jugador>().id)
        {
            CartaJugada.transform.SetParent(ManoJugador.transform);
        }
        else if(perteneceAJugador== j2.GetComponent<Jugador>().id)
        {
            CartaJugada.transform.SetParent(ManoOponente.transform);   
        }
    }

    /*METODOS DE ATAQUE Y BLOQUEO QUE HACE FALTA USAR EN LA PARTIDA*/
    public void Atacar(CartasJugadas objetivo)
    {
        if (!mareo && !defensor) //si esta mareada no puede atacar, si tiene atributo defensor no puede atacar
        {
            int danioAsignado;

            if (toqueMortal)
            {
                danioAsignado = objetivo.resistenciaActual;
            }
            else
            {
                danioAsignado = this.fuerzaActual;
            }

            //daño a la carta bloqueadora
            if (objetivo != null)
            {
                int danioRealizado = Mathf.Min(danioAsignado, objetivo.resistenciaActual);
                objetivo.RecibirDanio(danioAsignado);

                if (vinculoVital)
                {
                    TratarVida(0, danioRealizado);
                }

                if (arrolla && danioAsignado > danioRealizado)
                {
                    int excesoDanio = danioAsignado - danioRealizado;
                    TratarVida(1, excesoDanio);

                    if (vinculoVital)
                    {
                        TratarVida(0, excesoDanio);
                    }
                }
            }

            else //daño directo al jugador
            {
                TratarVida(1, danioAsignado);

                if (vinculoVital)
                {
                    TratarVida(0, danioAsignado);
                }
            }


            if (!vigilancia)
            {
                girada = true; // Al atacar, la carta se gira
            }

        }
    }

    public void RecibirDanio(int danio)
    {
        if ( !this.indestructible)
        {
            resistenciaActual -= danio;

            if (resistenciaActual <= 0)
            {
                DestruirCarta();
            }
        }
       
    }

    public void DestruirCarta()
    {
        Debug.Log($"Carta {carta.nombreCarta} ha muerto");
        //LLEVAR AL CEMENTERIO
        if (perteneceAJugador == j1.GetComponent<Jugador>().id)
        {
            j1.GetComponent<Jugador>().cementerio.Add(this);
        }
        else if (perteneceAJugador == j2.GetComponent<Jugador>().id)
        {
            j2.GetComponent<Jugador>().cementerio.Add(this);
        }
    }



    //FALTA LA LOGICA DEL METODO DE BLOQUEO
    public void Bloquear(CartasJugadas objetivo)
    {
        if (!girada) // si esta girada no puede bloquear
        {
            if ((objetivo.vuelo && this.vuelo) || !objetivo.vuelo)
            {
                //hacer logica de bloqueo
            }
        }
    }

    public void TratarVida(int caso, int danio)
    {
        if (caso == 0) //vinculo vital
        {
            //le sumamos al jugador que controla la carta el daño realizado
            if (perteneceAJugador == j1.GetComponent<Jugador>().id)
            {
                j1.GetComponent<Jugador>().vida += danio;
            }
            else if (perteneceAJugador == j2.GetComponent<Jugador>().id)
            {
                j2.GetComponent<Jugador>().vida += danio;
            }
        }
        else if (caso == 1) //arrolla y restar vida al oponente
        {
            //le restamos el exceso de daño al jugador contrario
            if (perteneceAJugador == j1.GetComponent<Jugador>().id)
            {
                j2.GetComponent<Jugador>().vida -= danio;
            }
            else if (perteneceAJugador == j2.GetComponent<Jugador>().id)
            {
                j1.GetComponent<Jugador>().vida -= danio;
            }
        }
    }
}
