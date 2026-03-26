using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UtilCartas;
using UnityEngine.UI;
using TMPro;

/*
* Controla el flujo de la partida, aquí va el sistema de turnos.
* 
*/

public class Partida : MonoBehaviour
{
    public enum OrdenJugadores { JUGADOR, OPONENTE };
    public enum Fases { INICIO, PRINCIPAL_1, COMBATE, PRINCIPAL_2, FINAL };
    public Jugador jugador;
    public Jugador oponente;
    public bool tierrasJugadasEsteTurno = false;
    List<Jugador> jugadores;
    bool turno = false; //false = turno jugador; true = turno oponente
    public Jugador jugadorActivo => turno ? oponente : jugador;
    int ganador = 0;
    public Fases faseActual; //veo en que momento de partida estamos.

    // Zonas de juego del oponente para que la IA sepa dónde poner sus cartas
    public Transform zonaTierrasOponente;
    public Transform zonaBatallaOponente;
    //para la fase final y descartar cartas (maximo 7 en mano)
    public bool esperandoDescarte = false;
    public int cartasParaDescartar = 0;
    public List <GameObject> criaturasAtacantes = new List<GameObject>();
    public TextMeshProUGUI textoVidaOponente;
    public TextMeshProUGUI textoVidaJugador;


    void Start()
    {
        AccionesPrevias();
        //iniciar turno
        IniciarTurno();
    }


    public void GenerarPrioridadJugador()
    {
        turno = Convert.ToBoolean(NumAleatorio(0, 1));
        turno = false; // fuerza el turno del jugador para probar

        jugadores = new List<Jugador> { jugador, oponente };
    }

    public void AccionesPrevias()
    {
        GenerarPrioridadJugador();

        for (int i = 0; i < 2; i++)
        {
            jugadores[i].RellenarBaraja();
            jugadores[i].CrearBarajaPartida();
            jugadores[i].RobarCarta(7);
        }

        //FORZAR CARTA DEL OPONENTE PARA PROBAR ---------------------
        ForzarCartaOponente();
    }

    public void ForzarCartaOponente()
    {
        if (oponente.mano.Count > 0)
        {
            var cartaPrueba = jugadores[1].mano[0];
            oponente.mano.RemoveAt(0);
            Debug.Log("Carta de prueba añadida al campo del oponente: " + cartaPrueba.nombreCarta);
        }
    }

    //SISTEMA DE TURNOS Y FASES (MAQUINA DE ESTADOS)

    public void IniciarTurno()
    {
        Debug.Log("--- Comienza el turno de: " + (turno ? "Oponente" : "jugador") + " ---");
        faseActual = Fases.INICIO;
        FaseInicio();

        if (turno)
        {
            StartCoroutine(CerebroIA()); // Inicia el turno de la IA
        }
    }

    public void AvanzarFase()
    {
        if (ganador != 0) return; //si el juego acabó no se hace nada

        if(esperandoDescarte == true)
        {
            Debug.Log("¡Espera! Aún tienes que descartar cartas antes de avanzar de fase.");
            return; // No avanzamos de fase hasta que el jugador descarte las cartas necesarias
        }

        switch (faseActual)
        {
            case Fases.INICIO:
                faseActual = Fases.PRINCIPAL_1;
                FasePrincipal(1);
                break;

            case Fases.PRINCIPAL_1:
                faseActual = Fases.COMBATE;
                FaseCombate();
                break;

            case Fases.COMBATE:
                if(turno == false)
                {
                    ResolverCombate(); // Resolvemos el combate antes de pasar a la siguiente fase
                }
                faseActual = Fases.PRINCIPAL_2;
                FasePrincipal(2);
                break;

            case Fases.PRINCIPAL_2:
                Debug.Log(">>> Intentando entrar a fase final...");
                faseActual = Fases.FINAL;
                FaseFinal();
                break;

            case Fases.FINAL:
            Debug.Log(">>> Termina el turno. Cambiando jugador activo...");
                turno = !turno; //cambia el jugador activo
                //faseActual = Fases.FINAL;
                IniciarTurno();
                break;
        }
    }

