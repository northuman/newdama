// ============================================================
// OPCIONAL 2 — CATEDRÁTICO DE LA UNIVERSIDAD  (farming post-M14)
// NPC: doctor Enrique de Vitoria  |  questId: "Catedratico"
// Recompensa: cartas MÍTICAS ROJAS.  Apuesta: 2 sueldos (gana 1 sueldo y medio).
// requiredOutfit: Any  |  isRepeatable = true
// Plantilla: NOBLES Y PROHOMBRES DE LA CIUDAD
// ============================================================


== Catedratico ==
Caterina: Buen día, señor. Con todos los respetos, ¿os apetece jugar una partida de Naipes?
Catedrático: Sí, por supuesto. Mis condiciones son 2 sueldos y una carta mítica roja. ¿Estáis de acuerdo?
Caterina: Sí, juguemos.
~ StartQuest("Catedratico")
-> END

= victoria
Catedrático: Vuestra lógica supera a la de mis alumnos. Aquí tenéis vuestra carta mítica roja y las monedas.
Caterina: Gracias. Con Dios.
~ FinishQuest("Catedratico")
-> END

= derrota
Caterina: Quizá en otra ocasión. Aquí tenéis lo acordado.
Catedrático: Volved cuando queráis ampliar vuestra educación.
~ FinishQuest("Catedratico")
-> END

= rechazo
Caterina: Quizá en otra ocasión.
-> END
