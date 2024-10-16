using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MostrarCarta : MonoBehaviour
{
    public List<Carta> mostrarCarta = new List<Carta>();
    public int mostrarId;

    //Parametros de la carta
    public int id;
    public string nombreCarta;
    public string tipo;
    public string rareza;
    public string costeMana;
    public int fuerza;
    public int resistencia;
    public string descripcion;
    public string flavour;
    //public Sprite spriteImagen;


    //Parametros en la interfaz
    public TMP_Text nombreText;
    public TMP_Text tipoText;
    public TMP_Text manaText;
    public TMP_Text statsText;
    public TMP_Text descText;
    public TMP_Text flavourText;
    //public Image fotoImagen;
    public bool reverso;
    //public static bool staticReverso;
    public string color;
    public GameObject ManoJugador;
    public GameObject ManoOponente;
    public int numCartasEnBaraja;
    public GameObject Jugador1;
    public GameObject Jugador2;

    void Start()
    {
        Jugador1 = GameObject.Find("Jugador");
        numCartasEnBaraja = Jugador.tamanyoBaraja;
        Jugador2 = GameObject.Find("Oponente");

    }

    //Esta función permite obtener los parámetros de Carta desde esta clase
    public Carta GetCarta()
    {
        return mostrarCarta[0];
    }

    /*
    Coge los parámetros de las cartas y rellena las plantillas
    */
    public void RenderizarCarta()
    {

        //Coge las cartas de cada jugador
        if(Jugador1.GetComponent<Jugador>().findById(id) != null){
            
            mostrarCarta[0] = Jugador1.GetComponent<Jugador>().findById(id);
        }
        else if(Jugador2.GetComponent<Jugador>().findById(id) != null){
            
            mostrarCarta[0] = Jugador2.GetComponent<Jugador>().findById(id);
        }

        //Le pone el color a la carta
        color = mostrarCarta[0].color;
        UtilCartas.colorearCarta(this.gameObject);
        
        //Se asignan los valores a la carta que coge de cada jugador
        id = mostrarCarta[0].id; 
        nombreCarta = mostrarCarta[0].nombreCarta;
        tipo = mostrarCarta[0].tipoToString(mostrarCarta[0].tipo);
        rareza = mostrarCarta[0].rareza;
        costeMana = mostrarCarta[0].manaToString(mostrarCarta[0].costeMana);
        fuerza = mostrarCarta[0].fuerza;
        resistencia = mostrarCarta[0].resistencia;
        descripcion = mostrarCarta[0].descripcion;
        flavour = mostrarCarta[0].flavour;
        //spriteImagen = mostrarCarta[0].spriteImagen;


        //Se asignan los valores de la carta a los de la interfaz
        nombreText.text = "" + nombreCarta;
        tipoText.text = "" + tipo + " - " + rareza;
        descText.text = "" + descripcion;
        flavourText.text = "" + flavour;
        //fotoImagen.sprite = spriteImagen;


        //si la carta No es una tierra, se muestran sus parámetros      
        if(tipo != "Tierra"){
            manaText.text = "" + costeMana;
            statsText.text = "" + fuerza + " / " + resistencia; 
        }
        //si es una tierra, se ocultan el maná y las estadísticas.
        else{
            manaText.text = "";
            statsText.text = "";
            this.transform.Find("Borde/Color/Lineas/Stats").GetComponent<Image>().enabled = false;
        }
        
        //Busca el panel con nombre Mano y lo guarda como GameObject
        ManoJugador = GameObject.Find("ManoJugador");
        ManoOponente = GameObject.Find("ManoOponente");
        
    }
    
    void Update()
    {
        //Se renderiza la carta cada frame
        RenderizarCarta();
    }

}
