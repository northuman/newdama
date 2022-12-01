using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Partida : MonoBehaviour {

    const float ENTRETIEMPO = 1f;

    public enum Fase { MULLIGAN, MANTENIMIENTO, ROBO, PRINCIPAL, COMBATE, PRINCIPAL2, FIN };
    public enum Turno { JUGADOR, OPONENTE };
    public enum Combate { ATACANTES, BLOQUEADORES, ORDEN_BLOQUEADORES, RESPUESTA_ATACANTE, RESPUESTA_DEFENSOR, DANYO, CEMENTERIO }

    public static Fase faseActual = Fase.MULLIGAN;
    public static Turno turno = Turno.JUGADOR;
    public static Combate momentoCombate = Combate.ATACANTES;

    public static bool primerTurno = true;
    public static bool quedarMano = false;
    public static bool pasarFase = false;
    public static bool continuarFase = false;

    public static Jugador jugador;
    public static Jugador oponente;

    public static GameObject cajaDialogo;
    public static GameObject botonAceptar;
    public static GameObject botonCancelar;
    public static Button botonFases;
    
    void Start() {
        
        EmpezarPartida();
    }

    void EmpezarPartida() {

        jugador = GameObject.Find("Jugador").GetComponent<Jugador>();
        oponente = GameObject.Find("Oponente").GetComponent<Jugador>();
        //manoJugador = GameObject.Find("Mano Jugador");

        botonFases = GameObject.Find("Boton Fases").GetComponent<Button>();
        botonFases.onClick.AddListener(delegate { SetPasarFaseTrue(); });
        cajaDialogo = GameObject.Find("Caja de Dialogo");
        botonAceptar = GameObject.Find("Boton Aceptar");
        botonCancelar = GameObject.Find("Boton Cancelar");
        cajaDialogo.SetActive(false);

        jugador.Barajar();
        oponente.Barajar();

        StartCoroutine(EmpezarCicloPartida());
    }

    // Ciclo Partida --------------------------------------------------------------------------------------------------

    IEnumerator EmpezarCicloPartida() {
  
        Mulligan(); 

        yield return new WaitUntil(GetQuedarMano);
        
        while(jugador.vida > 0 && oponente.vida > 0) {

            StartCoroutine(Mantenimiento());
            yield return new WaitUntil(GetPasarFase);
            if(primerTurno) { primerTurno = false; }
            else { Robo(); }
            StartCoroutine(FasePrincipal(1));
            yield return new WaitUntil(GetPasarFase);
            StartCoroutine(FaseCombate());
            yield return new WaitUntil(GetPasarFase);
            StartCoroutine(FasePrincipal(2));
            yield return new WaitUntil(GetPasarFase);
            EndTurn();
            yield return new WaitForSeconds(2f);
            
            //break;
        }
    }

    bool GetQuedarMano() {
        
        return quedarMano; 
    }

    public void SetPasarFaseTrue() {

        pasarFase = true;
    }

    // Ciclo Partida --------------------------------------------------------------------------------------------------

    // Fase Fin Turno -------------------------------------------------------------------------------------------------

    void EndTurn() {

        //Si hay efectos que hacer, preguntar si se desea hacerlos
        pasarFase = false;
        continuarFase = false;
        Debug.Log("Fase Final de Turno");
        faseActual = Fase.FIN;

        jugador.permitidoJugarCartas = false;
        oponente.permitidoJugarCartas = false;

        if(turno == Turno.JUGADOR) { 
            
            turno = Turno.OPONENTE; 
            jugador.tierraDelTurnoJugada = false;
            jugador.RestarVidaPorMana();
        }

        else { 
            
            turno = Turno.JUGADOR; 
            oponente.tierraDelTurnoJugada = false;
            oponente.RestarVidaPorMana();
        }
    }

    // Fase Fin Turno -------------------------------------------------------------------------------------------------

    // Fase Combate ---------------------------------------------------------------------------------------------------

    void MandarCementerio() {

        Debug.Log("Mandar Cementerio");
        momentoCombate = Combate.CEMENTERIO;

        jugador.MandarCementerio();
        oponente.MandarCementerio();
    }

    void Enfrentamientos() {

        continuarFase = false;

        Debug.Log("Enfrentamientos");

        if(turno == Turno.JUGADOR) {

            GameObject criaturas = DropZone.criaturasJugador;

            for(int i=0; i<criaturas.transform.childCount; i++) {

                Transform criatura = criaturas.transform.GetChild(i);
                Arrastrable arrastrable = criatura.gameObject.GetComponent<Arrastrable>();

                if(arrastrable.atacando) {

                    if(arrastrable.bloqueadaPor.Count == 1) {

                        arrastrable.Combate(arrastrable.bloqueadaPor[0]);
                        Debug.Log("Atacante: " + arrastrable.carta.nombreCarta);
                        Debug.Log("Bloqueador: " + arrastrable.bloqueadaPor[0].carta.nombreCarta);
                    }

                    else if (arrastrable.bloqueadaPor.Count > 1) {

                        ModificarCajaDialogo(3);
                        cajaDialogo.SetActive(true);
                        botonAceptar.SetActive(false);
                        botonCancelar.SetActive(false);

                        momentoCombate = Combate.ORDEN_BLOQUEADORES;

                        for(int j=0; j<arrastrable.bloqueadaPor.Count; j++) {

                            arrastrable.bloqueadaPor[j].gameObject.transform.SetParent(cajaDialogo.transform);
                            Arrastrable.atacante = arrastrable;
                        }
                    }

                    else {

                        arrastrable.InflingirDanyo(Partida.oponente);
                        Debug.Log("Ataca directamente: " + arrastrable.carta.nombreCarta);
                    }
                }
            }
        }

        if(turno == Turno.OPONENTE) {

            GameObject criaturas = Partida.oponente.goCriaturas;

            for(int i=0; i<criaturas.transform.childCount; i++) {

                Transform criatura = criaturas.transform.GetChild(i);
                Arrastrable arrastrable = criatura.gameObject.GetComponent<Arrastrable>();

                if(arrastrable.atacando) {

                    if(arrastrable.bloqueadaPor.Count == 1) {

                        arrastrable.Combate(arrastrable.bloqueadaPor[0]);
                        Debug.Log("Atacante: " + arrastrable.carta.nombreCarta);
                        Debug.Log("Bloqueador: " + arrastrable.bloqueadaPor[0].carta.nombreCarta);
                    }

                    else if(arrastrable.bloqueadaPor.Count > 1) {

                        //Bloqueos
                    }

                    else {

                        arrastrable.InflingirDanyo(Partida.jugador);
                        Debug.Log("Ataca directamente: " + arrastrable.carta.nombreCarta);
                    }
                }
            }

            continuarFase = true;
        }
    }

    void Bloqueadores() {

        continuarFase = false;

        Debug.Log("Declarar Bloqueadores");
        momentoCombate = Combate.BLOQUEADORES;
    }

    IEnumerator FaseCombate() {

        faseActual = Fase.COMBATE;
        pasarFase = false;
        continuarFase = false;

        jugador.permitidoJugarCartas = false;
        oponente.permitidoJugarCartas = false;

        Debug.Log("Fase de Combate");

        if(turno == Turno.JUGADOR && jugador.criaturasActivas > 0) {

            Debug.Log("Jugador declara Atacantes");
            momentoCombate = Combate.ATACANTES;

            //Cambiar Funcion del Boton de fases
            botonFases.GetComponent<Button>().onClick.RemoveAllListeners();
            botonFases.GetComponent<Button>().onClick.AddListener(delegate { ContinuarFase(); });

            yield return new WaitUntil(GetContinuarFase);

            //if(cartas atacando)
            if(jugador.CriaturasAtacando() > 0) {

                Bloqueadores();
                //yield return new WaitUntil(GetPasarFase);
                yield return new WaitUntil(GetContinuarFase);
                Enfrentamientos();
                yield return new WaitUntil(GetContinuarFase);
                MandarCementerio();
                yield return new WaitUntil(GetContinuarFase);
            }

            else if(jugador.CriaturasAtacando() == 0) {

                pasarFase = true;
            }

            botonFases.GetComponent<Button>().onClick.RemoveAllListeners();
            botonFases.GetComponent<Button>().onClick.AddListener(delegate { SetPasarFaseTrue(); });
        }

        else if(turno == Turno.OPONENTE && oponente.criaturasActivas > 0) {

            Debug.Log("Oponente declara Atacantes");
            momentoCombate = Combate.ATACANTES;
            Debug.Log("Criaturas activas: " + oponente.criaturasActivas);

            //Cambiar Funcion del Boton de fases
            botonFases.GetComponent<Button>().onClick.RemoveAllListeners();
            botonFases.GetComponent<Button>().onClick.AddListener(delegate { ContinuarFase(); });

            yield return new WaitUntil(GetContinuarFase);

            //if(cartas atacando)
            if(oponente.CriaturasAtacando() > 0) {

                Bloqueadores();
                //yield return new WaitUntil(GetPasarFase);
                yield return new WaitUntil(GetContinuarFase);
                Enfrentamientos();
                yield return new WaitUntil(GetContinuarFase);
                MandarCementerio();
                yield return new WaitUntil(GetContinuarFase);
            }

            else if(oponente.CriaturasAtacando() == 0) {

                pasarFase = true;
            }

            botonFases.GetComponent<Button>().onClick.RemoveAllListeners();
            botonFases.GetComponent<Button>().onClick.AddListener(delegate { SetPasarFaseTrue(); });
        }

        else {

            pasarFase = true;
        }

        //Hacer brillar criaturas sin mareo
    }

    // Fase Combate ---------------------------------------------------------------------------------------------------

    // Fase Principal -------------------------------------------------------------------------------------------------

    IEnumerator FasePrincipal(int n) {
        
        pasarFase = false;

        if(n == 1) {

            Debug.Log("Fase Principal");
            faseActual = Fase.PRINCIPAL;
        }

        else {

            Debug.Log("Fase Principal2");
            faseActual = Fase.PRINCIPAL2;
        }
        
        yield return new WaitForSeconds(ENTRETIEMPO);

        if(turno == Turno.JUGADOR) {

            jugador.permitidoJugarCartas = true;
        }

        else if(turno == Turno.OPONENTE) {

            oponente.permitidoJugarCartas = true;
        }

        else {

            Debug.Log("Algo rompiste en Partida -> FasePrincipal");
        }
    }

    bool GetPasarFase() {

        return pasarFase;
    }

    // Fase Principal -------------------------------------------------------------------------------------------------

    // Mantenimiento --------------------------------------------------------------------------------------------------

    bool GetContinuarFase() {

        return continuarFase;
    }

    void ContinuarFase() {

        continuarFase = true;
    }

    IEnumerator Mantenimiento() {

        Debug.Log("Mantenimiento");
        faseActual = Fase.MANTENIMIENTO;
        yield return new WaitForSeconds(ENTRETIEMPO);

        if(jugador.ManaRestante() > 0) {

            StartCoroutine(PreguntarJugarCarta());

            if(turno == Turno.JUGADOR)
                yield return new WaitUntil(GetContinuarFase);
        }

        EnderezarCartas();
    }

    void EnderezarCartas() {

        Debug.Log("Enderezar Cartas");
        pasarFase = false;
        continuarFase = false;

        if(turno == Turno.JUGADOR) { 

            jugador.EnderezarTierras();
            jugador.EnderezarCriaturas();
        }

        else {

            oponente.EnderezarTierras();
            oponente.EnderezarCriaturas();
        }

        pasarFase = true;
    }

    public static void ContinuarTurno() {

        cajaDialogo.SetActive(false);
        continuarFase = true;
    }

    public static void JugarInterrupcion() {

        cajaDialogo.SetActive(false);
    }

    // Mantenimiento --------------------------------------------------------------------------------------------------

    // Robo -----------------------------------------------------------------------------------------------------------

    void Robo() {

        Debug.Log("Robo del turno");

        faseActual = Fase.ROBO;

        if(turno == Turno.JUGADOR) { jugador.RobarCarta(); }
        else { oponente.RobarCarta(); }
    }

    // Robo -----------------------------------------------------------------------------------------------------------

    //Mulligan --------------------------------------------------------------------------------------------------------

    void Mulligan() {

        Debug.Log("Mulligan");

        //Jugador y Oponente roban su primera mano
        faseActual = Fase.MULLIGAN;
        StartCoroutine(jugador.RobarCartas(jugador.mulligan));
        StartCoroutine(oponente.RobarCartas(oponente.mulligan));
    }

    public static Carta CartaDesdeGameObject(GameObject cartaGameObject) {

        Carta carta;

        carta = cartaGameObject.GetComponent<MostrarDatosCarta>().carta;

        return carta;
    }

    public static void QuedarMano() {

        cajaDialogo.SetActive(false);
        quedarMano = true;
        faseActual = Fase.MANTENIMIENTO;

        botonAceptar.GetComponent<Button>().onClick.RemoveAllListeners();
        botonCancelar.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    //Mulligan --------------------------------------------------------------------------------------------------------

    //Dialogo ---------------------------------------------------------------------------------------------------------

    //Modificar Dialogo
    //Motivo 1 -> Mulligan

    static void ModificarCajaDialogo(int motivo) {

        TMPro.TMP_Text textoDialogo = cajaDialogo.transform.GetChild(0).gameObject.GetComponent<TMPro.TMP_Text>(); //Texto de Pregunta
        TMPro.TMP_Text textoAceptar = botonAceptar.transform.GetChild(0).GetComponent<TMPro.TMP_Text>(); //Boton Aceptar
        TMPro.TMP_Text textoCancelar = botonCancelar.transform.GetChild(0).GetComponent<TMPro.TMP_Text>(); //Boton Cancelar

        switch(motivo) {

            case 0:
                Debug.Log("Se ha muerto");
                break;

            case 1:

                textoDialogo.text = "¿Desea hacer Mulligan a " + (jugador.mulligan-1) + "?";

                textoAceptar.text = "Hacer Mulligan";
                botonAceptar.GetComponent<Button>().onClick.AddListener(delegate { jugador.DevolverManoInicial(); });
 
                textoCancelar.text = "Quedar Mano";
                botonCancelar.GetComponent<Button>().onClick.AddListener(delegate { QuedarMano(); });

                break;

            case 2:

                textoDialogo.text = "¿Desea jugar alguna carta antes de pasar de fase?";

                textoAceptar.text = "Jugar Carta";
                botonAceptar.GetComponent<Button>().onClick.AddListener(delegate { JugarInterrupcion(); });

                textoCancelar.text = "Continuar";
                botonCancelar.GetComponent<Button>().onClick.AddListener(delegate { ContinuarTurno(); });

                break;

            case 3:

                textoDialogo.text = "¿A qué bloqueador desea atacar primero?";

                break;
        }
    }

    public static IEnumerator PreguntarMulligan() {

        yield return new WaitForSeconds(ENTRETIEMPO);

        ModificarCajaDialogo(1);

        cajaDialogo.SetActive(true);
    }

    IEnumerator PreguntarJugarCarta() {

        yield return new WaitForSeconds(ENTRETIEMPO);

        ModificarCajaDialogo(2);

        cajaDialogo.SetActive(true);
    }

    //Dialogo ---------------------------------------------------------------------------------------------------------
}

/*
1. Elegir quién va primero 
2. Mulligan
3. Fase de Mantenimiento
4. Fase de Robo
5. Fase Principal
6. Fase de Combate
7. Segunda Principal
8. Fin de Turno
9. Volver a la 3 para el otro jugador
*/