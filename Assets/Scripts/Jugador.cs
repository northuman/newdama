using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{

    public int id;
    public string nombre;
    public int vida;
    public int[] mana; 
    public List<Carta> barajaOriginal = new List<Carta>();
    public List<Carta> barajaPartida;
    public List<Carta> mano;
    public List<CartasJugadas> cementerio;
    public List<CartasJugadas> pila;
    public List<CartasJugadas> tierras;
    public List<CartasJugadas> batalla;
    public GameObject CartaJugada;
    public static int tamanyoBaraja;   
    public int x;
    public GameObject Mano;

    //bool tierraJugada = false;

    public void InicializarJugador(){
        //barajaOriginal = baraja;
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
    public void rellenarBaraja()
    {
        x = 0;
        tamanyoBaraja = 40;
        for(int i = 0; i < tamanyoBaraja; i++)
        {        
            x = Random.Range(0,CartaDatabase.listaCartas.Count -1);
            barajaOriginal.Add(new Carta(CartaDatabase.listaCartas[x]));
        }
    }

    /*
    Comprueba si está en el panel de tierras y se llama a GirarCarta()
    Si la carta está girada se llama a SumarMana(), si no a RestarMana()
    Se le pasa una carta.
    */
    public void GirarTierra(GameObject tierraSeleccionada)
    {
        GameObject panelTierras = GameObject.Find("AreaTierras");
        if(tierraSeleccionada.transform.parent.gameObject == panelTierras){
            tierraSeleccionada.GetComponent<CartasJugadas>().RotarCarta();
            //tierraSeleccionada.GetComponent<Reverso>().GirarCarta();
            if(tierraSeleccionada.GetComponent<CartasJugadas>().girada == true){
                SumarMana(tierraSeleccionada);
            }else{
                RestarMana(tierraSeleccionada);
            }
        }
    }

    
    /* 
    * SumarMana()
    Suma a cada posición del maná del jugador, la posición correspondiente del coste de maná de la carta.
    Se le pasa una carta.
    * RestarMana()
    Resta a cada posición de la misma manera que sumar.
    */
    
    public void SumarMana(GameObject cartaSeleccionada)
    {
        mana[0] += cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[1];
        mana[1] += cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[2];
        mana[2] += cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[3];
        mana[3] += cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[4];
        
    }
    public void RestarMana(GameObject cartaSeleccionada)
    {
        mana[0] -= cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[1];
        mana[1] -= cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[2];
        mana[2] -= cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[3];
        mana[3] -= cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[4];
    }

    /*
    Aquí se harán los cálculos del maná. 
    Se tiene que comprobar que el juegador tenga suficiente.
    También se tiene que comprobar qué tipo de carta es.
    Devuelve true si tiene suficiente maná.
    Deberá llamar a RestarMana().
    */
    public bool ComprobarMana(GameObject carta)
    {
        bool ok = true;
        for(int i = 0; i < mana.Length; i++){
            if(mana[i] - carta.GetComponent<MostrarCarta>().GetCarta().costeMana[i+1] < 0){
                ok = false;
                Debug.Log("No tienes maná suficiente");
            }
        }        
        return ok;
    }

    /**Robar carta
    num: cantidad de cartas a robar.
    Se añade una carta a la mano por iteración
    Se llama a InstanciarPrefab para que haga una copia de la plantilla con la carta que se ha robado
    Se quita la carta de la baraja
    **/
    public bool RobarCarta(int num){
        bool robar = false;
        if(barajaPartida.Count>=num){
            for(int i = 0; i<num;i++){
                
                mano.Add(barajaPartida[barajaPartida.Count-1]);    
                
                StartCoroutine(InstanciarPrefab(barajaPartida[barajaPartida.Count-1]));
                barajaPartida.RemoveAt(barajaPartida.Count-1);
            }
            robar = true;
        }
        return robar;
    }

    
    //Elimina carta en mano
    //num : indice de carta de mano
    public void EliminarCartaMano(int num){
        //Eliminar en caso de tener id
        /*
        var cartaAEliminar = mano.SingleOrDefault(r => r.Id == num);
        if (cartaAEliminar != null)
            mano.Remove(cartaAEliminar);
            */

        //Eliminar en caso de posicion
        mano.RemoveAt(num);
    }

    
    public void ReiniciarEstadisticasBatalla(){
        //Reiniciar a cada carta en campo de batalla fuerza y resistencia por defecto
        for(int i = 0; i<batalla.Count; i++){
            batalla[i].fuerzaActual = batalla[i].carta.fuerza;
            batalla[i].resistenciaActual = batalla[i].carta.resistencia;
        }
    }
    

    //
    void Start()
    {    
        //Esto está aquí porque tiene que hacerse una vez al principio
        //El constructor de la clase, al tener un parámetro, no se ejecuta de forma automática
        vida = 20;
        mana = new int[]{0,0,0,0};  
    }

    //Rutina para instanciar prefabs
    //Instancia una plantilla de carta vacía. Se le pasa una carta y le pone su id a la instancia.
    IEnumerator InstanciarPrefab(Carta carta)
    {
        //yield return new WaitForSeconds(0.5f);
        Instantiate(CartaJugada, transform.position, transform.rotation);
        CartaJugada.GetComponent<MostrarCarta>().id = carta.id;
        CartaJugada.GetComponent<CartasJugadas>().perteneceAJugador = id;
        //CartaJugada.transform.localScale = new Vector3(0.5f, 0.5f, 0);
        
        //Debug.Log("instancio carta " + CartaJugada.GetComponent<MostrarCarta>().id);
        yield return null;

    }

    //Encontrar carta por ID
    //Se le pasa un id, busca en BarajaOriginal y devuelve la carta
    public Carta FindById(int id){
        foreach (Carta c in barajaOriginal){
            if (c.id == id){
                return c;
            }
        }
        return null;
    }
}
