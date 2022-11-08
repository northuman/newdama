using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Arrastrable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler {
    
    public Transform padreOriginal = null;
    public Transform padrePlaceholder = null;

    GameObject placeholder = null;

    Transform transformCarta;
    public bool cartaGirada = false;
    public bool cartaMuerta = false;
    public static Arrastrable bloqueador = null;
    public static Arrastrable atacante = null;
    public List<Arrastrable> bloqueadaPor = null;

    //Esto se usara para saber si una carta al jugarse ira al cementerio o si permanecera en la mesa
    public enum TipoCarta {CRIATURA, CONJURO, INSTANTANEO, ARTEFACTO, ENCANTEMIENTO, TIERRA, NULO};
    public TipoCarta tipoCarta = TipoCarta.NULO;

    public Jugador propietario;
    public Carta carta;
    MostrarDatosCarta datosCarta;

    void Start() {
        
        transformCarta = this.gameObject.transform;
        bloqueadaPor = new List<Arrastrable>();
        datosCarta = gameObject.GetComponent<MostrarDatosCarta>();
        carta = datosCarta.carta;
        ObtenerTipoCarta();
    }

    public void OnBeginDrag(PointerEventData datosEvento) {

        AumentarTamanyoCarta();

        CrearPlaceholder();

        EnderezarCarta();

        //Guardamos los datos del padre original de la carta
        padreOriginal = this.transform.parent;
        padrePlaceholder = padreOriginal;

        //Cambiamos el padre de la carta al Canvas para que las demas cartas se reposicionen
        this.transform.SetParent(this.transform.parent.parent);

        //Cogemos el CanvasGroup de la carta y evitamos que bloquee el Raycast. Asi los paneles podran ver cuando soltamos la carta sobre ellos
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData datosEvento) {

        this.transform.position = datosEvento.position;

        RecolocarCarta();
    }

    public void OnEndDrag(PointerEventData datosEvento) {

        ReducirTamanyoCarta();

        if(cartaGirada) { GirarCarta(); }

        this.transform.SetParent(padreOriginal);

        //Devolvemos la carta a la posicion correspondiente del layout element
        if(padreOriginal == padrePlaceholder)
            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

        //Para poder volver a coger la misma carta
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Destroy(placeholder);
    }

    public void OnPointerClick(PointerEventData datosEvento) {

        if(SceneManager.GetActiveScene().name == "Arena") {

            if(datosEvento.button == PointerEventData.InputButton.Left) {

                if(tipoCarta == TipoCarta.TIERRA && gameObject.transform.parent == DropZone.tierrasJugador.transform) {

                    if(!cartaGirada) {

                        propietario.AnyadirMana(carta);
                        GirarCarta();
                        cartaGirada = true;
                    }
                }

                if(tipoCarta == TipoCarta.CRIATURA && Partida.faseActual == Partida.Fase.COMBATE && Partida.momentoCombate == Partida.Combate.ATACANTES 
                    && Partida.turno == Partida.Turno.JUGADOR) {

                    if(!cartaGirada) {

                        GirarCarta();
                        cartaGirada = true;
                    }
                }

                if(tipoCarta == TipoCarta.CRIATURA && Partida.faseActual == Partida.Fase.COMBATE && Partida.momentoCombate == Partida.Combate.BLOQUEADORES 
                    && Partida.turno == Partida.Turno.JUGADOR) {

                    if(!bloqueador) {

                        bloqueador = this;
                    }

                    else {

                        this.bloqueadaPor.Add(bloqueador);
                        bloqueador = null;
                    }
                }

                if(tipoCarta == TipoCarta.CRIATURA && Partida.faseActual == Partida.Fase.COMBATE 
                    && Partida.momentoCombate == Partida.Combate.ORDEN_BLOQUEADORES) {

                    atacante.Combate(this);
                    this.transform.SetParent(this.padreOriginal);

                    if(Partida.cajaDialogo.transform.childCount <= 3) {

                        Partida.cajaDialogo.SetActive(false);
                        Partida.momentoCombate = Partida.Combate.DANYO;
                    }
                }
            }
        }
    }

    public void InflingirDanyo() {

        Partida.oponente.vida -= carta.fuerza;
        Partida.oponente.ActualizarVida();
    }

    public void Combate(Arrastrable otra) {

        //Ataque y vida de ambos
        this.carta.resistenciaTemp = this.carta.resistenciaTemp - otra.carta.fuerzaTemp;  
        otra.carta.resistenciaTemp = otra.carta.resistenciaTemp - this.carta.fuerzaTemp;  

        if(Partida.momentoCombate == Partida.Combate.ORDEN_BLOQUEADORES) {

            if(otra.carta.resistenciaTemp < 0) { 

                this.carta.fuerzaTemp = this.carta.fuerzaTemp + otra.carta.resistenciaTemp; 
                otra.carta.resistenciaTemp = 0;

                if(this.carta.fuerzaTemp < 0) { this.carta.fuerzaTemp = 0; } 
            }
        }

        this.datosCarta.ActualizarEstadisticas();
        otra.datosCarta.ActualizarEstadisticas();

        if(this.carta.resistenciaTemp <= 0) {

            this.cartaMuerta = true;
        }

        if(otra.carta.resistenciaTemp <= 0) {

            otra.cartaMuerta = true;
        }
    }

    void GirarCarta() {

        Vector3 eulerAngles = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, -90f);
    }

    void EnderezarCarta() {

        Vector3 eulerAngles = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, 0f);
    }

    void AumentarTamanyoCarta() {

        transformCarta.localScale *= 2;
        //transformCarta.Translate(Vector3.down * 100);
    }

    void ReducirTamanyoCarta() {

        //transformCarta.Translate(Vector3.up * 100);
        transformCarta.localScale /= 2;
    }

    void ObtenerTipoCarta() {

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

        if(SceneManager.GetActiveScene().name == "Arena") {

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

    public void ColocarCarta() {

        if(this.tipoCarta == TipoCarta.CRIATURA) {

            //this.gameObject.transform = DropZone.criaturasJugador.transform;
            padreOriginal = propietario.goCriaturas.transform;
            this.transform.SetParent(padreOriginal);
        }

        else {

            //padreOriginal = DropZone.tierrasJugador.transform;
            this.transform.SetParent(propietario.goTierras.transform);
        }
    }

    public void AnyadirCartaPila() {

        padreOriginal = DropZone.pila.transform;
        Pila.pila.Push(this);
        Pila.cantidadCartas++;
    }

    public void IrCementerio() {

        if(this.cartaMuerta) {

            padreOriginal = propietario.goCementerio.transform;
            if(cartaGirada) { EnderezarCarta(); }
            this.transform.SetParent(padreOriginal);
        }
    }
}
