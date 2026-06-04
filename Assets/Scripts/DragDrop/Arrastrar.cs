using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/*
* Script para arrastrar las cartas con el ratón. 
* eventData : es la variable que guarda la información del evento, en este caso la carta.
* OnBeginDrag : se guarda el objeto parent actual para que si el destino no es válido vuelva a su origen
*               se desactivan los raycast del canvas para que no interfieran con el arrastrado
* OnDrag : va cambiando la posición de la carta
* OnEndDrag : se cambia el parent de la carta. Esto se hace a la vez que DropZone comprueba si el destino es válido.
*             Si es válido se habrá guardado el nuevo parent en parentToReturnTo, si no será el original y la carta volverá a su sitio.
*             Se vuelven a activar los raycast.
*/

public class Arrastrar : MonoBehaviour, IBeginDragHandler,IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Transform parentToReturnTo = null;
    private Vector3 escalaOriginal;
    [Range(0.5f, 10f)]
    private float escalaOnDrag =1.5f;
    private float velocidadZoom = 0.1f;
    private Coroutine zoomCoroutine;
  
    /*
    Primero se comprueba si la carta pertenece al jugador.
    Se guarda la escala actual de la carta, se guarda el parent actual de la carta.
    Se cambia el parent al superior.
    Si no cumple la primera condicion se desactiva el script
    */
    public void OnBeginDrag(PointerEventData eventData)
    {   
        if(eventData.button == 0)
        {
            int perteneceA = eventData.pointerDrag.GetComponent<CartasJugadas>().perteneceAJugador;
            if(perteneceA == 1)
            {
                escalaOriginal = transform.localScale;    
                parentToReturnTo = transform.parent;
                transform.SetParent(transform.parent.parent);
                GetComponent<CanvasGroup>().blocksRaycasts = false;

                //zoom a la carta
                if(zoomCoroutine != null)StopCoroutine(zoomCoroutine);
                zoomCoroutine = StartCoroutine(EscalarCarta(escalaOriginal * escalaOnDrag));
            }
            else
            {
                enabled = false;
            }
        }
    }
    /*
    Se va cambiando la posicion de la carta conforme se arrastra.
    */
    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.button == 0)
        {
            transform.position = eventData.position;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(eventData.button == 0)
        {
            transform.SetParent(parentToReturnTo);
            GetComponent<CanvasGroup>().blocksRaycasts = true;

            // reiniciar escala
            if (zoomCoroutine != null)StopCoroutine(zoomCoroutine);
            zoomCoroutine = StartCoroutine(EscalarCarta(escalaOriginal));
        }
    }

    //corrutina para escalar la carta cuando la arrastras
    private IEnumerator EscalarCarta(Vector3 escala)
    {
        Vector3 inicio = transform.localScale;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / velocidadZoom;
            transform.localScale = Vector3.Lerp(inicio, escala, t);
            yield return null;
        }
    }

