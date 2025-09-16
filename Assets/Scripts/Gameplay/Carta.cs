using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

/*
* Contiene los parámetros de las cartas, el constructor y funciones auxiliares de cartas.
*/

public class Carta
{

    public enum Tipos { TIERRA, CRIATURA, CONJURO, INSTANTANEO, ARTEFACTO, ENCANTAMIENTO };
    public enum Colores { INCOLORO, BLANCO, NEGRO, ROJO, VERDE };
    private string[] valoresColores = { "", "B", "N", "R", "V" }; //Enum Colores para convertir el coste de maná y poder mostrarlo

    public static int nextid = 1;
    public int id = nextid++;
    public string nombreCarta;
    public int tipo;
    public string rareza;
    public int[] costeMana; //Orden de enum Colores
    public string color;
    public int fuerza;
    public int resistencia;
    public string descripcion; //
    public string flavour;   // 

    // lista de cosas
    public List<IAtributo> atributos;
    public List<IHabilidad> habilidades;
    public List<ITrigger> triggers;

   


    //public Sprite spriteImagen;

    public Carta()
    {

    }

    public Carta(string NombreCarta, int Tipo, string Rareza, int[] CosteMana, string Color, int Fuerza, int Resistencia, string Descripcion, string Flavour)
    {

        nombreCarta = NombreCarta;
        tipo = Tipo;
        rareza = Rareza;
        costeMana = CosteMana;
        color = Color;
        fuerza = Fuerza;
        resistencia = Resistencia;
        descripcion = Descripcion;
        flavour = Flavour;
        atributos = new List<IAtributo>();
        atributos.Add(new Prisa());
        triggers = new List<ITrigger>();

        //spriteImagen=SpriteIm;

    }

    public Carta(Carta c)
    {
        nombreCarta = c.nombreCarta;
        tipo = c.tipo;
        rareza = c.rareza;
        costeMana = c.costeMana;
        color = c.color;
        fuerza = c.fuerza;
        resistencia = c.resistencia;
        descripcion = c.descripcion;
        flavour = c.flavour;
        atributos = new List<IAtributo>(c.atributos);
        Debug.Log($"'{atributos[0]}'");
    }


    /*
    * Descripcion: devuelve el tipo de carta en un string para poder mostrarlo en MostrarCarta
    * Parametros: t = tipo de la clase Carta
    * Devuelve: tipo en formato string
    */
    public string TipoToString(int t)
    {
        string mostrarTipo = "";
        switch (t)
        {
            case 0:
                mostrarTipo = "Tierra";
                break;
            case 1:
                mostrarTipo = "Criatura";
                break;
            case 2:
                mostrarTipo = "Conjuro";
                break;
            case 3:
                mostrarTipo = "Instantáneo";
                break;
            case 4:
                mostrarTipo = "Artefacto";
                break;
            case 5:
                mostrarTipo = "Encantamiento";
                break;

        }
        return mostrarTipo;
    }


    /*
    * Descripcion: devuelve el coste de la carta en el formato correcto para mostarlo en MostrarCarta
    * Parametros: m = vector de coste de la clase
    * Devuelve: coste en formato string
    */
    public string ManaToString(int[] m)
    {
        string costeTotal = "";
        for (int i = 0; i < m.Length; i++)
        {
            int contador = m[i];
            if (i == 0 && m[0] > 0)
            {
                costeTotal += m[0];
            }
            while (contador > 0)
            {
                costeTotal += valoresColores[i];
                contador--;
            }
        }
        return costeTotal;
    }
}
