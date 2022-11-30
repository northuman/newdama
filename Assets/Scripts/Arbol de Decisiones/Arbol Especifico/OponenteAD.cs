using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArbolDecisiones;

public class OponenteAD : Arbol {

    public IA oponente;

    protected override Nodo EstablecerArbol() {

        //Nodo raiz = new JugarTierra(oponente);

        Nodo raiz = new Selector(new List<Nodo>{

            new Secuencia(new List<Nodo>{
                
                new JugarTierra(oponente),
            }),
            new Secuencia(new List<Nodo>{ //Esperando

                new Esperar()
            })
        });

        return raiz;
    }
}
