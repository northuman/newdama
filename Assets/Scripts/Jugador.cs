using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{

    public int id;
    public string nombre;
    public int vida;
    public int[] mana;  
    public List<Carta> barajaOriginal;
    public List<Carta> barajaPartida;
    public List<Carta> mano;
    public List<CartasJugadas> cementerio;
    public List<CartasJugadas> pila;
    public List<CartasJugadas> tierras;
    public List<CartasJugadas> batalla;
    public GameObject CartaJugada;
    public static int tamanyoBaraja;   
    public GameObject Mano;

    //bool tierraJugada = false;

    void Start()
    {    
        InicializarJugador();  
    }
    
    public void InicializarJugador(){
        //barajaOriginal = baraja;
        vida = 20;
        mana = new int[]{0,0,0,0};
        barajaOriginal = new List<Carta>();
        barajaPartida = new List<Carta>();
        mano = new List<Carta>();
        cementerio = new List<CartasJugadas>();
        pila    = new List<CartasJugadas>();
        tierras = new List<CartasJugadas>();
        batalla = new List<CartasJugadas>();
    }

    public void CrearBarajaPartida()
    {
        barajaPartida = barajaOriginal.ToList();
        barajaPartida.Randomizar();
    }

    //hay que crear la barajaOriginal para poder probar
    public void RellenarBaraja()
    {
        tamanyoBaraja = 40;
        for(int i = 0; i < tamanyoBaraja; i++)
        {
            int random = Random.Range(0, CartaDatabase.listaCartas.Count - 1);
            barajaOriginal.Add(new Carta(CartaDatabase.listaCartas[random]));
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

        int incoloro = cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[0];
        int restado = 0;
        int pos = 0;

        //resta del mana incoloro       restado=0   incoloro=2  restante=0
        while (restado < incoloro){
            if (mana[pos] > 0){
                mana[pos] -= 1;
                restado++;
            }else{
                pos++;
            }
        }
    }

    /*
    Aquí se hacen los cálculos del maná. 
    Se tiene que comprobar que el juegador tenga suficiente.
    También se tiene que comprobar qué tipo de carta es.
    Devuelve true si tiene suficiente maná.
    */
    public bool ComprobarMana(GameObject carta)
    {
        int cantidad = 0;
        bool ok = true;
        for(int i = 0; i < mana.Length; i++){
            if(mana[i] - carta.GetComponent<MostrarCarta>().GetCarta().costeMana[i+1] < 0){
                ok = false;
                Debug.Log("No tienes maná suficiente");
            }else{
                if(carta.GetComponent<MostrarCarta>().GetCarta().costeMana[i+1] > 0){
                    cantidad += carta.GetComponent<MostrarCarta>().GetCarta().costeMana[i+1];
                }
            }
        }
        if(ok && ManaRestante(cantidad) >= carta.GetComponent<MostrarCarta>().GetCarta().costeMana[0]){
            ok = true;
        }else{ok = false;}
        return ok;
    }

    public int ManaRestante(int cantidad){
        int restante = 0;
        for(int i = 0; i < mana.Length; i++){
            restante += mana[i];
        }
        return restante - cantidad;
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

     
    public void ReiniciarEstadisticasBatalla(){
        //Reiniciar a cada carta en campo de batalla fuerza y resistencia por defecto
        for(int i = 0; i<batalla.Count; i++){
            batalla[i].fuerzaActual = batalla[i].carta.fuerza;
            batalla[i].resistenciaActual = batalla[i].carta.resistencia;
        }
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
