using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Arrastrable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler {
    
    public Transform padreOriginal = null;
    public Transform padrePlaceholder = null;

    GameObject placeholder = null;

    bool cartaAmpliada = false;
    Transform transformCarta;

    //Esto se usara para saber si una carta al jugarse ira al cementerio o si permanecera en la mesa
    public enum TipoCarta {CRIATURA, CONJURO, INSTANTANEO, ARTEFACTO, ENCANTEMIENTO, TIERRA, NULO};
    public TipoCarta tipoCarta = TipoCarta.NULO;

    void Start() {
        
        transformCarta = this.gameObject.transform;
    }

    public void OnBeginDrag(PointerEventData datosEvento) {

        if(!cartaAmpliada) { AumentarTamanyoCarta(); }

        ObtenerTipoCarta();

        CrearPlaceholder();

        //Guardamos los datos del padre original de la carta
        padreOriginal = this.transform.parent;
        padrePlaceholder = padreOriginal;

        //Cambiamos el padre de la carta al Canvas para que las demas cartas se reposicionen
        this.transform.SetParent(this.transform.parent.parent);

        //Cogemos el CanvasGroup de la carta y evitamos que bloquee el Raycast. Asi los paneles podran ver cuando soltamos la carta sobre ellos
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        //Buscamos todas las DropZone para utilizarlas mas adelante
        DropZone[] zonas = GameObject.FindObjectsOfType<DropZone>();

        //Filtramos para hacer que brillen por ejemplo y asi el jugador sepa donde soltar la carta
    }

    public void OnDrag(PointerEventData datosEvento) {

        this.transform.position = datosEvento.position;

        RecolocarCarta();
    }

    public void OnEndDrag(PointerEventData datosEvento) {

        if(cartaAmpliada) { ReducirTamanyoCarta(); }

        this.transform.SetParent(padreOriginal);

        //Devolvemos la carta a la posicion correspondiente del layout element
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

        //Para poder volver a coger la misma carta
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Destroy(placeholder);
    }

    public void OnPointerClick(PointerEventData datosEvento) {

        if(!cartaAmpliada) { AumentarTamanyoCarta(); }
        else { ReducirTamanyoCarta(); }
    }

    void AumentarTamanyoCarta() {

        transformCarta.localScale *= 2;
        transformCarta.Translate(Vector3.up * 300);
        cartaAmpliada = true;
    }

    void ReducirTamanyoCarta() {

        transformCarta.Translate(Vector3.down * 300);
        transformCarta.localScale /= 2;
        cartaAmpliada = false;
    }

    void ObtenerTipoCarta() {

        //tipoCarta = this.GetComponent<MostrarDatosCarta>();
        string cadenaTipo = this.GetComponent<MostrarDatosCarta>().carta.tipoCarta;

        if(cadenaTipo.Contains("Criatura")) { tipoCarta = TipoCarta.CRIATURA; }
        else if(cadenaTipo.Contains("Conjuro")) { tipoCarta = TipoCarta.CONJURO; }
        else if(cadenaTipo.Contains("Instantáneo")) { tipoCarta = TipoCarta.INSTANTANEO; }
        else if(cadenaTipo.Contains("Artefacto")) { tipoCarta = TipoCarta.ARTEFACTO; }
        else if(cadenaTipo.Contains("Encantamiento")) { tipoCarta = TipoCarta.ENCANTEMIENTO; }
        else if(cadenaTipo.Contains("Tierra")) { tipoCarta = TipoCarta.TIERRA; }
    }

    void CrearPlaceholder() {

        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);

        //Le anyadimos el componente layout para que pueda comportarse igual que las cartas en los layout group
        LayoutElement layoutElement = placeholder.AddComponent<LayoutElement>();
        layoutElement.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        layoutElement.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        layoutElement.flexibleWidth = 0;
        layoutElement.flexibleHeight = 0;

        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
    }

    void RecolocarCarta() {

        if(placeholder.transform.parent != padrePlaceholder)
            placeholder.transform.SetParent(padrePlaceholder);

        int nuevoIndice = padreOriginal.childCount;

        for(int i=0; i<padrePlaceholder.childCount; i++) {

            if(this.transform.position.x < padrePlaceholder.GetChild(i).transform.position.x) {

                nuevoIndice = i;

                if(placeholder.transform.GetSiblingIndex() < nuevoIndice) { nuevoIndice--; }
                break;
            }
        }

        placeholder.transform.SetSiblingIndex(nuevoIndice);
    }
}
