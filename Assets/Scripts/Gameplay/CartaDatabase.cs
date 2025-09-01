using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/*
*Carga el archivo txt seleccionado desde el editor y crea una lista con las cartas.
*CargarCartas: abre el archivo, separa las líneas por "#", las pasa como parámetro para crear la carta.
*/
public class CartaDatabase : MonoBehaviour
{
    public static List<Carta> listaCartas = new();
    public TextAsset archivoCartas; //este es el archivo de donde se van a crear las cartas
    void Awake()
    {
        CargarCartas();
        //listaCartas.Add(new Carta("None",0,"Común", new int[5],"B", 0,0,Resources.Load<TextAsset>("None"), Resources.Load<TextAsset>("None") ));
    }

    private void CargarCartas()
    {

        if (archivoCartas != null)
        {  //se comprueba que se haya seleccionado el archivo

            using StringReader sr = new(archivoCartas.text);
            string linea;
            while ((linea = sr.ReadLine()) != null)
            {

                string[] partes = linea.Split('#');
                listaCartas.Add(new Carta(partes[0], int.Parse(partes[1]), partes[2], ParseCoste(partes[3]), partes[4], int.Parse(partes[5]), int.Parse(partes[6]), partes[7], partes[8]));
                Debug.Log($"Carta añadida: {partes[0]}");
            }

        }
        else
        {
            Debug.LogError("No hay archivo de cartas");
        }
    }

    private int[] ParseCoste(string coste)
    {
        //Debug.Log(coste[0]);
        return new int[5] { int.Parse(coste[0].ToString()), int.Parse(coste[1].ToString()), int.Parse(coste[2].ToString()), int.Parse(coste[3].ToString()), int.Parse(coste[4].ToString()) };
    }
}

