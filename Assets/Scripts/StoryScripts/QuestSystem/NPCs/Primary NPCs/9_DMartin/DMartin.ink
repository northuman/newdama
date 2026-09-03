// ============================================================
// MISIÓN 9 — EN CASA DE D. MARTÍN DE LA SANTA CRUZ
// NPC: D. Martín de la Santa Cruz  |  questId: "DMartin"  |  DMartinNpc
// requiredOutfit: Josep + requiredItems: "espada_damasco"
//   - Vestida de mujer -> knot "DMartin_rechazo" (el criado la echa)
// Apuesta: 15 dineros + 1 carta mítica
// AL GANAR -> sub-quest del hijo (prostíbulo M10 -> inclusa M11 -> terciopelero M12)
// isRepeatable solo TRAS terminar la sub-quest del hijo (M13).
// ============================================================


== DMartin ==
Caterina: Buen día.
Martín: Buen día.
Caterina: Vengo de parte del marqués de Dos Aguas para daros un objeto personal de vuestro padre, una espada de acero de Damasco. Aquí la tenéis.
Martín: Ah, muchas gracias. ¿Dónde la habéis encontrado?
Caterina: En la tumba de vuestro señor padre. Perdonad que haya tenido que abrirla, pero así me lo indicó el señor marqués. Tened por seguro que, tras coger la espada, dejé el sepulcro como si nada hubiese pasado.
Martín: Percibo algo raro en vos. ¿Sois un sodomita o acaso sois una mujer disfrazada de hombre?
Caterina: ¿Cómo?
Martín: No temáis en descubrir vuestra condición femenina. Como sabéis, soy cristiano nuevo y vivo continuamente disfrazado. Tras el edicto de expulsión de los judíos de 1492, decidí que mi familia y yo nos convirtiésemos a la fe cristiana, y desde entonces vivimos bajo un disfraz inseguro, pero eficaz. Os lo cuento porque no creo que os creyeran si nos delatáis, ya que acabarían descubriendo que sois una mujer. Os interesa la discreción, como a nosotros.
Caterina: Tenéis razón. Soy una mujer. Mi nombre es Caterina de Eraso y soy la sobrina de la Madre Superiora del Monasterio de la Merced, extramuros de Levante.
Martín: ¿Y por qué vais disfrazada?
Caterina: Quería salir del monasterio, que para mí era como una cárcel divina, y vivir mi propia vida. Mi sueño es ir a Sevilla para embarcarme al Nuevo Mundo y prosperar allí, como mujer o como hombre, así lo quiera la Providencia.
Martín: Estáis arriesgando vuestra vida por un sueño, algo tan honorable como loco. ¿Y cómo pretendéis llegar a Sevilla? Los pasajes de los barcos que marchan hacia allá no cuestan poco.
Caterina: He oído que va a haber un torneo de Naipes en la ciudad y que el premio será un pasaje a Sevilla.
Martín: Así es, pero para jugar ese torneo debéis conseguir una invitación.
Caterina: ¿Y es muy difícil conseguirla?
Martín: No, teniendo los contactos adecuados y el nivel de juego necesario. ¿Sabéis jugar a los Naipes?
Caterina: La duda ofende, mi señor.
Martín: Juguemos pues. 15 dineros y una carta mítica serán la recompensa para el ganador.
{ HasGold(15): -> jugar | -> sinDinero }

= jugar
Caterina: Adelante, juguemos.
~ StartCombat("DMartin.resolucion")
-> DONE

= resolucion
{ combat_won: -> victoria | -> derrota }

= sinDinero
Caterina: Señor, volveré cuando tenga el dinero.
Martín: Aquí os estaré esperando.
-> END

