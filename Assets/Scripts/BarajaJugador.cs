using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarajaJugador : MonoBehaviour
{
    /*
    public List<Carta> container = new List<Carta>();
    public int x;
    //public static int tamanyoBaraja;
    public List<Carta> baraja = new List<Carta>();
    public static List<Carta> staticBaraja = new List<Carta>();


    
    * Cartas del panel mazo que se muestra en el juego
    
    public GameObject cartaEnMazo1;
    public GameObject cartaEnMazo2;
    public GameObject cartaEnMazo3;
    public GameObject cartaEnMazo4;

    public GameObject CartaAMano;
    public GameObject[] Clones;
    public GameObject Mano;
    */


    void Start()
    {
        /*
        * Rellena la baraja con cartas aleatorias de la base de datos
        
        x = 0;
        tamanyoBaraja = 40;
        for(int i = 0; i < tamanyoBaraja; i++)
        {
            x = Random.Range(1,5);
            baraja[i] = new Carta(CartaDatabase.listaCartas[x]);
            //CartaDatabase.listaCartas[x];
        }

        StartCoroutine(StartGame());
        */
    }

    // Update is called once per frame
    void Update()
    {   
        /*
        staticBaraja = baraja;

        
        * Desactiva las cartas de la interfaz según queden menos cartas en el mazo para que
        * de la sensación de que se terminan.
        
        if(tamanyoBaraja < 30)
        {
            cartaEnMazo1.SetActive(false);
        }
        if(tamanyoBaraja < 20)
        {
            cartaEnMazo2.SetActive(false);
        }
        if(tamanyoBaraja < 5)
        {
            cartaEnMazo3.SetActive(false);
        }
        if(tamanyoBaraja < 1)
        {
            cartaEnMazo4.SetActive(false);
        }
        */
    }

    /*
    * Rutina para que espere un segundo cada vez que saque una carta del mazo y la ponga en la mano.
    * Sin esta rutina las cartas salen demasiado rápido.
    
    IEnumerator StartGame()
    {
        for(int i = 0; i <=6; i++) //Aquí está la cantidad de cartas que se mostrará en la mano al iniciar 
        {
            yield return new WaitForSeconds(0.5f);

            Instantiate(CartaAMano, transform.position, transform.rotation);
        }
    }
    
    public void Barajar()
    {
        for(int i=0; i < tamanyoBaraja; i++)
        {
            container[0] = baraja[i];
            int randomIndex = Random.Range(i, tamanyoBaraja);
            baraja[i] = baraja[randomIndex];
            baraja[randomIndex] = container[0];
        }
    }
    */
}
