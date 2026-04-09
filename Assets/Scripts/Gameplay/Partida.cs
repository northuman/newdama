using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UtilCartas;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/*
* Controla el flujo de la partida, aquí va el sistema de turnos.
* 
*/

public class Partida : MonoBehaviour
{
    public enum OrdenJugadores { JUGADOR, OPONENTE };
    public enum Fases { INICIO, PRINCIPAL_1, BLOQUEO, COMBATE, PRINCIPAL_2, FINAL };
    public Jugador jugador;
    public Jugador oponente;
    public bool tierrasJugadasEsteTurno = false;
    List<Jugador> jugadores;
    public bool turno = false; //false = turno jugador; true = turno oponente
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
    public Dictionary<GameObject, GameObject> emparejamientos = new Dictionary<GameObject, GameObject>(); // Para almacenar qué criatura bloquea a cuál
    public GameObject bloqueadorSeleccionado = null; // Para saber qué bloqueador ha elegido el jugador durante la fase de bloqueo
    public TextMeshProUGUI textoVidaOponente;
    public TextMeshProUGUI textoVidaJugador;
    public GameObject panelVictoria;
    public GameObject panelDerrota;
    private bool juegoTerminado = false;



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
                faseActual = Fases.BLOQUEO;
                FaseBloqueo();
                break;

            case Fases.BLOQUEO:
                ResolverCombate(); // Resolvemos el bloqueo antes de pasar a la siguiente fase
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

        VerificarEstadoPartida();
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
        if (criaturasAtacantes.Count == 0) return;

        int danoTotal = 0;
        Jugador objetivo = turno ? jugador : oponente; // Quién recibe los golpes

        foreach (GameObject atacante in criaturasAtacantes)
        {
            if (atacante != null)
            {
                Carta datosAtacante = atacante.GetComponent<MostrarCarta>().GetCarta();
                
                // NUEVO: Miramos en el Diccionario si alguien bloquea a este atacante
                GameObject bloqueador = emparejamientos[atacante];

                if (bloqueador == null)
                {
                    // Nadie lo bloquea -> Daño a la cara
                    danoTotal += datosAtacante.fuerza; 
                    Debug.Log($"- {datosAtacante.nombreCarta} no es bloqueado. Hace {datosAtacante.fuerza} de daño directo.");
                }
                else
                {
                    // Ha sido bloqueado -> No hay daño a la cara
                    Carta datosBloqueador = bloqueador.GetComponent<MostrarCarta>().GetCarta();
                    Debug.Log($"- ¡CHOQUE! {datosBloqueador.nombreCarta} bloquea a {datosAtacante.nombreCarta}. (0 daño a tu héroe)");
                    
                    // (Nota: Más adelante haremos que las criaturas se quiten vida entre ellas aquí)
                }
            }
        }

        // Aplicamos solo el daño que logró pasar las defensas
        objetivo.vida -= danoTotal; 
        Debug.Log($"¡BOOM! El objetivo recibe {danoTotal} de daño. Le quedan {objetivo.vida} vidas.");

        // Actualizamos los textos
        if (turno == false && textoVidaOponente != null) textoVidaOponente.text = objetivo.vida.ToString();
        else if (turno == true && textoVidaJugador != null) textoVidaJugador.text = objetivo.vida.ToString();

        // Limpiamos la mesa para el siguiente turno
        criaturasAtacantes.Clear();
        emparejamientos.Clear();
        bloqueadorSeleccionado = null; // Reiniciamos el cursor
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
        
        JugarTierraIA();
        yield return new WaitForSeconds(1.0f);

        ExtraerManaIA();
        yield return new WaitForSeconds(1.0f);

        JugarCriaturasIA();
        yield return new WaitForSeconds(1.5f);

        AvanzarFase();
        yield return new WaitForSeconds(1.5f);

        //La IA declara ataques y el código interno nos mete en la faese de bloqueo
        AtacarIA();

        while(faseActual == Fases.BLOQUEO)
        {
            yield return null; // Esperamos sin avanzar el tiempo hasta que el jugador termine de bloquear
        }

