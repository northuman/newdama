using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ModificarBaraja : MonoBehaviour, IPointerClickHandler {

    public EditorBaraja editorBaraja;
    public Baraja baraja;
    public int id = 0;

    void Start() {

        editorBaraja = GameObject.Find("Editor Baraja").GetComponent<EditorBaraja>();
    }

    public void OnPointerClick(PointerEventData datosEvento) {

        if(datosEvento.button == PointerEventData.InputButton.Left) {

            editorBaraja.EditarBaraja(id);
        }
    }

    public void EliminarBaraja() {

        Destroy(baraja.gameObject);
        Destroy(baraja);
        Destroy(this.gameObject);
    }
}
