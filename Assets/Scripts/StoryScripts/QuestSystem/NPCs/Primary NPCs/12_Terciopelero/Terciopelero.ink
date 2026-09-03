// ============================================================
// MISIÓN 12 — CON EL MAESTRO TERCIOPELERO
// NPC: Jean-Jacques Delon (maestro terciopelero)  |  questId: "Terciopelero"
// MECÁNICA DE DOBLE ATUENDO: Caterina ve al niño (de mujer) pero el maestro
//   solo cierra trato con un HOMBRE -> hay que volver de Josep ("marido").
//   También vale presentarse directamente de Josep.
// Apuesta: si gana Josep -> compra al niño por 18 dineros + 2 cartas raras
//          si gana el maestro -> compra al niño por 3 sueldos + 3 cartas raras
// Resultado: en ambos casos Josep se lleva al niño (M13).
// ============================================================


// ------------------------------------------------------------
// PRELUDIO (de mujer): el maestro remite a "vuestro marido"
// ------------------------------------------------------------
== Terciopelero_mujer ==
Caterina: Buen día.
Terciopelero: Buen día. ¿Qué se os ofrece?
Caterina: Estoy buscando una pieza elegante, de terciopelo azul, para una verbena.
Terciopelero: Habéis venido al taller adecuado. Niño, trae las muestras para que las vea la señora.
Caterina: Ese niño parece bien dispuesto. Soy nueva en la ciudad y necesito un mozo para mi servicio. ¿Es hijo vuestro?
Terciopelero: Hijo mío. Ni en mis peores pesadillas. Es mi esclavo. Lo compré en la inclusa. Está aprendiendo el oficio, pero me cuesta más que lo que vale.
Caterina: ¿Me lo podríais vender?
Terciopelero: A vos, no. A vuestro marido, quizá. Que venga él y llegaremos a un acuerdo por este ganapán.
Caterina: De acuerdo, hablaré con mi marido. Con Dios.
Terciopelero: Con Dios.
-> END

// ------------------------------------------------------------
// NEGOCIACIÓN (de Josep): se cierra el trato con la partida
// ------------------------------------------------------------
== Terciopelero ==
// Caterina puede iniciar la visita, pero el trato y el combate solo se
// celebran con Josep (el marido al que remite el preludio).
{ outfit == "Caterina": -> Terciopelero_mujer }
Caterina: Buen día.
Terciopelero: Buen día.
Caterina: Mi mujer me ha dicho que quería compraros un esclavo joven, para tenerlo a su servicio. ¿Es ese gaznápiro del rincón?
Terciopelero: Sí, es él.
Caterina: No sé qué le habrá visto al mozo. ¿Qué sabe hacer?
Terciopelero: No sabe leer, ni escribir. Apenas sabe contar. Pero está fuerte y parece sano.
Caterina: ¿Y cuánto pedís por él?
Terciopelero: 3 sueldos podría ser un precio justo.
Caterina: ¿3 sueldos? Mucho me parece. Os ofrezco un trato. Jugamos una partida de Naipes. Si ganáis, os compro al zagal por 3 sueldos y os doy 3 cartas raras. Si os gano yo, os compro el zagal por 18 dineros y me dais 2 cartas raras.
Terciopelero: Hecho. Juguemos.
{ HasGold(36): -> jugar | -> sinDinero }

= jugar
~ StartCombat("Terciopelero.resolucion")
-> DONE

= resolucion
{ combat_won: -> victoria | -> derrota }

= sinDinero
Caterina: Volveré con el dinero. Con Dios.
Terciopelero: Au revoir.
-> END

// ------------------------------------------------------------
// RAMA VICTORIA -> compra al niño por 18 dineros + 2 cartas raras
// ------------------------------------------------------------
= victoria
Terciopelero: Sois un macandón. No entiendo cómo me habéis podido ganar. Llevaos a este lerdo. Espero que os sirva mejor que a mí.
Caterina: Con Dios.
Terciopelero: Au revoir.
~ FinishQuest("Terciopelero", true)
-> END

// ------------------------------------------------------------
// RAMA DERROTA -> compra al niño por 3 sueldos + 3 cartas raras
// ------------------------------------------------------------
= derrota
Caterina: Aquí tenéis el dinero y las cartas.
Terciopelero: De acuerdo, llevaos a este lerdo. Espero que os sirva mejor que a mí.
~ GiveItem("nino")
~ FinishQuest("Terciopelero", false)
-> END
