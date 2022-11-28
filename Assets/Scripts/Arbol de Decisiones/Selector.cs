using System.Collections;
using System.Collections.Generic;

namespace ArbolDecisiones {

    public class Selector : Nodo {

        public Selector() : base() {}
        public Selector(List<Nodo> hijos) : base(hijos) {}

        public override EstadoNodo Evaluar() {

            foreach(Nodo hijo in hijos) {

                switch(hijo.Evaluar()) {

                    case EstadoNodo.FALLO:
                        continue;
                    case EstadoNodo.EXITO:
                        estado = EstadoNodo.EXITO;
                        return estado;
                    case EstadoNodo.EJECUTANDO:
                        estado = EstadoNodo.EJECUTANDO;
                        return estado;
                    default:
                        continue;
                }
            }

            estado = EstadoNodo.FALLO;
            return estado;
        }
    }
}
