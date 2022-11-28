using System.Collections;
using System.Collections.Generic;

namespace ArbolDecisiones {

    public class Secuencia : Nodo {

        public Secuencia() : base() {}
        public Secuencia(List<Nodo> hijos) : base(hijos) {}

        public override EstadoNodo Evaluar() {

            bool algunHijoEjecutando = false;

            foreach(Nodo hijo in hijos) {

                switch(hijo.Evaluar()) {

                    case EstadoNodo.FALLO:
                        estado = EstadoNodo.FALLO;
                        return estado;
                    case EstadoNodo.EXITO:
                        continue;
                    case EstadoNodo.EJECUTANDO:
                        algunHijoEjecutando = true;
                        continue;
                    default:
                        estado = EstadoNodo.EXITO;
                        return estado;
                }
            }

            estado = algunHijoEjecutando ? EstadoNodo.EJECUTANDO : EstadoNodo.EXITO;
            return estado;
        }
    }
}
