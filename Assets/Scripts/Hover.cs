using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hover : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    GameObject carta = null;
    Vector3 escalaOG = new Vector3(1, 1, 1);
    Vector3 positionOG;
    public void OnPointerEnter(PointerEventData eventData){
        carta = this.gameObject;
        escalaOG = carta.transform.localScale;
        positionOG = carta.transform.position;
    }

    public void OnPointerExit(PointerEventData eventData){
        carta = this.gameObject;
    }
}
