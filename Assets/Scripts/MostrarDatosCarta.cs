using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MostrarDatosCarta : MonoBehaviour {

    public Carta carta;
    public Arrastrable arrastrable;

    public TMPro.TMP_Text nombre;
    public TMPro.TMP_Text costeMana;
    public TMPro.TMP_Text palabrasClave;
    public TMPro.TMP_Text descripcion;
    public TMPro.TMP_Text flavour;
    public TMPro.TMP_Text tipos;
    public TMPro.TMP_Text fuerza;
    public TMPro.TMP_Text resistencia;

    TMPro.TextMeshProUGUI goFuerza;
    TMPro.TextMeshProUGUI goResistencia;

    void Start() {

        arrastrable = gameObject.GetComponent<Arrastrable>();
        
        nombre.text = carta.nombreCarta;
        costeMana.text = StringCosteMana();
        palabrasClave.text = carta.palabrasClave;
        descripcion.text = carta.descripcion;
        if(carta.flavour != null && carta.flavour != "") 
            flavour.text = carta.flavour;


        arrastrable.fuerzaTemp = carta.fuerza;
        arrastrable.resistenciaTemp = carta.resistencia;

        goFuerza = gameObject.transform.Find("Marco Carta").transform.Find("Caracteristicas")
        .transform.Find("Marco Estadisticas").transform.Find("Caja Estadisticas")
        .transform.Find("Fuerza").GetComponent<TMPro.TextMeshProUGUI>();

        goResistencia = gameObject.transform.Find("Marco Carta").transform.Find("Caracteristicas")
        .transform.Find("Marco Estadisticas").transform.Find("Caja Estadisticas")
        .transform.Find("Resistencia").GetComponent<TMPro.TextMeshProUGUI>();

        PintarCarta();
        EscribirTiposCarta();
        EscribirFuerzaResistencia();
    }

    string StringCosteMana() {

        //   in ro bl ve ne
        //int[0][0][0][0][0]

        if(carta.costeMana.Length < 5) {return null;}

        string resultado = "";

        if(carta.costeMana[0] > 0)
            resultado = carta.costeMana[0].ToString(); //Valor de mana incoloro

        if(carta.costeMana[1] > 0) { //Valor de mana rojo

            for(int i=0; i<carta.costeMana[1]; i++) {

                resultado += "R";
            }
        }

        if(carta.costeMana[2] > 0) { //Valor de mana blanco

            for(int i=0; i<carta.costeMana[2]; i++) {

                resultado += "B";
            }
        }

        if(carta.costeMana[3] > 0) { //Valor de mana verde

            for(int i=0; i<carta.costeMana[3]; i++) {

                resultado += "V";
            }
        }

        if(carta.costeMana[4] > 0) { //Valor de mana negro

            for(int i=0; i<carta.costeMana[4]; i++) {

                resultado += "N";
            }
        }

        return resultado;
    }

    void PintarCarta() {

        //   in ro bl ve ne
        //int[0][0][0][0][0]

        //Al igual que en Magic, si la carta es de un color se pinta entera de ese color
        //Si es de dos colores si pinta mitad de cada
        //Si es de tres o cuatro entonces se pinta de ocre

        Image color1 = gameObject.transform.Find("Marco Carta").transform.Find("Color 1").GetComponent<Image>();
        Image color2 = gameObject.transform.Find("Marco Carta").transform.Find("Color 2").GetComponent<Image>();

        Color32[] colores = new Color32[5];
        colores[0] = new Color32(255, 0, 0, 255);       //Rojo
        colores[1] = new Color32(255, 255, 255, 255);   //Blanco
        colores[2] = new Color32(0, 255, 0, 255);       //Verde
        colores[3] = new Color32(30, 30, 30, 255);      //Negro
        colores[4] = new Color32(180, 170, 30, 255);    //Multicolor/Ocre

        bool primerColor = false, segundoColor = false, tercerColor = false; 

        if(carta.tipoCarta.Contains("Tierra")) {

            if(carta.tiposConcretos.Contains("Señorío")) {

                color1.color = colores[0];
                color2.color = colores[0];
                primerColor = true;
            }

            if(carta.tiposConcretos.Contains("Parroquia")) {
                
                if(!primerColor) {

                    color1.color = colores[1];
                    color2.color = colores[1];
                }

                else {

                    color2.color = colores[1];
                    segundoColor = true;
                }
            }

            if(carta.tiposConcretos.Contains("Bosque")) {

                if(!primerColor) {

                    color1.color = colores[2];
                    color2.color = colores[2];
                }

                else if(!segundoColor) {

                    color2.color = colores[2];
                    segundoColor = true;
                }

                else { tercerColor = true; }
            }

            if(carta.tiposConcretos.Contains("Pantano")) {

                if(!primerColor) {

                    color1.color = colores[3];
                    color2.color = colores[3];
                }

                else if(!segundoColor) {

                    color2.color = colores[3];
                    segundoColor = true;
                }

                else { tercerColor = true; }
            }
        }

        else {

            //Para saber cuantos colores tiene la carta

            for(int i=1; i<carta.costeMana.Length; i++) {

                if(carta.costeMana[i] > 0) {

                    if(!primerColor) {

                        color1.color = colores[i-1];
                        color2.color = colores[i-1];
                        primerColor = true;
                    }

                    else if(!segundoColor) {

                        color2.color = colores[i-1];
                        segundoColor = true;
                    }

                    else if(!tercerColor) {

                        tercerColor = true;
                        break;
                    }
                }
            }
        }

        if(tercerColor) {

            color1.color = colores[4];
            color2.color = colores[4];
        }
    }

    void EscribirTiposCarta() {

        if(carta.tiposConcretos != null && carta.tiposConcretos != "")
            tipos.text = carta.tipoCarta + " - " + carta.tiposConcretos;
        else
            tipos.text = carta.tipoCarta;
    }

    public void EscribirFuerzaResistencia() {

        if(arrastrable && arrastrable.tipoCarta == Arrastrable.TipoCarta.CRIATURA) {

            //estadisticas.text = carta.fuerza.ToString() + "/" + carta.resistencia.ToString();
            fuerza.text = carta.fuerza.ToString();
            resistencia.text = carta.resistencia.ToString();
        }

        else {

            DesactivarFuerzaResistencia();
        }
    }

    void DesactivarFuerzaResistencia() {

        gameObject.transform.Find("Marco Carta").transform.Find("Caracteristicas")
        .transform.Find("Marco Estadisticas").transform.Find("Caja Estadisticas")
        .transform.Find("Fuerza").GetComponent<TMPro.TextMeshProUGUI>().enabled = false;

        gameObject.transform.Find("Marco Carta").transform.Find("Caracteristicas")
        .transform.Find("Marco Estadisticas").transform.Find("Caja Estadisticas")
        .transform.Find("Barra").GetComponent<TMPro.TextMeshProUGUI>().enabled = false;

        gameObject.transform.Find("Marco Carta").transform.Find("Caracteristicas")
        .transform.Find("Marco Estadisticas").transform.Find("Caja Estadisticas")
        .transform.Find("Resistencia").GetComponent<TMPro.TextMeshProUGUI>().enabled = false;

        gameObject.transform.Find("Marco Carta").transform.Find("Caracteristicas")
        .transform.Find("Marco Estadisticas").transform.Find("Caja Estadisticas")
        .GetComponent<Image>().enabled = false;

        gameObject.transform.Find("Marco Carta").transform.Find("Caracteristicas")
        .transform.Find("Marco Estadisticas").GetComponent<Image>().enabled = false;
    }

    public void ActualizarEstadisticas() {

        fuerza.text = arrastrable.fuerzaTemp.ToString();
        resistencia.text = arrastrable.resistenciaTemp.ToString();

        if(arrastrable.fuerzaTemp > carta.fuerza) {

            goFuerza.color = new Color32(30, 120, 60, 255); //Verde
        }

        else if (arrastrable.fuerzaTemp < carta.fuerza) {

            goFuerza.color = new Color32(120, 60, 30, 255); //Rojo
        }

        else {

            goFuerza.color = new Color32(0, 0, 0, 255);
        }


        if(arrastrable.resistenciaTemp > carta.resistencia) {

            goResistencia.color = new Color32(30, 120, 60, 255); //Verde
        }

        else if (arrastrable.resistenciaTemp < carta.resistencia) {

            goResistencia.color = new Color32(120, 60, 30, 255); //Rojo
        }

        else {

            goResistencia.color = new Color32(0, 0, 0, 255);
        }
    }
}
