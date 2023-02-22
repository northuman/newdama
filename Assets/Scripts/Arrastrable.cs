using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class Arrastrable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler {
    
    public Transform padreOriginal = null;
    public Transform padrePlaceholder = null;

    GameObject placeholder = null;

    public Vector3 escalaOriginal;
    public int fuerzaTemp;
    public int resistenciaTemp;
    public bool cartaGirada = false;
    public bool cartaMuerta = false;
    public bool cartaEnMano = true;
    public bool atacando = false;
    public bool bloqueando = false;
    public bool mareo = false;
    public static Arrastrable bloqueador = null;
    public static Arrastrable atacante = null;
    public List<Arrastrable> bloqueadaPor = null;

    //Esto se usara para saber si una carta al jugarse ira al cementerio o si permanecera en la mesa
    public enum TipoCarta {CRIATURA, CONJURO, INSTANTANEO, ARTEFACTO, ENCANTEMIENTO, TIERRA, NULO};
    public TipoCarta tipoCarta = TipoCarta.NULO;

    public Jugador propietario;
    public Jugador jugador;
    public IA oponente;
    public Carta carta;
    public MostrarDatosCarta datosCarta;
    public List<Efecto> efectos;
    public Efecto resolviendo;

    private float update = 0.0f;

    private void Start() {
        
        escalaOriginal = this.transform.localScale;
        bloqueadaPor = new List<Arrastrable>();
        datosCarta = gameObject.GetComponent<MostrarDatosCarta>();
        carta = datosCarta.carta;
        efectos = ClonarEfectos(carta.efectos);
        ResetearEstadisticas();
        ObtenerTipoCarta();
        jugador = GameObject.Find("Jugador").GetComponent<Jugador>();
        oponente = GameObject.Find("Oponente").GetComponent<IA>();
    }

    private void Update() {
        
        update += Time.deltaTime;

        if(resolviendo) {

            if(update > 2.0f) {

                update = 0.0f;
                ResolverEfecto(resolviendo);
                resolviendo = null;
            }
        }

        else if (update > 1.0f) {

            update = 0.0f;
            ComprobarEfecto();
        }
    }

    public List<Efecto> ClonarEfectos(List<Efecto> es) {

        List<Efecto> listaClonada = new List<Efecto>();

        foreach(Efecto e in es) {

            if(e) { listaClonada.Add(Instantiate(e)); }
        }

        return listaClonada;
    }

    public void ComprobarEfecto() {

        //Recorrer Todos los efectos
        //Comprobar su condicion
        //Si es el momento adecuado activar el efecto

        foreach(Efecto efecto in efectos) {

            if(efecto) {

                switch(efecto.condicion) {
                    

                    case 0: //Nada mas la carta entra al campo de batalla

                        if(tipoCarta == TipoCarta.CONJURO) {

                            if(!efecto.resuleto && this.transform.parent == GameObject.Find("Pila").transform) {

                                resolviendo = efecto;
                            }
                        }
                        break;
                }
            }
        }
    }

    public void ResolverEfecto(Efecto efecto) {

        efecto.resuleto = true;
        Debug.Log("Resolvemos Efecto de tipo: " + efecto.habilidad);

        switch(efecto.habilidad) {

            case 0:

                StartCoroutine(propietario.RobarCartas(efecto.cantidad));
                Debug.Log("Intento Robar Cartas");
                break;

            case 1:
                Debug.Log("Descartar " + efecto.cantidad + " cartas");
                StartCoroutine(propietario.DescartarCartas(efecto.cantidad));
                break;
        }
    }

    public void ResetearEstadisticas() {

        fuerzaTemp = this.carta.fuerza;
        resistenciaTemp = this.carta.resistencia;
    }

    public void OnBeginDrag(PointerEventData datosEvento) {

        AumentarTamanyoCarta();

        CrearPlaceholder();

        EnderezarCarta();

        //Guardamos donde estaba la carta originalmente y lo copiamos al placeholder
        padreOriginal = this.transform.parent;
        padrePlaceholder = padreOriginal;

        //Cambiamos el padre de la carta al Canvas para que las demas cartas se reposicionen
        this.transform.SetParent(this.transform.parent.parent);

        //Cogemos el CanvasGroup de la carta y evitamos que bloquee el Raycast. Asi los paneles podran ver cuando soltamos la carta sobre ellos
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData datosEvento) {

        this.transform.position = datosEvento.position;

        if(cartaEnMano)
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

                if(propietario.descartando == true) {

                    cartaMuerta = true;
                    IrCementerio();
                    propietario.DescartarCarta();
                }

                else {

                    if(tipoCarta == TipoCarta.TIERRA && gameObject.transform.parent == DropZone.tierrasJugador.transform) {

                        if(!cartaGirada) {

                            propietario.AnyadirMana(carta);
                            GirarCarta();
                            this.transform.SetAsLastSibling();
                        }
                    }

                    if(tipoCarta == TipoCarta.CRIATURA && Partida.faseActual == Partida.Fase.COMBATE && 
                        Partida.momentoCombate == Partida.Combate.ATACANTES && Partida.turno == Partida.Turno.JUGADOR
                        && propietario == jugador && mareo == false) {

                        if(!cartaGirada) {

                            Atacar();
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
    }

    public void Atacar() {

        GirarCarta();
        atacando = true;
    }

    public void InflingirDanyo(Jugador jugador) {

        jugador.vida -= carta.fuerza;
        jugador.ActualizarVida();
    }

    public void Combate(Arrastrable otra) {

        //Ataque y vida de ambos
        this.resistenciaTemp = this.resistenciaTemp - otra.fuerzaTemp;  
        otra.resistenciaTemp = otra.resistenciaTemp - this.fuerzaTemp;

        if(Partida.momentoCombate == Partida.Combate.ORDEN_BLOQUEADORES) {

            if(otra.resistenciaTemp < 0) { 

                this.fuerzaTemp = -otra.resistenciaTemp; 
                otra.resistenciaTemp = 0;

                if(this.fuerzaTemp < 0) { this.fuerzaTemp = 0; } 
            }
        }

        this.datosCarta.ActualizarEstadisticas();
        otra.datosCarta.ActualizarEstadisticas();

        if(this.resistenciaTemp <= 0) {

            this.cartaMuerta = true;
        }

        if(otra.resistenciaTemp <= 0) {

            otra.cartaMuerta = true;
        }
    }

    public void GirarCarta() {

        Vector3 eulerAngles = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, -90f);
        cartaGirada = true;
    }

    public void EnderezarCarta() {

        Vector3 eulerAngles = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, 0f);
    }

    void AumentarTamanyoCarta() {

        escalaOriginal = this.transform.localScale;

        this.transform.localScale = new Vector3(2f, 2f, 2f);
    }

    void ReducirTamanyoCarta() {

        this.transform.localScale = escalaOriginal;
    }

    public void CambiarEscala(float e) {

        this.escalaOriginal *= e;
        this.transform.localScale = escalaOriginal;
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
            mareo = true;
        }

        else {

            //padreOriginal = DropZone.tierrasJugador.transform;
            this.transform.SetParent(propietario.goTierras.transform);
        }
    }

    public void NuevoPadre(Transform padre) {

        this.transform.SetParent(padre);
        padreOriginal = padre;
    }

    public void IrCementerio() {

        if(this.cartaMuerta) {

            padreOriginal = propietario.goCementerio.transform;
            CambiarEscala(0.6f);
            if(cartaGirada) { EnderezarCarta(); }
            this.transform.SetParent(padreOriginal);
        }
    }

    public Carta GetCarta() {

        return this.GetComponent<MostrarDatosCarta>().carta;
    }
}