    // LOGICA DE CADA FASE
    public void FaseInicio()
    {
        Debug.Log("Fase de inicio (enderezco, mantenimiento, robo");

        tierrasJugadasEsteTurno = false; //reiniciamos el contador de tierras jugadas al inicio del turno.

        //1. Enderezar cartas giradas
        EnderezarCartasMesa();

        //2. Mantenimiento (Upkeep)

        //3. Robar (Draw)
        bool falloRobar = jugadorActivo.RobarCarta(1);
        
        if (!falloRobar)
        {
            ganador = turno ? 1 : 2; //si no se puede robar, pierte.
            Debug.Log("Jugador " + ganador + " ha ganado por deckeo");
        }

        // Cuando acabe la animación de robar, el jugador debería poder darle al botón 
        // de "Avanzar Fase" para pasar a la Fase Principal 1.
    }

    private void EnderezarCartasMesa()
    {
        // El radar: busca todas las cartas que hay en la escena
        CartasJugadas[] todasLasCartas = FindObjectsOfType<CartasJugadas>();

        foreach (CartasJugadas carta in todasLasCartas)
        {
            // Si la carta es del jugador activo Y está girada...
            if (carta.perteneceAJugador == jugadorActivo.id && carta.girada == true)
            {
                carta.RotarCarta(); // Esto la devuelve a su posición vertical
            }
        }
        Debug.Log("Mesa enderezada para el jugador: " + jugadorActivo.id);
    }

    public void FasePrincipal(int numeroFase)
    {
        Debug.Log($"{numeroFase} Fase principal {numeroFase} (jugar tierras, criaturas, conjuros)");
        // Aquí el juego se detiene y espera a que el jugador arrastre cartas a la mesa.
        // Solo avanzará cuando pulse el botón de "AvanzarFase()".

    }

    public void FaseCombate()
    {
        Debug.Log("--- Fase de combate ---");

        //vacio la lista por si había atacantes del turno anterior
        criaturasAtacantes.Clear();

        if (turno == false)
        {
            Debug.Log("Es el turno del jugador. Esperando a que declare atacantes...");
            // Aquí el juego se detiene y espera a que el jugador arrastre sus criaturas a la zona de ataque.
        }
        else
        {
            Debug.Log("Es el turno de la IA. Por ahora no ataca (aún no sé cómo).");
            AvanzarFase();
        }
        //Activar habilidades principio combate

        //Declaracion atacantes jugador0->jugador1

        //Instantaneos Primero defensor

        //Declaracion bloqueadores jugador1

        //Instantaneos Primero defensor

        //Asignar danyo

        //Instantaneos Primero defensor

        //Resolver efectos fin de combate e instantaneos

        Debug.Log(" FASE DE COMBATE (Declarar Atacantes, Bloqueadoras, Daño)");
        // El combate es un mini-bucle complejo, pero la base está aquí.

    }

