using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nueva Carta", menuName = "Cartas", order = 0)]
public class Carta : ScriptableObject {

    public string nombreCarta;
    public int[] costeMana;
    public string palabrasClave;
    public string descripcion;
    public string tipoCarta;
    public string tiposConcretos;
    public int fuerza;
    public int resistencia;
} 
