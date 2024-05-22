using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Arrastrar : MonoBehaviour, IBeginDragHandler,IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;


    public void OnBeginDrag(PointerEventData eventData)
    {   
        GameObject j1 = GameObject.Find("Jugador");
        int idCarta = eventData.pointerDrag.gameObject.GetComponent<MostrarCarta>().id;
        
        parentToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        Debug.Log("Arrastrando" + this.gameObject.name);
        //if(j1.GetComponent<Jugador>().jugarCarta(buscarCartaenMano(idCarta, j1))){
        //}
    }

    public int buscarCartaenMano(int idCarta, GameObject jugador){
        for(int i = 0; i < jugador.GetComponent<Jugador>().mano.Count; i++){
            if(idCarta == jugador.GetComponent<Jugador>().mano[i].id){
                return i;
            }
        }
        return -1;
    }
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
        //Debug.Log("Arrastrando");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        //Debug.Log("Termino de arrastrar");
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
