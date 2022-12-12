using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArbolDecisiones;

public class OponenteAD : Arbol {

    public IA oponente;

    protected override Nodo EstablecerArbol() {

        JugarTierra jt = new JugarTierra(oponente);
        JugarCriatura jc = new JugarCriatura(oponente);

        Nodo raiz = new Selector(new List<Nodo>{

            new Selector(new List<Nodo>{

                jt,
                jc,
                new PasarFase(oponente)
            }),
            new Esperar()
        });

        /*Nodo raiz = new Selector(new List<Nodo>{

            new Secuencia(new List<Nodo>{
                
                new JugarTierra(oponente),
                //new JugarCriatura(oponente),
                //new Atacar(oponente),
                new PasarFase(oponente),
            }),
            new Secuencia(new List<Nodo>{ //Esperando

                new Esperar()
            })
        });*/

        return raiz;
    }
}