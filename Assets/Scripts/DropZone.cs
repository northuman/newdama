using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public static GameObject pila;
    public static GameObject manoJugador;
    public static GameObject tierrasJugador;
    public static GameObject criaturasJugador;
    public static GameObject encantamientosJugador;
    public static Arrastrable carta = null;

    public Jugador jugador;
    private float updateTime = 0.0f;
    

    private void Start() {
        
        pila = GameObject.Find("Pila");
        manoJugador = GameObject.Find("Mano Jugador");
        tierrasJugador = GameObject.Find("Tierras Jugador");
        criaturasJugador = GameObject.Find("Criaturas Jugador");
        encantamientosJugador = GameObject.Find("Encantamientos Jugador");
        if(SceneManager.GetActiveScene().name == "Arena")
            jugador = GameObject.Find("Jugador").GetComponent<Jugador>();
    }

    private void Update() {

        if(SceneManager.GetActiveScene().name == "Arena") {

            updateTime += Time.deltaTime;

            if(carta && updateTime > 1.0f) {

                AumentarColor(carta);
                updateTime = 0.0f;
            }

            if(!carta)
                ApagarColor();
        } 
    }
    
    public void OnPointerEnter(PointerEventData datosEvento) {

        if(datosEvento.pointerDrag == null) { return; }

        Arrastrable carta = datosEvento.pointerDrag.GetComponent<Arrastrable>();

        /*if(carta != null) {


        }*/
    }

    public void OnPointerExit(PointerEventData datosEvento) {

        if(datosEvento.pointerDrag == null) { return; }

        Arrastrable carta = datosEvento.pointerDrag.GetComponent<Arrastrable>();

        /*if(carta != null && carta.padrePlaceholder == this.transform) {


        }*/
    }

    public void OnDrop(PointerEventData datosEvento) {

        Arrastrable carta = datosEvento.pointerDrag.GetComponent<Arrastrable>();

        if(SceneManager.GetActiveScene().name == "Arena") {

            if(carta != null && Partida.faseActual != Partida.Fase.MULLIGAN) {

                if(carta.propietario == jugador && carta.cartaEnMano) {

                    if(carta.propietario.permitidoJugarCartas) {

                        if(this.gameObject.name != manoJugador.gameObject.name) {

                            if(carta.tipoCarta == Arrastrable.TipoCarta.TIERRA) {

                                if(carta.propietario.tierraDelTurnoJugada == false && this.transform.parent != manoJugador.transform) {

                                    carta.NuevoPadre(tierrasJugador.transform);
                                    //carta.CambiarEscala(0.75f);
                                    carta.cartaEnMano = false;
                                    carta.propietario.tierraDelTurnoJugada = true;
                                    tierrasJugador.GetComponent<DropZone>().Ordenar();
                                }

                                else {

                                    Debug.Log("Ya has jugado tierra este turno");
                                }
                            }

                            if(carta.tipoCarta == Arrastrable.TipoCarta.CRIATURA) {

                                bool jugarCarta = jugador.tierras.SuficienteMana(carta.GetCarta());

                                if(jugarCarta) {

                                    carta.ColocarCarta();
                                    carta.propietario.criaturaJugada = carta;
                                    /*carta.NuevoPadre(criaturasJugador.transform);
                                    carta.cartaEnMano = false;*/
                                }
                            }

                            if(carta.tipoCarta == Arrastrable.TipoCarta.CONJURO) {

                                bool jugarCarta = jugador.tierras.SuficienteMana(carta.GetCarta());

                                if(jugarCarta) {

                                    carta.NuevoPadre(pila.transform);
                                    carta.cartaEnMano = false;
                                }
                            }

                            if(carta.tipoCarta == Arrastrable.TipoCarta.ARTEFACTO || carta.tipoCarta == Arrastrable.TipoCarta.ENCANTEMIENTO) {

                                bool jugarCarta = jugador.tierras.SuficienteMana(carta.GetCarta());

                                if(jugarCarta) {

                                    carta.NuevoPadre(encantamientosJugador.transform);
                                    //carta.CambiarEscala(0.75f);
                                    carta.cartaEnMano = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        else if(SceneManager.GetActiveScene().name == "EditorBarajas") {

            EditorBaraja editorBarajas = GameObject.Find("Editor Baraja").GetComponent<EditorBaraja>();

            if(editorBarajas.panelSoltarCartas.activeInHierarchy) {

                Carta datosCarta = carta.GetComponent<MostrarDatosCarta>().carta; 
                editorBarajas.AnyadirCartaBaraja(carta);
            }
        }
    }

    public void AumentarColor(Arrastrable carta) {

        /*
        pila = GameObject.Find("Pila");
        manoJugador = GameObject.Find("Mano Jugador");
        tierrasJugador = GameObject.Find("Tierras Jugador");
        criaturasJugador = GameObject.Find("Criaturas Jugador");
        encantamientosJugador = GameObject.Find("Encantamientos Jugador");
        */

        if(carta.propietario == jugador && carta.cartaEnMano) {

            if(carta.tipoCarta == Arrastrable.TipoCarta.CRIATURA) {

                Image imagen = criaturasJugador.GetComponent<Image>();
                imagen.color = new Color(imagen.color.r, imagen.color.b, imagen.color.g, 0.7f);
            }

            if(carta.tipoCarta == Arrastrable.TipoCarta.TIERRA) {

                Image imagen = tierrasJugador.GetComponent<Image>();
                imagen.color = new Color(imagen.color.r, imagen.color.b, imagen.color.g, 0.7f);
            }

            if(carta.tipoCarta == Arrastrable.TipoCarta.ENCANTEMIENTO
            || carta.tipoCarta == Arrastrable.TipoCarta.ARTEFACTO) {

                Image imagen = encantamientosJugador.GetComponent<Image>();
                imagen.color = new Color(imagen.color.r, imagen.color.b, imagen.color.g, 0.7f);
            }

            if(carta.tipoCarta == Arrastrable.TipoCarta.CONJURO
            || carta.tipoCarta == Arrastrable.TipoCarta.INSTANTANEO) {

                Image imagen = pila.GetComponent<Image>();
                imagen.color = new Color(imagen.color.r, imagen.color.b, imagen.color.g, 0.7f);
            }
        }
    }

    public float DistanciaZonas(GameObject zona) {

        GameObject goCarta = carta.gameObject;
        float xCarta = goCarta.transform.position.x;
        float yCarta = goCarta.transform.position.y;
        float xZona = zona.transform.position.x;
        float yZona = zona.transform.position.y;

        float distancia = Mathf.Sqrt(Mathf.Pow(xCarta-xZona, 2) + Mathf.Pow(yCarta-yCarta, 2));
        float diagonal = Mathf.Sqrt(Mathf.Pow(xCarta-xZona, 2) + Mathf.Pow(yCarta-yCarta, 2)) * Mathf.Sqrt(2);

        Debug.Log("RESULTADO: " + (distancia / diagonal));

        return distancia / diagonal;
    }

    public void ApagarColor() {

        Image imagen = criaturasJugador.GetComponent<Image>();
        imagen.color = new Color(imagen.color.r, imagen.color.b, imagen.color.g, 0.0f);
        imagen = tierrasJugador.GetComponent<Image>();
        imagen.color = new Color(imagen.color.r, imagen.color.b, imagen.color.g, 0.0f);
        imagen = encantamientosJugador.GetComponent<Image>();
        imagen.color = new Color(imagen.color.r, imagen.color.b, imagen.color.g, 0.0f);
        imagen = pila.GetComponent<Image>();
        imagen.color = new Color(imagen.color.r, imagen.color.b, imagen.color.g, 0.0f);
    }

    public void Ordenar() {

        for(int i=0; i<transform.childCount; i++) {

            for(int j=0; j<transform.childCount-1-i; j++) {

                string a = transform.GetChild(j).GetComponent<MostrarDatosCarta>().carta.nombreCarta;
                string b = transform.GetChild(j+1).GetComponent<MostrarDatosCarta>().carta.nombreCarta;

                if(string.Compare(a,b) > 0) {

                    transform.GetChild(j).SetSiblingIndex(j+1);
                }
            }
        }
    }
}
