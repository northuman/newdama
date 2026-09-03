// ============================================================
// OPCIONAL 1 — CHANTRE DE LA CATEDRAL  (farming post-M14)
// NPC: fray Tomás del Canto Gregoriano  |  questId: "Chantre"
// Recompensa: cartas MÍTICAS BLANCAS.  Apuesta: 2 sueldos (gana 1 sueldo y medio).
// requiredOutfit: Any  |  isRepeatable = true
// Plantilla: ERMITAÑOS / FRAILES / MONJAS / CURAS
// ============================================================


== Chantre ==
Caterina: Buen día. Dios esté con vos.
Chantre: ¿Qué os trae por aquí?
Caterina: Estoy buscando rivales para jugar a los Naipes.
Chantre: Perfecto. Tengo un rato libre. Acepto jugar. Mis condiciones son 2 sueldos y una carta mítica blanca. ¿Estáis de acuerdo?
Caterina: Sí, juguemos.
~ StartQuest("Chantre")
-> END

= victoria
Chantre: Cantáis victoria como un coro de ángeles. Aquí tenéis vuestra carta mítica blanca y las monedas.
Caterina: Gracias. Con Dios.
~ FinishQuest("Chantre")
-> END

= derrota
Caterina: Quizá en otra ocasión. Aquí tenéis lo acordado.
Chantre: Que el Altísimo os guíe. Volved cuando queráis.
~ FinishQuest("Chantre")
-> END

= rechazo
Caterina: Quizá en otra ocasión.
-> END
