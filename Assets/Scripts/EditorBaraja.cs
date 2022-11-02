using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorBaraja : MonoBehaviour {

    public GameObject panelBarajas;
    public GameObject panelSoltarCartas;
    public GameObject botonNuevaBaraja;
    public GameObject botonListo;
    public GameObject botonVolver;
    public GameObject prefabImagenBaraja;
    public GameObject prefabImagenCarta;

    public GameObject goBaraja;
    public GameObject goBarajas;

    public ListaBarajas listaBarajas;
    public bool creandoBaraja = false;
    public int cantidadBarajas = 0;

    void Start() {

        botonListo.SetActive(false);
        panelSoltarCartas.SetActive(false);
    }

    public void VolverMenu() {

        SceneManager.LoadScene("Menu");
    }

    void ActivarBotonesPrincipales() {

        botonListo.SetActive(false);
        panelSoltarCartas.SetActive(false);
        panelBarajas.SetActive(true);
        botonVolver.SetActive(true);
        botonNuevaBaraja.SetActive(true);
    }

    void ActivarBotonesCrandoBaraja() {

        botonListo.SetActive(true);
        panelSoltarCartas.SetActive(true);
        panelBarajas.SetActive(false);
        botonVolver.SetActive(false);
        botonNuevaBaraja.SetActive(false);
    }

    public void CrearBaraja() {

        creandoBaraja = true;

        ActivarBotonesCrandoBaraja();

        goBaraja = new GameObject("Baraja" + cantidadBarajas);
        goBaraja.AddComponent<Baraja>();
    }

    public void EditarBaraja(int id) {

        ActivarBotonesCrandoBaraja();

        goBaraja = goBarajas.transform.GetChild(id).gameObject;

        CrearImagenesBaraja();
    }

    public void AnyadirCartaBaraja(Arrastrable arrastrable) {

        Carta carta = arrastrable.carta;

        Baraja baraja = goBaraja.GetComponent<Baraja>();
        bool contieneCarta = baraja.ContieneCarta(carta);

        if(!contieneCarta) { 

            baraja.AnyadirCarta(carta);
            CrearImagenCarta(carta);
        }

        else if(arrastrable.tipoCarta == Arrastrable.TipoCarta.TIERRA) {

            baraja.AnyadirCarta(carta);
            AumentarNumeroImagen(carta);
        }

        else if(baraja.NumeroCopias(carta) < 4) {

            baraja.AnyadirCarta(carta);
            AumentarNumeroImagen(carta);
        }
    }

    public void CrearImagenCarta(Carta carta) {

        GameObject imagenCarta = Instantiate(prefabImagenCarta, new Vector3(0, 0, 0), Quaternion.identity);

        imagenCarta.gameObject.name = carta.nombreCarta;

        string texto = carta.nombreCarta;

        imagenCarta.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().SetText(texto);

        imagenCarta.transform.SetParent(panelSoltarCartas.transform);
    }

    public void CrearImagenesBaraja() {

        Baraja baraja = goBaraja.GetComponent<Baraja>();

        bool crear = true;

        for(int i=0; i<baraja.cartas.Count; i++) {

            for(int j=0; j<panelSoltarCartas.transform.childCount; j++) {

                if(baraja.cartas[i].nombreCarta == panelSoltarCartas.transform.GetChild(j).name) {

                    crear = false;
                    break;
                }
            }

            if(crear) {

                CrearImagenCarta(baraja.cartas[i]);
            }

            else {

                AumentarNumeroImagen(baraja.cartas[i]);
            }
        }
    }

    public void AumentarNumeroImagen(Carta carta) {

        GameObject imagenCarta = GameObject.Find(carta.nombreCarta);

        TMPro.TMP_Text tmpCantidad = imagenCarta.transform.GetChild(2).GetComponent<TMPro.TMP_Text>();
        int cantidad = int.Parse(tmpCantidad.text);

        tmpCantidad.SetText((++cantidad).ToString());
    }

    public void LimpiarPanelSoltar() {

        foreach(Transform child in panelSoltarCartas.transform) {

            GameObject.Destroy(child.gameObject);
        }
    }

    public void BarajaLista() {

        if(goBaraja && goBaraja.GetComponent<Baraja>().cartas.Count > 0) {

            listaBarajas.barajas.Add(goBaraja.GetComponent<Baraja>());

            ActivarBotonesPrincipales();

            LimpiarPanelSoltar();

            if(creandoBaraja) { AnyadirImagenBaraja(); }

            creandoBaraja = false;
        }
    }

    public void AnyadirImagenBaraja() {

        GameObject imagenBaraja = Instantiate(prefabImagenBaraja, new Vector3(0, 0, 0), Quaternion.identity);

        imagenBaraja.GetComponent<ModificarBaraja>().baraja = goBaraja.GetComponent<Baraja>();

        imagenBaraja.GetComponent<ModificarBaraja>().id = cantidadBarajas;

        string texto = "Baraja " + ++cantidadBarajas;

        imagenBaraja.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().SetText(texto);

        imagenBaraja.transform.SetParent(panelBarajas.transform);

        goBaraja.transform.SetParent(goBarajas.transform);
    }
}
