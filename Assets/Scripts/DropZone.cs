using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public enum TipoZona {MANO, CRIATURAS, TIERRAS, ENCANTAMIENTOS, OTROS};

    public TipoZona tipoZona;
    
    public void OnPointerEnter(PointerEventData datosEvento) {

        //Debug.Log("OnPointerEnter to " + gameObject.name);

        if(datosEvento.pointerDrag == null) { return; }

        Arrastrable carta = datosEvento.pointerDrag.GetComponent<Arrastrable>();

        if(carta != null) {

            if(tipoZona == TipoZona.CRIATURAS && carta.tipoCarta == Arrastrable.TipoCarta.CRIATURA) {

                carta.padrePlaceholder = this.transform;
            }

            else if(tipoZona == TipoZona.MANO) {

                carta.padrePlaceholder = this.transform;
            }
        }
    }

    public void OnPointerExit(PointerEventData datosEvento) {

        //Debug.Log("OnPointerExit to " + gameObject.name);

        if(datosEvento.pointerDrag == null) { return; }

        Arrastrable carta = datosEvento.pointerDrag.GetComponent<Arrastrable>();

        if(carta != null && carta.padrePlaceholder == this.transform) {

            if(tipoZona == TipoZona.CRIATURAS && carta.tipoCarta == Arrastrable.TipoCarta.CRIATURA) {

                carta.padrePlaceholder = this.transform;
            }

            else if(tipoZona == TipoZona.MANO) {

                carta.padrePlaceholder = carta.padreOriginal;
            }
        }
    }

    public void OnDrop(PointerEventData datosEvento) {

        Debug.Log(datosEvento.pointerDrag.name + " fue soltado sobre " + gameObject.name);

        Arrastrable carta = datosEvento.pointerDrag.GetComponent<Arrastrable>();

        if(carta != null) {

            if(tipoZona == TipoZona.CRIATURAS && carta.tipoCarta == Arrastrable.TipoCarta.CRIATURA) {

                carta.padreOriginal = this.transform;
            }

            else if(tipoZona == TipoZona.MANO) {

                carta.padreOriginal = this.transform;
            }
        }
    }
}
