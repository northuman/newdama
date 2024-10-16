using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/*
* Descripcion: clase para efecto hover sobre las cartas.
* OnPointerEnter : cuando el ratón está sobre el objeto éste se guarda. Tambien se guarda la posicion y la escala.
*   todo: calcular si el objeto está fuera de la pantalla y reposicionarlo. 
* OnPointerExit : cuando el ratón sale del objeto se reestablecen los valores de posicion y escala iniciales.
*/
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
