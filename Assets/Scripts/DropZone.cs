using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
/*
* Este script tiene la función de cambiar el parent de la carta para que al soltarla se 
* coloque en el panel
* Adicionalmente se comprueba si está permitido colocarla
*/
    public enum tipoDropZone {MANO, TIERRAS, BATALLA}
    /*MANO : admite todos los tipos de carta
    /TIERRAS : admite solo tierras
    / BATALLA : admite crriaturas e instantaneos
    */

    public tipoDropZone tipoZona;

    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " dropea en " + gameObject.name);

        Arrastrar arrastrando = eventData.pointerDrag.GetComponent<Arrastrar>();
        if (arrastrando != null)
        {
            string tipo = eventData.pointerDrag.GetComponent<MostrarCarta>().tipo; 
            //se guarda el tipo de la carta que se está arrastrando

            if(validarTipo(tipo))
            {
            //si el tipo es el correcto se cambia el parent 
            arrastrando.parentToReturnTo = this.transform;
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public bool validarTipo(string tipo) //funcion que comprueba si la carta se puede colocar en el panel
    {
        bool validar = false;
        switch(tipoZona)
        {
            case tipoDropZone.MANO: 
                validar = true;
                break;
            case tipoDropZone.TIERRAS:
                if(tipo.Equals("Tierra"))
                {
                    validar = true;
                }
                break;
            case tipoDropZone.BATALLA:
                if(tipo.Equals("Criatura") || tipo.Equals("Instantáneo"))
                {
                    validar = true;
                }
                break;
        }
        return validar;
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
