using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Efecto", menuName = "Efectos", order = 0)]
public class Efecto : ScriptableObject {
    
    public int condicion;
    public int habilidad;
    public int cantidad;
    public bool resuleto = false;
}
