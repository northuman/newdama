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
    public bool tamanyoAumentado = false;
    public bool visible = true;
    public bool cartaGirada = false;
    public bool cartaMuerta = false;
    public bool cartaEnMano = true;
    public bool atacando = false;
    public bool bloqueando = false;
    public bool mareo = false;
    public bool buffoJugadorAplicado = false;
    public static Arrastrable bloqueador = null;
    public static Arrastrable atacante = null;
    public List<Arrastrable> bloqueadaPor = null;
    public Arrastrable bloqueandoA = null;

    //Esto se usara para saber si una carta al jugarse ira al cementerio o si permanecera en la mesa
    public enum TipoCarta {CRIATURA, CONJURO, INSTANTANEO, ARTEFACTO, ENCANTEMIENTO, TIERRA, NULO};
    public TipoCarta tipoCarta = TipoCarta.NULO;

    public Jugador propietario;
    public static Jugador jugador;
    public static IA oponente;
    public Carta carta;
    public MostrarDatosCarta datosCarta;
    public List<Efecto> efectos;
    //public bool efectosResueltos = false;
    public int cantidadResueltos = 0;

    public GameObject clon;
    public GameObject sombreado;

    public List<Efecto> resolviendo = new List<Efecto>();

    private float updateTime = 0.0f;

    private void Start() {
        
        escalaOriginal = this.transform.localScale;
        bloqueadaPor = new List<Arrastrable>();
        datosCarta = gameObject.GetComponent<MostrarDatosCarta>();
        carta = datosCarta.carta;
        efectos = ClonarEfectos(carta.efectos);
        ResetearEstadisticas();
        ObtenerTipoCarta();
        if(SceneManager.GetActiveScene().name == "Arena") {

            jugador = GameObject.Find("Jugador").GetComponent<Jugador>();
            oponente = GameObject.Find("Oponente").GetComponent<IA>();
        }

        if(!(propietario is IA))
        gameObject.transform.Find("Dorso").gameObject.SetActive(false);
        sombreado = gameObject.transform.Find("Sombreado").gameObject;
        sombreado.SetActive(false);
        sombreado.GetComponent<Image>().color = Color.black;
    }

    private void Update() {

        if(!cartaMuerta) {

            updateTime += Time.deltaTime;

            if(resolviendo.Count > 0) {

                if(updateTime > 2.0f) {

                    updateTime = 0.0f;
                    ResolverEfecto(resolviendo[0]);
                }
            }

            else if (updateTime > 1.0f) {

                updateTime = 0.0f;
                ComprobarEfecto();
            }
        }
    }

    public void MostrarCarta() {

        visible = true;
        gameObject.transform.Find("Dorso").gameObject.SetActive(false);
    }

    public List<Efecto> ClonarEfectos(List<Efecto> es) {

        List<Efecto> listaClonada = new List<Efecto>();

        if(es != null) {

            foreach(Efecto e in es) {

                if(e) { listaClonada.Add(Instantiate(e)); }
            }
        }

        return listaClonada;
    }

    public void ComprobarEfecto() {

        //Recorrer Todos los efectos
        //Comprobar su condicion
        //Si es el momento adecuado activar el efecto

        if(!cartaEnMano && !cartaMuerta) {

            cantidadResueltos = 0;

            foreach(Efecto efecto in efectos) {

                if(efecto) {

                    //if(efecto.resuelto || efecto is Activado) { cantidadResueltos++; } //|| efecto is Aura

                    switch(efecto.condicion) {

                        case -1: //Clickar en algo

                            if(tipoCarta == TipoCarta.CONJURO) {

                                resolviendo.Add(efecto);
                                propietario.clickable = efecto;
                            }
                            break;

                        case 0: //Nada mas la carta entra al campo de batalla

                            updateTime = 2f;

                            if(tipoCarta == TipoCarta.CONJURO) {

                                if(!efecto.resuelto && this.transform.parent == GameObject.Find("Pila").transform) {

                                    resolviendo.Add(efecto);
                                    efecto.resuelto = true;
                                }
                            }

                            if(tipoCarta == TipoCarta.CRIATURA) {

                                if(!efecto.resuelto && !cartaEnMano) { //&& this.transform.parent == propietario.goCriaturas
                                    
                                    resolviendo.Add(efecto);
                                    efecto.resuelto = true;
                                }
                            }

                            break;
                        
                        case 1: //Ser un Aura

                            if(tipoCarta == TipoCarta.ENCANTEMIENTO && efecto is Aura) {

                                if(!cartaEnMano && !efecto.resuelto) {

                                    propietario.AnyadirAura((Aura)efecto);
                                    efecto.resuelto = true;
                                }
                            }

                            if(tipoCarta == TipoCarta.CRIATURA && efecto is Aura) {

                                if(!cartaEnMano && !efecto.resuelto) {

                                    propietario.AnyadirAura((Aura)efecto);
                                    efecto.resuelto = true;
                                }
                            }
                            break;

                        case 2: //Al atacar
                            
                            if (tipoCarta == TipoCarta.CRIATURA
                                && atacando 
                                && Partida.momentoCombate == Partida.Combate.DANYO) {

                                if(!efecto.resuelto) {

                                    resolviendo.Add(efecto);
                                    efecto.resuelto = true;
                                }
                            }
                            break;

                        case 3: //Cuando una criatura entra al campo de batalla
                            
                            if  (tipoCarta == TipoCarta.CRIATURA
                                && propietario.criaturaJugada
                                && propietario.criaturaJugada != this) {
                                
                                resolviendo.Add(efecto);
                                //propietario.criaturaJugada = null;
                            }

                            break;
                    }
                }
            }

            if(tipoCarta == TipoCarta.CONJURO && EfectosResueltos()) {

                cartaMuerta = true;
                IrCementerio();
            }
        }
    }

    public bool EfectosResueltos() {

        bool resueltos = true;

        foreach(Efecto e in efectos) {

            if(!e.resuelto) { resueltos = false; }
        }

        return resueltos;
    }

    public void ResolverEfecto(Efecto efecto) {

        switch(efecto.habilidad) {

            case 0: //Robar X cartas
                
                StartCoroutine(propietario.RobarCartas(efecto.cantidad));
                efecto.resuelto = true;
                Debug.Log("Intento Robar Cartas");
                break;

            case 1: //Descartar X cartas
                Debug.Log("Descartar " + efecto.cantidad + " cartas");
                StartCoroutine(propietario.DescartarCartas(efecto));
                break;

            case 2: //Equipar
                Debug.Log("Selecciona la criatura a la que equipar la carta");
                StartCoroutine(propietario.EquiparCarta(efecto));
                break;

            case 3: //Hacer X danyos a cara
                Debug.Log(propietario.name + " hace danyo a su oponente");
                propietario.oponente.RecibirDanyo(efecto.cantidad);
                break;

            case 4: //Dar palabra clave
                Debug.Log("Otorga palabra clave a " + efecto.cantidad + " criatura/as");
                StartCoroutine(propietario.DarPalabrasClave(efecto));
                break;

            case 5: //Prisa
                Debug.Log(this.GetCarta().nombreCarta + " Tiene prisa");
                mareo = false;
                propietario.criaturasActivas++;
                break;

            case 6: //Ganar Vidas
                Debug.Log(propietario.name + " gana " + efecto.cantidad + " vida/as");
                propietario.GanarVidas(efecto.cantidad);
                resolviendo.Remove(efecto);
                propietario.criaturaJugada = null;
                break;
            case 7: //Danyo a objetivo
                Debug.Log("Selecciona a qué objetivo deseas realizar el daño");
                StartCoroutine(propietario.HacerDanyo(efecto));
                break;
        }
        resolviendo.Remove(efecto);
    }

    public void ResetearEstadisticas() {

        fuerzaTemp = this.carta.fuerza;
        resistenciaTemp = this.carta.resistencia;
        buffoJugadorAplicado = false;
    }

    public void AplicarBuffo(int[] estadisticas) {

        this.fuerzaTemp += estadisticas[0];
        this.resistenciaTemp += estadisticas[1];
    }

    public void OnBeginDrag(PointerEventData datosEvento) {

        AumentarTamanyoCarta();

        CrearPlaceholder();

        VoltearCarta();

        DropZone.carta = this;

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

        DropZone.carta = null;

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

                List<Activado> activados = GetActivados();

                if(propietario.descartando) {

                    cartaMuerta = true;
                    cartaEnMano = false;
                    IrCementerio();
                    propietario.DescartarCarta();
                }

                else if(propietario.equipando && resolviendo.Count == 0) {

                    propietario.cartaEquipada = this;
                    propietario.equipando = false;
                }

                else if(propietario.equipando && resolviendo.Count > 0) {

                    if(CumpleCondicionColores(resolviendo[0])) {

                        propietario.cartaEquipada = this;
                        propietario.equipando = false;
                    }
                }

                else if(propietario.oponente.equipando) {

                    propietario.oponente.cartaEquipada = this;
                    propietario.oponente.equipando = false;
                }

                //Si tiene un Efecto Activado
                else if(!cartaEnMano && activados.Count == 1) {

                    if(propietario.tierras.SuficienteMana(activados[0])) {

                        resolviendo.Add(activados[0]);

                        if(activados[0].habilidad == 2) {

                            propietario.equipo = this;
                        }
                    }
                }
                
                else {

                    if(tipoCarta == TipoCarta.TIERRA && gameObject.transform.parent == DropZone.tierrasJugador.transform) {

                        if(!cartaGirada) {

                            propietario.AnyadirMana(carta);
                            GirarCarta();
                            this.transform.SetAsLastSibling();
                        }
                    }

                    else if(tipoCarta == TipoCarta.CRIATURA && Partida.faseActual == Partida.Fase.COMBATE && 
                        Partida.momentoCombate == Partida.Combate.ATACANTES && Partida.turno == Partida.Turno.JUGADOR
                        && propietario == jugador && mareo == false) {

                        if(!cartaGirada) {

                            Atacar();
                        }
                    }

                    else if(tipoCarta == TipoCarta.CRIATURA && Partida.faseActual == Partida.Fase.COMBATE 
                            && Partida.momentoCombate == Partida.Combate.ORDEN_BLOQUEADORES) {

                        atacante.Combate(this);
                        this.transform.SetParent(this.padreOriginal);

                        if(Partida.cajaDialogo.transform.childCount <= 3) {

                            Partida.cajaDialogo.SetActive(false);
                            Partida.momentoCombate = Partida.Combate.DANYO;
                        }
                    }

                    else if(tipoCarta == TipoCarta.CRIATURA && Partida.momentoCombate == Partida.Combate.BLOQUEADORES
                            && Partida.turno == Partida.Turno.OPONENTE) {

                        if(propietario == jugador) {

                            if(!atacante) {

                                Debug.Log("PRIMERO DEBES SELECCIONAR LA CRIATURA A LA QUE DESEAS BLOQUEAR");
                            }

                            else if(bloqueandoA) {

                                Debug.Log("ESTA CRIATURA YA ESTA BLOQUEANDO A " + this.bloqueandoA.GetCarta().nombreCarta);
                            }

                            else {

                                atacante.AnyadirBloqueador(this);
                                Debug.Log("BLOQUEO ACEPTADO");
                            }
                        }

                        else if(propietario == oponente) {

                            atacante = this;
                        }
                    }
                }
            }

            if(datosEvento.button == PointerEventData.InputButton.Right) {

                if(tipoCarta == TipoCarta.CRIATURA && Partida.faseActual == Partida.Fase.COMBATE 
                    && Partida.momentoCombate == Partida.Combate.ATACANTES && Partida.turno == Partida.Turno.JUGADOR
                    && propietario == jugador && atacando == true) {

                    CancelarAtaque();
                }

                else if(tipoCarta == TipoCarta.CRIATURA && Partida.faseActual == Partida.Fase.COMBATE
                        && Partida.momentoCombate == Partida.Combate.BLOQUEADORES
                        && Partida.turno == Partida.Turno.OPONENTE
                        && propietario == jugador) {

                    this.EliminarBloqueo();
                    Debug.Log("BLOQUEO ELIMINADO");
                }
            }
        }
    }

    public void Atacar() {

        GirarCarta();
        atacando = true;
    }

    public void CancelarAtaque() {

        EnderezarCarta();
        atacando = false;
    }

    public void InflingirDanyo(Jugador jugador) {

        jugador.vida -= fuerzaTemp;
        jugador.ActualizarVida();
    }

    public void AnyadirBloqueador(Arrastrable b) {

        if(!(this.bloqueadaPor.Contains(b))) {

            this.bloqueadaPor.Add(b);
            b.bloqueandoA = this;

            Color aleatorio;

            if(this.sombreado.GetComponent<Image>().color == Color.black) {

                do {
                    aleatorio = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 0.5f);
                } while (aleatorio == Color.black);
                
                this.sombreado.GetComponent<Image>().color = aleatorio;
            }

            else {

                aleatorio = this.sombreado.GetComponent<Image>().color;
            }

            b.sombreado.GetComponent<Image>().color = aleatorio;
            this.sombreado.SetActive(true);
            b.sombreado.SetActive(true);
        }
    }

    public void EliminarBloqueo() {

        bloqueandoA.bloqueadaPor.Remove(this);
        if(bloqueandoA.bloqueadaPor.Count == 0) {

            bloqueandoA.sombreado.SetActive(false);
            bloqueandoA.sombreado.GetComponent<Image>().color = Color.black;
        }

        this.sombreado.SetActive(false);
        this.sombreado.GetComponent<Image>().color = Color.black;
        bloqueandoA = null;
    }

    public void Combates() {

        foreach(Arrastrable a in bloqueadaPor) {

            this.Combate(a);
        }
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
        Debug.Log("SALGO");
    }

    public void EnderezarCarta() {

        Vector3 eulerAngles = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, 0f);
        cartaGirada = false;
    }

    public void VoltearCarta() { //Solo a la hora de arrastrar la carta

        Vector3 eulerAngles = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, 0f);
        Debug.Log("ENTRO");
    }

    void AumentarTamanyoCarta() {

        tamanyoAumentado = true;
        this.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
    }

    void ReducirTamanyoCarta() {

        tamanyoAumentado = false;
        this.transform.localScale = escalaOriginal;
    }

    public void CambiarEscala(float e) {

        /*this.escalaOriginal *= e;
        this.transform.localScale = escalaOriginal;*/
        this.transform.localScale = escalaOriginal * e;
        escalaOriginal = this.transform.localScale;
    }

    void ObtenerTipoCarta() {

        string cadenaTipo = this.GetComponent<MostrarDatosCarta>().carta.tipoCarta;

        if(cadenaTipo != null) {

            if(cadenaTipo.Contains("Criatura")) { tipoCarta = TipoCarta.CRIATURA; }
            else if(cadenaTipo.Contains("Conjuro")) { tipoCarta = TipoCarta.CONJURO; }
            else if(cadenaTipo.Contains("Instantáneo")) { tipoCarta = TipoCarta.INSTANTANEO; }
            else if(cadenaTipo.Contains("Artefacto")) { tipoCarta = TipoCarta.ARTEFACTO; }
            else if(cadenaTipo.Contains("Encantamiento")) { tipoCarta = TipoCarta.ENCANTEMIENTO; }
            else if(cadenaTipo.Contains("Tierra")) { tipoCarta = TipoCarta.TIERRA; }
        }
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
            cartaEnMano = false;
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

            cartaEnMano = false;
            padreOriginal = propietario.goCementerio.transform;
            CambiarEscala(0.6f);

            if(cartaGirada) { EnderezarCarta(); }
            this.transform.SetParent(padreOriginal);
        }
    }

    public Carta GetCarta() {

        return this.GetComponent<MostrarDatosCarta>().carta;
    }

    public void ActualizarEstadisticas() {

        if(tipoCarta == TipoCarta.CRIATURA)
            GetComponent<MostrarDatosCarta>().ActualizarEstadisticas();
    }

    public List<Activado> GetActivados() {

        List<Activado> activados = new List<Activado>();

        foreach(Efecto efecto in efectos) {

            if(efecto is Activado) {

                activados.Add((Activado)efecto);
            }
        }

        return activados;
    }

    public bool CumpleCondicionColores(Efecto efecto) {

        if(efecto == null) {

            Debug.Log("ESTO FALLA");
        }

        if(efecto.coloresNoAfectados == null || efecto.coloresNoAfectados.Length == 0) {

            return true; 
        }

        else if(CompararColores(GetColores(), efecto.coloresNoAfectados)) {
            
            return true;
        }

        return false;
    }

    public bool[] GetColores() {

        bool[] colores = new bool[5];

        for(int i=0; i<5; i++) {

            if(i>0 && GetCarta().costeMana[i] > 0) { colores[i] = true; }
            else { colores[i] = false; }
        }

        return colores;
    }

    public bool CompararColores(bool[] coloresCarta, bool[] coloresNoAfectados) { //Si la carta debe ser afecta devuelve verdadero

        bool afectada = true;

        for(int i=0; i<5; i++) {

            if(coloresNoAfectados[i] && coloresCarta[i]) { 
                
                afectada = false;
                break;
            }
        }

        return afectada;
    }

    public void EscribirNombreEquipada(Efecto efecto) {

        if(efecto.objetivo) {

            string buscar = "Marco Carta/Caracteristicas/Marco Descripcion/Caja Descripcion/Extras";
            string texto = "Equipada a " + efecto.objetivo.GetCarta().nombreCarta;
            gameObject.transform.Find(buscar).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = texto;
        }

        else {

            string buscar = "Marco Carta/Caracteristicas/Marco Descripcion/Caja Descripcion/Extras";
            gameObject.transform.Find(buscar).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
        }
    }

    public void HacerDanyo(Efecto efecto) {

        if(efecto.objetivo) {

            this.resistenciaTemp -= efecto.cantidad;
            this.ActualizarEstadisticas();
            
            if(resistenciaTemp <= 0) {

                this.cartaMuerta = true;
                this.IrCementerio(); 
            }
        }
    }

    public void DarPalabrasClave(Efecto efecto) {

        if(efecto.objetivo) {

            string buscar = "Marco Carta/Caracteristicas/Marco Descripcion/Palabras Clave";
            string texto = "";
            
            foreach(string palabraClave in efecto.otorgarPalabrasClave) {

                //AnyadirEfectoPalabraClave(palabraClave);

                if(texto == "")
                    texto += palabraClave + ".";
                else
                    texto += " " + palabraClave + ".";

                switch(palabraClave) {

                    case "Prisa":
                        mareo = false;
                        propietario.criaturasActivas++;
                        break;
                }
            }
            if(gameObject.transform.Find(buscar).GetComponentInChildren<TMPro.TextMeshProUGUI>())
            gameObject.transform.Find(buscar).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = texto;
            else
            Debug.Log("Algo ha fallado");
        }
    }
}