        yield return new WaitForSeconds(1.0f);

        if (faseActual == Fases.PRINCIPAL_2)
        {
            Debug.Log("IA: Ya he terminado mis fases. Pasando a Fase Final.");
            AvanzarFase(); // Empuja a la Fase Final (donde descartará si le sobran)
        }
    }

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

    public void VerificarEstadoPartida()
    {
        if(juegoTerminado) return;
        if(oponente.vida <= 0)
        {
            FinalizarJuego(true);
        }
        else if (jugador.vida <= 0)
        {
            FinalizarJuego(false);
        }
    }

    public void FinalizarJuego(bool victoria)
    {
        juegoTerminado = true;
        if(victoria){
            panelVictoria.SetActive(true);
            Debug.Log("¡Felicidades! Has ganado la partida.");
        }
        else
        {
            panelDerrota.SetActive(true);
            Debug.Log("Lo siento, has perdido la partida.");
        }
    }

    public void ReiniciarPartida()
    {
        Time.timeScale = 1f; //si no, el juego empezará congelado
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        // TRUCO: Quitarle 10 de vida al Oponente
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (oponente != null)
            {
                oponente.vida -= 19;
                // Actualizamos el texto visual para ver el cambio
                if (textoVidaOponente != null) 
                    textoVidaOponente.text = oponente.vida.ToString();
                
                Debug.Log("DEBUG: Has usado la tecla K. Vida oponente: " + oponente.vida);
                
                // Comprobamos si ha muerto para que salga el panel de Victoria
                VerificarEstadoPartida();
            }
        }

        // TRUCO: Quitarte 10 de vida a TI (para probar la derrota)
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (jugador != null)
            {
                jugador.vida -= 10;
                if (textoVidaJugador != null) 
                    textoVidaJugador.text = jugador.vida.ToString();
                
                Debug.Log("DEBUG: Has usado la tecla L. Tu vida: " + jugador.vida);
                
                VerificarEstadoPartida();
            }
        }
    }

    void FaseBloqueo()
    {
        Debug.Log("--- Fase de bloqueo ---");
        emparejamientos.Clear(); // Limpiamos los bloqueos anteriores
        foreach(GameObject atacante in criaturasAtacantes)
        {
            emparejamientos.Add(atacante, null); // Inicialmente, ningún atacante tiene bloqueador asignado
        }

        if(turno == false)
        {
            Debug.Log("IA: viendo como bloquear tus atacantes...");
            // Aquí la IA debería analizar qué criaturas tiene en su zona de batalla y decidir cómo bloquear

            //TODO: Implementar lógica de bloqueo de la IA (por ahora no bloquea nada)
            AvanzarFase(); // Pasamos a la siguiente fase aunque no haya bloqueos
        }
        else
        {
            if(criaturasAtacantes.Count > 0)
            {
                Debug.Log("Jugador: Te atacan. Haz click n tu criatura, luegop en el enemigo para bloquear");
                //pausa y se espera el raton
            }
            else
            {
                Debug.Log("Nadie te ataque, pasando de fase...");
                AvanzarFase();
            }
        }
    }

    private void AtacarIA()
    {
        Debug.Log("IA: Analizando a quién mandar al ataque...");
        
        // La IA mira todas las cartas que tiene en su lado de la mesa
        foreach (Transform cartaTransform in zonaBatallaOponente)
        {
            CartasJugadas cj = cartaTransform.GetComponent<CartasJugadas>();
            MostrarCarta mc = cartaTransform.GetComponent<MostrarCarta>();

            // Si es una criatura y NO está girada... ¡Al ataque!
            if (cj != null && mc != null && cj.girada == false && mc.tipo == "Criatura")
            {
                cj.RotarCarta(); // La gira visualmente
                criaturasAtacantes.Add(cartaTransform.gameObject); // La mete en la lista
                
                Debug.Log($"IA: ¡Ataco con {mc.GetCarta().nombreCarta}!");
            }
        }

        // Una vez ha decidido quién ataca, en lugar de calcular el daño directo,
        // pasamos a TU fase de Bloqueo.
        AvanzarFase(); 
    }

}
