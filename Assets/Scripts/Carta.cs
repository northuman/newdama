using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nueva Carta", menuName = "Cartas", order = 0)]
public class Carta : ScriptableObject {

    public string nombreCarta;
    public int[] costeMana;
    public string palabrasClave;
    public string descripcion;
    public string flavour;
    public string tipoCarta;
    public string tiposConcretos;
    public int fuerza;
    public int resistencia;
    public int[] cantidadMana;
    public int[] cantidadMana2;

    public List<Efecto> efectos;

    //   in ro bl ve ne
    //int[0][0][0][0][0]

    public int CosteTotal() {

        int coste = 0;

        for(int i=0; i<costeMana.Length; i++) {

            coste += costeMana[i];
        }

        return coste;
    }
} 