public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == 0) // Clic izquierdo
        {
            Partida gestor = FindObjectOfType<Partida>();
            if(gestor == null) return;

            // Obtenemos los componentes de la carta para usarlos en ambas lógicas
            CartasJugadas cj = GetComponent<CartasJugadas>();
            MostrarCarta mc = GetComponent<MostrarCarta>();

            if (cj == null || mc == null) return;

            // --------------------------------------------------------
            // 0. LÓGICA DE CONJUROS (Modo Francotirador y Activación)
            // --------------------------------------------------------
            
            // CASO A: Tienes el Modo Francotirador activado y buscas una víctima
            if (gestor.esperandoObjetivo == true)
            {
                // Verificamos si haces clic en un enemigo (jugador 2) que sea Criatura
                if (mc.tipo == "Criatura" && cj.perteneceAJugador == 2)
                {
                    gestor.LanzarConjuroA(this.gameObject);
                }
                else
                {
                    Debug.LogWarning("Objetivo inválido. Debes hacer clic en una criatura enemiga.");
                }
                // ¡VITAL! Cortamos aquí. Si estamos en modo francotirador, el clic no debe hacer NADA más.
                return; 
            }

            // CASO B: Activar un Conjuro propio durante tu Fase Principal
            if (gestor.faseActual == Partida.Fases.PRINCIPAL_1 || gestor.faseActual == Partida.Fases.PRINCIPAL_2)
            {
                if (mc.tipo == "Conjuro" && cj.perteneceAJugador == 1)
                {
                    gestor.PrepararConjuro(this.gameObject);
                    return; // Cortamos aquí para que el juego se quede esperando el siguiente clic
                }
            }

            // --------------------------------------------------------
            // 1. LÓGICA DE DESCARTE (Si estamos en la Fase Final)
            // --------------------------------------------------------
            if (gestor.esperandoDescarte == true && gestor.faseActual == Partida.Fases.FINAL)
            {
                int perteneceA = cj.perteneceAJugador;
                
                // Comprobamos que la carta sea nuestra
                if (perteneceA == 1) 
                {
                    // Borramos la carta de la memoria (la lista lógica)
                    var datosCarta = mc.GetCarta();
                    gestor.jugador.mano.Remove(datosCarta);

                    // Destruimos la carta visual de la pantalla
                    Destroy(gameObject);

                    // Restamos al contador
                    gestor.cartasParaDescartar--;
                    Debug.Log("Has descartado una carta. Te faltan por tirar: " + gestor.cartasParaDescartar);

                    // Si ya hemos cumplido el cupo... ¡Pasamos turno automáticamente!
                    if (gestor.cartasParaDescartar <= 0)
                    {
                        gestor.esperandoDescarte = false;
                        Debug.Log("Descarte completado. Pasando el turno al rival...");
                        gestor.AvanzarFase();
                    }
                }
                
                // IMPORTANTE: Salimos de la función aquí. Si la carta se destruyó,
                // no queremos que intente ejecutar la lógica de ataque de abajo.
                return; 
            }

            // --------------------------------------------------------
            // 2. LÓGICA DE ATAQUE (Si estamos en la Fase de Combate)
            // --------------------------------------------------------
            if (gestor.faseActual == Partida.Fases.COMBATE)
            {
                // Solo podemos atacar con nuestras propias cartas y si son criaturas
                if (cj.perteneceAJugador == 1 && mc.tipo == "Criatura")
                {
                    // Regla de Magic: Una carta girada no puede atacar
                    if (cj.girada == true)
                    {
                        Debug.LogWarning("Esta criatura ya está girada (exhausta). No puede atacar.");
                    }
                    else
                    {
                        // La giramos visualmente para indicar que está atacando
                        cj.RotarCarta();
                        
                        // La metemos en la lista de la guerra de Partida.cs
                        gestor.criaturasAtacantes.Add(this.gameObject);
                        
                        Debug.Log($"¡{mc.GetCarta().nombreCarta} ha sido declarada como atacante!");
                    }
                }
            }

            // --------------------------------------------------------
            // 3. LÓGICA DE BLOQUEO (Si estamos en Fase de Bloqueo y NOS ATACAN)
            // --------------------------------------------------------
            if (gestor.faseActual == Partida.Fases.BLOQUEO && gestor.turno == true)
            {
                // CASO A: Hacemos clic en NUESTRA propia criatura para que defienda
                if (cj.perteneceAJugador == 1 && mc.tipo == "Criatura")
                {
                    if (cj.girada == true)
                    {
                        Debug.LogWarning("No puedes bloquear con una criatura girada (exhausta).");
                    }
                    else
                    {
                        // La guardamos en el Gestor
                        gestor.bloqueadorSeleccionado = this.gameObject;
                        Debug.Log($"Has seleccionado a {mc.GetCarta().nombreCarta} para defender. Ahora haz clic en el atacante enemigo.");
                    }
                }
                
                // CASO B: Hacemos clic en un ENEMIGO para lanzarle a nuestro defensor
                else if (cj.perteneceAJugador == 2 && gestor.bloqueadorSeleccionado != null)
                {
                    // Comprobamos si este enemigo realmente nos está atacando
                    if (gestor.criaturasAtacantes.Contains(this.gameObject))
                    {
                        // ¡Los emparejamos en el Diccionario!
                        gestor.emparejamientos[this.gameObject] = gestor.bloqueadorSeleccionado;
                        
                        string nomDefensor = gestor.bloqueadorSeleccionado.GetComponent<MostrarCarta>().GetCarta().nombreCarta;
                        Debug.Log($"¡ORDEN DADA! {nomDefensor} defenderá el ataque de {mc.GetCarta().nombreCarta}.");
                        
                        // Vaciamos el cursor para poder elegir otro bloqueador si queremos
                        gestor.bloqueadorSeleccionado = null; 
                    }
                    else
                    {
                        Debug.Log("Esa criatura enemiga no te está atacando, elige a otra.");
                    }
                }
            }
        }
    }
}
