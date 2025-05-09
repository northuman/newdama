using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject Mazo;

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


    //hay que crear la barajaOriginal para poder probar
    //Se crea la baraja con un número determinado de tierras para facilitar las pruebas
    public void RellenarBaraja()
    {
        tamanyoBaraja = 40;
        int cantidadTierras = 20;
        int cantidadResto = tamanyoBaraja - cantidadTierras;

        barajaOriginal.Clear();

        List<Carta> listaTierras = CartaDatabase.listaCartas.Where(c => c.tipo == 0).ToList();
        List<Carta> noTierras = CartaDatabase.listaCartas.Where(c => c.tipo != 0).ToList();

        if (listaTierras.Count == 0 || noTierras.Count == 0){
            Debug.LogError("No hay suficientes cartas de cada tipo en la base de datos.");
            return;
            //por si acaso el archivo está vacío
        }

        //Se añaden las tierras
        for (int i = 0; i < cantidadTierras; i++){
            int randomIndex = Random.Range(0, listaTierras.Count);
            barajaOriginal.Add(new Carta(listaTierras[randomIndex]));
        }
        for (int i = 0; i < cantidadResto; i++){
            int randomIndex = Random.Range(0, noTierras.Count);
            barajaOriginal.Add(new Carta(noTierras[randomIndex]));
        }

        //for(int i = 0; i < tamanyoBaraja; i++)
        //{
        //    int random = Random.Range(0, CartaDatabase.listaCartas.Count - 1);
        //    barajaOriginal.Add(new Carta(CartaDatabase.listaCartas[random]));
        //}
    }

    public void CrearBarajaPartida()
    {
        barajaPartida = barajaOriginal.ToList();
        barajaPartida.Randomizar();
        int contadorTierra = 0;
        int contadorCriatura = 0;

        //Debug para ver qué cartas hay en la baraja
        foreach (Carta c in barajaPartida){
            if(c.tipo == 0){contadorTierra++;};
            if(c.tipo == 1){contadorCriatura++;};
        }
        Debug.Log(contadorTierra + "tierras para " + nombre);
        Debug.Log(contadorCriatura + "criaturas para " + nombre);  
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
            }else{
                if(carta.GetComponent<MostrarCarta>().GetCarta().costeMana[i+1] > 0){
                    cantidad += carta.GetComponent<MostrarCarta>().GetCarta().costeMana[i+1];
                }
            }
        }
        if(ok && ManaRestante(cantidad) >= carta.GetComponent<MostrarCarta>().GetCarta().costeMana[0]){
            ok = true;
        }else{ok = false;}
        if(!ok){
                mensaje.MostrarMensaje("No tienes suficiente maná!");
        }
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
        if(barajaPartida.Count>=num)
        {
            StartCoroutine(RobarCartasSecuencialmente(num));
            robar = true;
        }
        return robar;
    }

    IEnumerator RobarCartasSecuencialmente(int num){
        
        for (int i = 0; i < num; i++){
        // Añadimos la carta a la mano del jugador
        mano.Add(barajaPartida[^1]);

        // Instanciamos y animamos la carta, esperando que termine la animación antes de seguir con la siguiente
        yield return StartCoroutine(InstanciarPrefab(barajaPartida[^1]));

        // La carta se ha robado y eliminamos de la baraja
        barajaPartida.RemoveAt(barajaPartida.Count - 1);
        }
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
        //Instantiate(CartaJugada, transform.position, transform.rotation);
        //CartaJugada.GetComponent<MostrarCarta>().id = carta.id;
        //CartaJugada.GetComponent<CartasJugadas>().perteneceAJugador = id;
        //yield return null;

        // Instancia el prefab y guarda la referencia
        
        GameObject nuevaCarta = Instantiate(CartaJugada, Mazo.transform.position, transform.rotation);

        // Asigna los datos a la carta recién instanciada
        nuevaCarta.GetComponent<MostrarCarta>().id = carta.id;
        nuevaCarta.GetComponent<CartasJugadas>().perteneceAJugador = id;

        // Inicializa la carta en una escala pequeña para el efecto
        nuevaCarta.transform.localScale = Vector3.zero;

        HorizontalLayoutGroup layoutGroup = Mano.GetComponent<HorizontalLayoutGroup>();
        layoutGroup.enabled = false;

        // Duración de la animación (puedes ajustarlo a tu gusto)
        float tiempoDeAnimacion = 0.3f;
        float tiempoPasado = 0f;

        // Mueve la carta de la posición del mazo a la mano del jugador
        while (tiempoPasado < tiempoDeAnimacion)
        {
            // Animación de movimiento: Interpolación lineal de la posición
            nuevaCarta.transform.position = Vector3.Lerp(Mazo.transform.position, Mano.transform.position, tiempoPasado / tiempoDeAnimacion);

            // Animación de escala: Crece la carta desde 0 a su tamaño normal
            nuevaCarta.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, tiempoPasado / tiempoDeAnimacion);

            tiempoPasado += Time.deltaTime;

            yield return null;
        }
        nuevaCarta.transform.SetParent(Mano.transform);

        layoutGroup.enabled = true;

        yield return new WaitForSeconds(0.1f); 
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
