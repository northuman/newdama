using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{

    public int vida;
    public int[] mana; //BLANCO, NEGRO, ROJO, VERDE
    public List<Carta> barajaOriginal = new List<Carta>();
    public List<Carta> barajaPartida;
    public List<Carta> mano;    //Ahora son cartas; cambiar prefab!!
    public List<CartasJugadas> cementerio;
    public List<CartasJugadas> pila;
    public List<CartasJugadas> tierras;
    public List<CartasJugadas> batalla;
    public GameObject CartaJugada;//solo para probar
    public static List<Carta> staticbarajaPartida = new List<Carta>();
    public static int tamanyoBaraja;   //solo para probar
    public int x;//solo para probar
    public GameObject[] Clones;
    public GameObject Mano;

    //bool tierraJugada = false;

    public Jugador( List<Carta> baraja)
    {
        vida = 20;
        barajaOriginal = baraja;
        barajaPartida = new List<Carta>();
        mano = new List<Carta>();
        cementerio = new List<CartasJugadas>();
        pila    = new List<CartasJugadas>();
        tierras = new List<CartasJugadas>();
        batalla = new List<CartasJugadas>();
    }

    public void crearBarajaPartida()
    {
        barajaPartida = barajaOriginal.ToList();
        barajaPartida.Randomizar();
    }

    //hay que crear la barajaOriginal para poder probar

    //public void enderezoInicial()

    public bool robarCarta(int num){
        bool robar = false;
        if(barajaPartida.Count>=num){
            for(int i = 0; i<num;i++){
                mano.Add(barajaPartida[barajaPartida.Count-1]);
                barajaPartida.RemoveAt(barajaPartida.Count-1);
            }
            robar = true;
        }
        return robar;
    }


    // Start is called before the first frame update
    void Start()
    {
        x = 0;
        tamanyoBaraja = 40;
        for(int i = 0; i < tamanyoBaraja; i++)
        {        
            x = Random.Range(1,6);
            barajaOriginal.Add(new Carta(CartaDatabase.listaCartas[x]));
        }
        crearBarajaPartida();

        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {

        staticbarajaPartida = barajaPartida;
    }
    IEnumerator StartGame()
    {
        for(int i = 0; i <=6; i++) //Aquí está la cantidad de cartas que se mostrará en la mano al iniciar 
        {
            yield return new WaitForSeconds(0.5f);

            Instantiate(CartaJugada, transform.position, transform.rotation);
        }
    }
}
