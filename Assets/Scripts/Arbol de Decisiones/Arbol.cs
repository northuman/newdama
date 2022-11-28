using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArbolDecisiones {

    public abstract class Arbol : MonoBehaviour {
        
        private Nodo raiz = null;

        protected void Start() {
            
            raiz = EstablecerArbol();
        }

        protected void Update() {
            
            if(raiz != null) 
                raiz.Evaluar();
        }

        protected abstract Nodo EstablecerArbol();
    }
}
