using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ImagenCartaEditor : MonoBehaviour, IPointerClickHandler {
    
    public EditorBaraja editorBaraja;
    public Carta carta;

    public void OnPointerClick(PointerEventData datosEvento) {

        if(datosEvento.button == PointerEventData.InputButton.Right) {

            //editorBaraja.ReducirNumero o BorrarImagen
            if(carta)
                editorBaraja.QuitarCarta(carta);
        }
    }
}
