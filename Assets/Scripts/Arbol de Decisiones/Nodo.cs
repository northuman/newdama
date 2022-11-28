using System.Collections;
using System.Collections.Generic;

namespace ArbolDecisiones {

    public enum EstadoNodo { EJECUTANDO, EXITO, FALLO }

    public class Nodo {

        protected EstadoNodo estado;

        public Nodo padre;
        protected List<Nodo> hijos = new List<Nodo>();

        private Dictionary<string, object> contextoDatos = new Dictionary<string, object>();

        public Nodo() {

            padre = null;
        }

        public Nodo(List<Nodo> hijos) {

            foreach(Nodo hijo in hijos)
                Enlazar(hijo);
        }

        private void Enlazar(Nodo nodo) {

            nodo.padre = this;
            hijos.Add(nodo);
        }

        public virtual EstadoNodo Evaluar() => EstadoNodo.FALLO;

        public void AnyadirDato(string clave, object valor) {

            contextoDatos[clave] = valor;
        }

        public object GetDato(string clave) {

            object valor = null;

            if(contextoDatos.TryGetValue(clave, out valor))
                return valor;

            Nodo nodo = padre;

            while(nodo != null) {

                valor = nodo.GetDato(clave);

                if(valor != null) 
                    return valor;

                nodo = nodo.padre;
            }

            return null;
        }

        public bool BorrarDato(string clave) {

            if(contextoDatos.ContainsKey(clave)) {

                contextoDatos.Remove(clave);
                return true;
            }

            Nodo nodo = padre;

            while(nodo != null) {

                bool borrado = nodo.BorrarDato(clave);

                if(borrado) 
                    return true;

                nodo = nodo.padre;
            }

            return false;
        }
    }
}
