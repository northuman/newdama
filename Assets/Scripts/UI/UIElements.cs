using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
* Contiene los elementos de interfaz y las funciones para mostrarlos.
*/

public class UIElements : MonoBehaviour
{
    public GameObject jugadorGM;
    Jugador jugador1;
    public int vida;
    public int[] manaJugador;
    public int manaB;
    public int manaN;
    public int manaR;
    public int manaV;

    public GameObject panelMana;
    public TMP_Text vida1;
    public TMP_Text manaBlanco;
    public TMP_Text manaNegro;
    public TMP_Text manaRojo;
    public TMP_Text manaVerde;

    public GameObject panelTexto;
    public TMP_Text cuadroTexto;

    public void EnsenyarTexto(GameObject carta, bool click){
        if(carta.GetComponent<Reverso>().enabled == false){
            if(click){
                cuadroTexto.text = carta.GetComponent<MostrarCarta>().flavour;

                //Fuerza recalcular tamaño del texto
                LayoutRebuilder.ForceRebuildLayoutImmediate(cuadroTexto.rectTransform);

                //Calcula altura preferida
                float textoAlto = cuadroTexto.preferredHeight;
                //float padding = -1f; // Ajusta según tu diseño

                //Aumenta el alto del panel si el texto lo necesita
                float altoDeseado = textoAlto;
                float altoMinimo = 200f; // Tu altura deseada por defecto

                RectTransform panelRect = panelTexto.GetComponent<RectTransform>();
                panelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(altoMinimo, altoDeseado));
                //muestro el panel
                TogglePanelTexto(true);
            }
            else{
                //oculto el panel
                TogglePanelTexto(false);
            }
        }  
    }

    public void TogglePanelTexto(bool mostrar){
        var canvasGroup = panelTexto.GetComponent<CanvasGroup>();
        if(mostrar){
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }else{
            //Oculto el panel
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
    

    /*
    Separa el array de maná en valores individuales para que se pueda mostrar en la interfaz
    */
    public void SepararMana()
    { 
        if (jugador1.mana != null){
            
            manaB = jugador1.mana[0];
            manaN = jugador1.mana[1];
            manaR = jugador1.mana[2];
            manaV = jugador1.mana[3];
        }
    }

    /*
    Este método hace que se muestre el maná de forma dinámica. 
    Cuando es 0 no se muestra la interfaz
    */
    public void ShowMana()
    {
        if(manaB == 0){
            panelMana.transform.Find("Blanco").gameObject.SetActive(false);
        }else{
            panelMana.transform.Find("Blanco").gameObject.SetActive(true);
        }
        if(manaN == 0){
            panelMana.transform.Find("Negro").gameObject.SetActive(false);
        }else{
            panelMana.transform.Find("Negro").gameObject.SetActive(true);
        }
        if(manaR == 0){
            panelMana.transform.Find("Rojo").gameObject.SetActive(false);
        }else{
            panelMana.transform.Find("Rojo").gameObject.SetActive(true);
        }
        if(manaV == 0){
            panelMana.transform.Find("Verde").gameObject.SetActive(false);
        }else{
            panelMana.transform.Find("Verde").gameObject.SetActive(true);
        }
        
        //Asigna los valores a las casillas de la interfaz
        manaBlanco.text = "" + manaB;
        manaNegro.text = "" + manaN;
        manaRojo.text = "" + manaR;
        manaVerde.text = "" + manaV;
    }
    
    void Start()
    {
        jugadorGM = GameObject.Find("Jugador");
        jugador1 = jugadorGM.GetComponent<Jugador>();
        TogglePanelTexto(false);
    }

    
    void Update()
    {
        //Los elementos de la interfaz se actualizan cada frame
        vida = jugador1.vida;
        vida1.text = "" + vida;
        SepararMana();
        ShowMana();
        
    }
}
