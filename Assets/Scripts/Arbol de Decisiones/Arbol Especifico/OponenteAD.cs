using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArbolDecisiones;

public class OponenteAD : Arbol {

    public IA oponente;

    protected override Nodo EstablecerArbol() {

        Nodo raiz = new JugarTierra(oponente);

        return raiz;
    }
}
