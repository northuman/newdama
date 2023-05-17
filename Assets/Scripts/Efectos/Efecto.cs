using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Efecto", menuName = "Efectos", order = 0)]
public class Efecto : ScriptableObject {
    
    public int condicion;
    public int habilidad;
    public int cantidad;
    public bool costePagado;
    public int[] coste;
    public string[] palabrasClave;
    public string[] otorgarPalabrasClave;
    public int[] mejoraEstadisticasPropias;
    public int[] bajadaEstadisticasPropias;
    public int[] mejoraEstadisticasOponente;
    public int[] bajadaEstadisticasOponente;
    public bool[] coloresNoAfectados;
    public bool resuelto = false;
    public Arrastrable objetivo = null;
}