    private void ResolverCombate()
    {
        // Si nadie ha atacado, no hacemos nada
        if (criaturasAtacantes.Count == 0) 
        {
            Debug.Log("Combate resuelto: No hubo atacantes.");
            return;
        }

        int danoTotal = 0;

        // Sumamos el daño de cada criatura en la lista
        foreach (GameObject atacante in criaturasAtacantes)
        {
            if (atacante != null)
            {
                Carta datosCarta = atacante.GetComponent<MostrarCarta>().GetCarta();
                
                // OJO: Aquí asumo que la variable de ataque en tu clase Carta se llama "ataque". 
                // Si se llama "fuerza" o "daño", cámbialo en la línea de abajo:
                danoTotal += datosCarta.fuerza; 
                
                Debug.Log($"- {datosCarta.nombreCarta} ataca con {datosCarta.fuerza} de poder.");
            }
        }

        Debug.Log($"¡BOOM! El oponente recibe un total de {danoTotal} puntos de daño.");
        
        // Restamos la vida al rival
        oponente.vida -= danoTotal; 
        
        Debug.Log($"Al oponente le quedan {oponente.vida} puntos de vida.");

        if(textoVidaOponente != null)
        {
            textoVidaOponente.text = oponente.vida.ToString();
        }
        else
        {
            Debug.LogWarning("Texto de vida del oponente no asignado en el inspector.");
        }

        // Vaciamos la lista de atacantes para el próximo turno
        criaturasAtacantes.Clear();
    }


public void FaseFinal()
    {
        Jugador jugadorActivo = turno ? oponente : jugador;
        Debug.Log(" FASE FINAL (Paso final y limpieza)");
        
        int excesoCartas = jugadorActivo.mano.Count - 7;
        
        if (excesoCartas > 0)
        {
            Debug.Log($"El jugador tiene {jugadorActivo.mano.Count} cartas en mano, debe descartar {excesoCartas} cartas.");
            cartasParaDescartar = excesoCartas;
            esperandoDescarte = true;

            // SI es la IA, le digo que descarte sola. Si es el jugador, esperamos
            if(turno == true)
            {
                StartCoroutine(DescarteAutomaticoIA());
            }
            else
            {
                Debug.Log("Esperando a que el jugador descarte cartas manualmente...");
                // ¡AQUÍ NOS DETENEMOS! El código termina aquí y no llama a AvanzarFase()
                // hasta que tú descartes las cartas con el ratón.
            }
        }
        else
        {
            Debug.Log("No es necesario descartar cartas. Avanzando al siguiente turno...");
            AvanzarFase();
        }
    }

private IEnumerator DescarteAutomaticoIA()
    {
        Debug.Log("IA: Vaya, tengo demasiadas cartas. Pensando cuáles tirar...");
        yield return new WaitForSeconds(1.5f); 
        
        // Mientras la IA tenga que descartar y tenga datos en su lista...
        while (cartasParaDescartar > 0 && oponente.mano.Count > 0)
        {
            // 1. Borramos el dato del "cerebro" del bot (la lista lógica)
            int indiceUltimaCarta = oponente.mano.Count - 1;
            oponente.mano.RemoveAt(indiceUltimaCarta);
            
            // 2. Borramos el dibujo de la pantalla (el GameObject)
            // Aseguramos que el panel de la mano (con mayúscula) tenga hijos físicos
            if (oponente.Mano.transform.childCount > 0) 
            {
                // Cogemos la última carta física que cuelga del panel
                int ultimoHijo = oponente.Mano.transform.childCount - 1;
                Transform cartaFisica = oponente.Mano.transform.GetChild(ultimoHijo);
                
                // ¡La destruimos!
                Destroy(cartaFisica.gameObject); 
            }
            
            cartasParaDescartar--;
            Debug.Log($"IA: He descartado una carta. Me quedan por tirar: {cartasParaDescartar}");
            
            yield return new WaitForSeconds(0.5f); 
        }

        // Una vez terminamos de descartar, quitamos el seguro y pasamos el turno
        esperandoDescarte = false;
        Debug.Log("IA: He terminado de descartar. Cambio de turno.");
        AvanzarFase(); 
    }

    private IEnumerator CerebroIA()
    {
        Debug.Log("IA: Pensando mi turno...");
        yield return new WaitForSeconds(1.5f); 
        
        // Pasa a Principal 1
        AvanzarFase(); 
        
        // 1. LA IA INTENTA JUGAR UNA TIERRA
        JugarTierraIA();
        yield return new WaitForSeconds(1.0f);

        // 2. La IA extrae el maná de las tierras que ya tiene en mesa (si es que tiene)
        ExtraerManaIA();
        yield return new WaitForSeconds(1.0f);

        // 3. LA IA INTENTA INVOCAR CRIATURAS
        JugarCriaturasIA();
        yield return new WaitForSeconds(1.5f);

        // Pasa a Combate
        AvanzarFase();
        Debug.Log("IA: No ataco esta vez (aún no sé cómo).");
        yield return new WaitForSeconds(1.5f);

        // Pasa a Principal 2
        AvanzarFase();
        yield return new WaitForSeconds(1.0f);

        // Termina su turno
        Debug.Log("IA: Termino mi turno, te toca.");
        AvanzarFase(); 
    }

    // --- MANOS VIRTUALES DE LA IA ---

