using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

/*
* falta crearlo como un gameobject para que se vea fisicamente
*/

public class Contador
{
    public int fuerza;
    public int resistencia;

    public Contador(int fuerza, int resistencia)
    {
        this.fuerza = fuerza;
        this.resistencia = resistencia;
    }
}