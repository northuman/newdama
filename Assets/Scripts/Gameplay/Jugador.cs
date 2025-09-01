using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/*
* Contiene los parámetros del jugador, la baraja y las funciones de las cartas.
* InicializarJugador : incializa todos los parámetros.
*/

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

    public void InicializarJugador()
    {
        //barajaOriginal = baraja;
        vida = 20;
        mana = new int[] { 0, 0, 0, 0 };
        barajaOriginal = new List<Carta>();
        barajaPartida = new List<Carta>();
        mano = new List<Carta>();
        cementerio = new List<CartasJugadas>();
        pila = new List<CartasJugadas>();
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

        if (listaTierras.Count == 0 || noTierras.Count == 0)
        {
            Debug.LogError("No hay suficientes cartas de cada tipo en la base de datos.");
            return;
            //por si acaso el archivo está vacío
        }

        //Se añaden las tierras
        for (int i = 0; i < cantidadTierras; i++)
        {
            int randomIndex = Random.Range(0, listaTierras.Count);
            barajaOriginal.Add(new Carta(listaTierras[randomIndex]));
        }
        for (int i = 0; i < cantidadResto; i++)
        {
            int randomIndex = Random.Range(0, noTierras.Count);
            barajaOriginal.Add(new Carta(noTierras[randomIndex]));
        }
    }

    public void CrearBarajaPartida()
    {
        barajaPartida = barajaOriginal.ToList();
        barajaPartida.Randomizar();
    }
    /*
    Comprueba si está en el panel de tierras y se llama a RotarCarta()
    Si la carta está girada se llama a SumarMana(), si no a RestarMana()
    Se le pasa un GameObject carta.
    */
    public void GirarTierra(GameObject tierraSeleccionada)
    {
        GameObject panelTierras = GameObject.Find("AreaTierras");
        if (tierraSeleccionada.transform.parent.gameObject == panelTierras)
        {
            tierraSeleccionada.GetComponent<CartasJugadas>().RotarCarta();

            if (tierraSeleccionada.GetComponent<CartasJugadas>().girada == true)
            {
                SumarMana(tierraSeleccionada);
            }
            else
            {
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
        for (int i = 0; i < 4; i++)
        {
            mana[i] += carta.costeMana[i + 1];
        }
    }

    /* RestarMana()
    Resta a cada posición de maná de color.
    Después comprueba el coste de maná incoloro.
    */
    public void RestarMana(GameObject cartaSeleccionada)
    {
        Carta carta = cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta();
        for (int i = 0; i < 4; i++)
        {
            mana[i] -= carta.costeMana[i + 1];
        }

        int incoloro = cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[0];
        int restado = 0;
        int pos = 0;

        //resta del mana incoloro
        while (restado < incoloro)
        {
            if (mana[pos] > 0)
            {
                mana[pos] -= 1;
                restado++;
            }
            else
            {
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
        for (int i = 0; i < mana.Length; i++)
        {
            if (mana[i] - carta.GetComponent<MostrarCarta>().GetCarta().costeMana[i + 1] < 0)
            {
                ok = false;
            }
            else
            {
                if (carta.GetComponent<MostrarCarta>().GetCarta().costeMana[i + 1] > 0)
                {
                    cantidad += carta.GetComponent<MostrarCarta>().GetCarta().costeMana[i + 1];
                }
            }
        }
        if (ok && ManaRestante(cantidad) >= carta.GetComponent<MostrarCarta>().GetCarta().costeMana[0])
        {
            ok = true;
        }
        else { ok = false; }
        if (!ok)
        {
            mensaje.MostrarMensaje("No tienes maná suficiente!");
        }
        return ok;
    }

    /*Función auxiliar para el cálculo de maná
    Te dice cuánto maná queda en el momento de la invocación
    */
    public int ManaRestante(int cantidad)
    {
        int restante = 0;
        for (int i = 0; i < mana.Length; i++)
        {
            restante += mana[i];
        }
        return restante - cantidad;
    }

    /**Robar carta
    num: cantidad de cartas a robar.
    Se añade una carta a la mano por iteración
    Se llama a RobarCartasSecuencialmente para que haga una copia de la plantilla con la carta que se ha robado
    **/
    public bool RobarCarta(int num)
    {
        bool robar = false;
        if (barajaPartida.Count >= num)
        {
            StartCoroutine(RobarCartasSecuencialmente(num));
            robar = true;
        }
        return robar;
    }

    IEnumerator RobarCartasSecuencialmente(int num)
    {

        for (int i = 0; i < num; i++)
        {
            // Añadimos la carta a la mano del jugador
            mano.Add(barajaPartida[^1]);

            // Instanciamos y animamos la carta, esperando que termine la animación antes de seguir con la siguiente
            yield return StartCoroutine(InstanciarPrefab(barajaPartida[^1]));

            // La carta se ha robado y eliminamos de la baraja
            barajaPartida.RemoveAt(barajaPartida.Count - 1);
        }
    }

    IEnumerator InstanciarPrefab(Carta carta)
    {
        GameObject nuevaCarta = Instantiate(CartaJugada, Mazo.transform.position, Quaternion.identity);
        nuevaCarta.GetComponent<MostrarCarta>().id = carta.id;
        nuevaCarta.GetComponent<CartasJugadas>().perteneceAJugador = id;

        // Escala inicial para animación
        nuevaCarta.transform.localScale = Vector3.zero;

        // IMPORTANTE: Insertar primero en la mano para que el layout la posicione correctamente
        nuevaCarta.transform.SetParent(Mano.transform, false);

        // Esperamos un frame para que el layout la posicione
        yield return null;

        // Guardamos la posición final asignada por el layout
        Vector3 destino = nuevaCarta.transform.position;

        // Sacamos temporalmente la carta del layout para animarla libremente
        nuevaCarta.transform.SetParent(this.transform, true); // mantener worldPosition

        // Posición inicial y escala
        Vector3 inicio = Mazo.transform.position;
        Vector3 escalaFinal = new(0.8f, 0.8f, 0.8f);
        float t = 0f;
        float duracion = 0.4f;

        // Animación: mover desde el mazo a su destino final
        while (t < duracion)
        {
            float smoothT = Mathf.SmoothStep(0, 1, t / duracion);

            nuevaCarta.transform.position = Vector3.Lerp(inicio, destino, smoothT);
            nuevaCarta.transform.localScale = Vector3.Lerp(Vector3.zero, escalaFinal, smoothT);

            t += Time.deltaTime;
            yield return null;
        }

        // Asegura valores finales
        nuevaCarta.transform.position = destino;
        nuevaCarta.transform.localScale = escalaFinal;

        // Finalmente, la volvemos a meter en el layout
        nuevaCarta.transform.SetParent(Mano.transform, true); // mantener posición

        yield return null;
    }

    public void ReiniciarEstadisticasBatalla()
    {
        //Reiniciar a cada carta en campo de batalla fuerza y resistencia por defecto
        for (int i = 0; i < batalla.Count; i++)
        {
            batalla[i].fuerzaActual = batalla[i].carta.fuerza;
            batalla[i].resistenciaActual = batalla[i].carta.resistencia;
        }
    }

    //Encontrar carta por ID
    //Se le pasa un id, busca en BarajaOriginal y devuelve la carta
    public Carta FindById(int id)
    {
        foreach (Carta c in barajaOriginal)
        {
            if (c.id == id)
            {
                return c;
            }
        }
        return null;
    }


}
