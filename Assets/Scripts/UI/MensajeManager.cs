using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

/*
* Controla los mensajes que se muestran por pantalla.
* Pensado para usarse desde otros puntos del código.
* mensajeText es el objeto del editor.
* mensaje es el string con el mensaje que se quiere mostrar.
*/

public class MensajeManager : MonoBehaviour
{
    public TextMeshProUGUI mensajeText;
    public float duracion = 0.5f;

    private Coroutine rutinaMensajeActual;

    private void Start()
    {
        mensajeText.text = "";
        mensajeText.enabled = false;

    }

    public void MostrarMensaje(string mensaje)
    {
        if (rutinaMensajeActual != null)
        {
            StopCoroutine(rutinaMensajeActual);
        }
        rutinaMensajeActual = StartCoroutine(MostrarMensajeCoroutine(mensaje));
    }

    private IEnumerator MostrarMensajeCoroutine(string mensaje)
    {
        mensajeText.text = mensaje;
        mensajeText.enabled = true;
        yield return new WaitForSeconds(duracion);
        mensajeText.enabled = false;
        rutinaMensajeActual = null;
    }
}
