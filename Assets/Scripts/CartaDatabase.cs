using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CartaDatabase : MonoBehaviour
{
    public static List<Carta> listaCartas = new();

    void Awake()
    {
        CargarCartas();
        //BARAJA NOBLEZA    
        //listaCartas.Add(new Carta("None",0,"Común", new int[5],"B", 0,0,Resources.Load<TextAsset>("None"), Resources.Load<TextAsset>("None") ));
        //listaCartas.Add(new Carta("Escudero",1, "Común", new int[5] {0,0,0,1,0},"B",1,1, Resources.Load<TextAsset>("Ds_Escudero"), Resources.Load<TextAsset>("Fl_Escudero") ));
        //listaCartas.Add(new Carta("Gentilhombre",1,"Común", new int[5] {1,0,0,1,0},"N", 1,1, Resources.Load<TextAsset>("Ds_Gentilhombre"), Resources.Load<TextAsset>("Fl_Gentilhombre") ));
        //listaCartas.Add(new Carta("Dama de corte",1,"Común", new int[5] {2,0,0,1,0},"R",1,1,Resources.Load<TextAsset>("Ds_DamaCorte"), Resources.Load<TextAsset>("Fl_DamaCorte") ));
        //listaCartas.Add(new Carta("Barón",1,"Común", new int[5] {2,0,0,2,0},"V",2,2,Resources.Load<TextAsset>("Ds_Baron"), Resources.Load<TextAsset>("None") ));
        //listaCartas.Add(new Carta("Señorío",0,"Común", new int[5] {0,0,0,0,0},"R",0,0,Resources.Load<TextAsset>("None"), Resources.Load<TextAsset>("None") ));
        
        //
    
    }

    //Parametros {nombre, tipo, rareza, coste, color, ataque, defensa, descripcion, flavour}            
    //Maná { INCOLORO, BLANCO, NEGRO, ROJO, VERDE}
    //Colores {"B", "N", "R", "V", "BN", "BR", "BV", "NR", "NV", "RN", "RV"}

    private void CargarCartas(){
        // Cargar archivo como TextAsset
        TextAsset txt = Resources.Load<TextAsset>("cartas");
        

        if(txt != null){

            using StringReader sr = new(txt.text);
            string linea;
            while ((linea = sr.ReadLine()) != null)
            {

                string[] partes = linea.Split('/');
                listaCartas.Add(new Carta(partes[0], int.Parse(partes[1]), partes[2], ParseCoste(partes[3]), partes[4], int.Parse(partes[5]), int.Parse(partes[6]), partes[7], partes[8]));
            }

        }
        else{
            Debug.LogError("No se puede cargar las cartas");
        }
    }

    private int[] ParseCoste(string coste) {
        //Debug.Log(coste[0]);
        return new int[5] {int.Parse(coste[0].ToString()), int.Parse(coste[1].ToString()), int.Parse(coste[2].ToString()), int.Parse(coste[3].ToString()), int.Parse(coste[4].ToString())};
    }   
}

