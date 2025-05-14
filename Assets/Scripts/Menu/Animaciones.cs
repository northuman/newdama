using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Animaciones : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject textoInformativo;
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        textoInformativo.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        textoInformativo.SetActive(false);
    }
}