    private void JugarTierraIA()
    {
        if (oponente.Mano == null) return;

        // Buscamos entre los GameObjects físicos que cuelgan de la mano del oponente
        foreach (Transform cartaTransform in oponente.Mano.transform)
        {
            MostrarCarta mc = cartaTransform.GetComponent<MostrarCarta>();
            if (mc != null && mc.tipo == "Tierra" && !tierrasJugadasEsteTurno)
            {
                // Mueve la carta a la mesa
                cartaTransform.SetParent(zonaTierrasOponente);
                cartaTransform.localPosition = Vector3.zero; // Asegura que la carta se posicione correctamente en la zona
                cartaTransform.localRotation = Quaternion.Euler(0, 0, 0); // Asegura que la carta mire hacia arriba
                
                oponente.mano.Remove(mc.GetCarta()); // Elimina la carta de la mano lógica del oponente

                Transform reverso = cartaTransform.Find("Reverso");
                if(reverso != null)
                {
                    reverso.gameObject.SetActive(false); // Desactiva el reverso para mostrar la cara de la carta
                }
                // La inicializa (como hace el DropZone)
                CartasJugadas cj = cartaTransform.GetComponent<CartasJugadas>();
                if (cj != null) cj.Inicializar(mc.GetCarta());
                
                tierrasJugadasEsteTurno = true;
                Debug.Log("IA: ¡He jugado una Tierra!");
                break; // Solo bajamos una tierra por turno
            }
        }
    }

private void JugarCriaturasIA()
    {
        if (oponente.Mano == null) 
        {
            Debug.LogWarning("IA: Oye, no tengo enlazado el objeto 'Mano' en el inspector.");
            return;
        }

        Debug.Log($"IA: Revisando mi mano... Tengo {oponente.Mano.transform.childCount} cartas físicas.");

        // Hacemos un bucle inverso
        for (int i = oponente.Mano.transform.childCount - 1; i >= 0; i--)
        {
            Transform cartaTransform = oponente.Mano.transform.GetChild(i);
            MostrarCarta mc = cartaTransform.GetComponent<MostrarCarta>();
            
            if (mc != null && mc.GetCarta() != null)
            {
                Carta datosCarta = mc.GetCarta();
                Debug.Log($"IA: Mirando la carta '{datosCarta.nombreCarta}' (Tipo: {mc.tipo})");

                if (mc.tipo == "Criatura")
                {
                    Debug.Log($"IA: ¡Es una criatura! Quiero invocar a {datosCarta.nombreCarta}. Voy a ver si tengo maná...");
                    
                    // Comprobamos si la IA tiene maná suficiente
                    if (oponente.ComprobarMana(cartaTransform.gameObject))
                    {
                        // Si tiene maná, la invocamos a la zona de batalla
                        oponente.RestarMana(cartaTransform.gameObject);
                        // La movemos a la zona de batalla
                        cartaTransform.SetParent(zonaBatallaOponente);
                        cartaTransform.localPosition = Vector3.zero; // Asegura que la carta se posicione correctamente en la zona
                        // Me aseguro de que la carta mire hacia arriba
                        cartaTransform.localRotation = Quaternion.Euler(0, 0, 0);

                        oponente.mano.Remove(datosCarta); // La eliminamos de la mano lógica del oponente

                        Transform reverso = cartaTransform.Find("Reverso");
                        if(reverso != null)
                        {
                            reverso.gameObject.SetActive(false); // Desactiva el reverso para mostrar la cara de la carta
                        }
                        
                        CartasJugadas cj = cartaTransform.GetComponent<CartasJugadas>();
                        if (cj != null) cj.Inicializar(datosCarta);
                        
                        Debug.Log($"IA: ¡ÉXITO! He invocado a {datosCarta.nombreCarta} en la zona de batalla.");
                    }
                    else
                    {
                        // ESTA ES LA CLAVE: Nos dirá cuánto maná tiene exactamente y por qué falla
                        string manaActual = $"[{oponente.mana[0]}, {oponente.mana[1]}, {oponente.mana[2]}, {oponente.mana[3]}]";
                        Debug.Log($"IA: FRACASO. No tengo maná para {datosCarta.nombreCarta}. Mi maná actual es: {manaActual}");
                    }
                }
            }
        }
    }
    private void ExtraerManaIA()
    {
        // Reviso todas las cartas que ya están en la mesa del oponente
        foreach (Transform cartaTransform in zonaTierrasOponente)
        {
            CartasJugadas cj = cartaTransform.GetComponent<CartasJugadas>();
            
            // Si es una tierra y NO está girada todavía, la giramos
            if (cj != null && cj.girada == false)
            {
                // La giramos visualmente
                cj.RotarCarta(); 
                
                // Le metemos el maná en el "bolsillo" al oponente
                oponente.SumarMana(cartaTransform.gameObject);
                
                Debug.Log("IA: He girado una tierra para obtener maná.");
            }
        }
    }

}