// ------------------------------------------------------------
// RAMA VICTORIA -> el favor: localizar a su hijo (sub-quest M10-M13)
// ------------------------------------------------------------
= victoria
Martín: Aquí tenéis las monedas y la carta. Sois más hábil de lo que pensaba.
Caterina: Gracias, señor.
Martín: Quizá me podáis hacer un favor. A cambio, os conseguiré la invitación para el torneo de Naipes de la ciudad. ¿Os interesa?
Caterina: Por supuesto, señor. ¿Qué he de hacer?
Martín: Hace unos 10 años, quise casarme con la hija de un maestro del gremio de los plateros. Inés, que así se llamaba mi pretendida, y yo estábamos enamorados y la pasión nos cegó. Ella se quedó embarazada. El platero la envió a la Casa de Recogidas de Santa María Magdalena. Inés dio a luz allí a nuestro hijo, pero se lo arrebataron al nacer. Tras recuperarse del parto, consiguió escapar y acabó en el prostíbulo de la puerta este de la ciudad.
Caterina: ¿Y no sabéis nada de vuestro hijo?
Martín: Apenas nada. No tengo heredero varón, ni esposa. Por eso me gustaría saber si mi hijo vive, para reconocerlo y enseñarle mis negocios.
Caterina: ¿Y qué queréis que haga?
Martín: Hablad con Inés e intentad localizar a mi hijo. Volved con información veraz sobre mi hijo y os daré la invitación para el gran torneo de Naipes de Levante.
Caterina: De acuerdo, señor, nos veremos pronto. Con Dios.
Martín: Con Yahveh.
~ TakeItem("espada_damasco")
~ SetFlag("encargo_hijo", true)
~ FinishQuest("DMartin", true)
-> END

// ------------------------------------------------------------
// RAMA DERROTA -> revancha disponible
// ------------------------------------------------------------
= derrota
Martín: No sois tan buena como pensabais.
Caterina: Aquí tenéis las monedas y la carta. ¿Me concederéis la revancha?
Martín: Cuando queráis. Las condiciones serán las mismas. Con Dios.
Caterina: Con Dios.
~ FinishQuest("DMartin", false)
-> END

= reintento
Caterina: Señor, ya estoy de vuelta. ¿Jugamos?
Martín: Preparad vuestro mazo. Estáis a punto de recibir una lección.
~ StartCombat("DMartin.resolucion")
-> DONE

// ------------------------------------------------------------
// REMATCH (solo si terminó la sub-quest del hijo): 15 dineros + 1 carta mítica
// ------------------------------------------------------------
= revancha
Caterina: Señor, ¿jugamos a los Naipes?
Martín: De acuerdo. 15 dineros y una carta mítica serán la recompensa para el ganador. Preparad vuestro mazo.
~ StartCombat("DMartin.resolucion_revancha")
-> DONE

= resolucion_revancha
{ combat_won: -> revancha_victoria | -> revancha_derrota }

= revancha_victoria
Martín: Aquí tenéis las monedas y la carta. Sois una donillera.
Caterina: Gracias, señor.
~ FinishQuest("DMartin", true)
-> END

= revancha_derrota
Martín: Sois una descocada.
Caterina: Aquí tenéis las monedas y la carta. ¿Querréis jugar de nuevo?
Martín: Cuando queráis. Las condiciones serán las mismas. Con Yahveh.
Caterina: Con Dios.
~ FinishQuest("DMartin", false)
-> END

// ============================================================
// VARIANTE DE RECHAZO — Caterina llega VESTIDA DE MUJER
// ============================================================
== DMartin_rechazo ==
Caterina: Buen día. Necesito hablar con el señor de la casa.
Criado: ¿Quién sois?
Caterina: Caterina de Eraso. Vengo de parte del marqués de Dos Aguas.
Criado: Vuestro aspecto no es el de una señora. ¿Sois parte de su servicio?
Caterina: Soy una recadera.
Criado: ¿Recadera? ¿No seréis una ramera? Mi señor no os va a recibir. No deben verle con mujeres. Podrían pensar que sois su barragana. No volváis.
-> END
