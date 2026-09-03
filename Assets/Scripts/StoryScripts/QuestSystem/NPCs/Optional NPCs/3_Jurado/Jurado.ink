// ============================================================
// OPCIONAL 3 — JURADO DE LA CIUDAD  (farming post-M14)
// NPC: D. Ferran de la Clau  |  questId: "Jurado"
// Recompensa: cartas MÍTICAS VERDES.  Apuesta: 2 sueldos (gana 1 sueldo y medio).
// requiredOutfit: Any  |  isRepeatable = true
// Plantilla: NOBLES Y PROHOMBRES DE LA CIUDAD
// ============================================================


== Jurado ==
Caterina: Buen día, señor. Con todos los respetos, ¿os apetece jugar una partida de Naipes?
Jurado: Sí, por supuesto. Mis condiciones son 2 sueldos y una carta mítica verde. ¿Estáis de acuerdo?
Caterina: Sí, juguemos.
~ StartQuest("Jurado")
-> END

= victoria
Jurado: Buen juicio el vuestro en la mesa. Aquí tenéis vuestra carta mítica verde y las monedas.
Caterina: Gracias. Con Dios.
~ FinishQuest("Jurado")
-> END

= derrota
Caterina: Quizá en otra ocasión. Aquí tenéis lo acordado.
Jurado: Volved cuando queráis, joven.
~ FinishQuest("Jurado")
-> END

= rechazo
Caterina: Quizá en otra ocasión.
-> END
