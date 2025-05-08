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
    public MensajeManager mensaje;

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
    Comprueba si está en el panel de tierras y se llama a RotarCarta()
    Si la carta está girada se llama a SumarMana(), si no a RestarMana()
    Se le pasa un GameObject carta.
    */
    public void GirarTierra(GameObject tierraSeleccionada)
    {
        GameObject panelTierras = GameObject.Find("AreaTierras");
        if(tierraSeleccionada.transform.parent.gameObject == panelTierras){
            tierraSeleccionada.GetComponent<CartasJugadas>().RotarCarta();
            
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
    
    */
    
    public void SumarMana(GameObject cartaSeleccionada)
    {
        Carta carta = cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta();
        for(int i = 0; i < 4; i++){
            mana[i] += carta.costeMana[i+1];
        }
    }

    /* RestarMana()
    Resta a cada posición de maná de color.
    Después comprueba el coste de maná incoloro.
    */
    public void RestarMana(GameObject cartaSeleccionada)
    {
        Carta carta = cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta();
        for (int i = 0; i < 4; i++){
            mana[i] -= carta.costeMana[i+1];
        }

        int incoloro = cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[0];
        int restado = 0;
        int pos = 0;

        //resta del mana incoloro
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
    Se comprueba que el juegador tenga suficiente.
    Devuelve true si tiene suficiente maná.
    */
    public bool ComprobarMana(GameObject carta)
    {
        int cantidad = 0;
        bool ok = true;
        for(int i = 0; i < mana.Length; i++){
            if(mana[i] - carta.GetComponent<MostrarCarta>().GetCarta().costeMana[i+1] < 0){
                ok = false;
                mensaje.MostrarMensaje("No tienes suficiente maná!");
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

    /*Función auxiliar para el cálculo de maná
    Te dice cuánto maná queda en el momento de la invocación
    */
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
                mano.Add(barajaPartida[^1]);    
                StartCoroutine(InstanciarPrefab(barajaPartida[^1]));
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
        Instantiate(CartaJugada, transform.position, transform.rotation);
        CartaJugada.GetComponent<MostrarCarta>().id = carta.id;
        CartaJugada.GetComponent<CartasJugadas>().perteneceAJugador = id;
        
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
