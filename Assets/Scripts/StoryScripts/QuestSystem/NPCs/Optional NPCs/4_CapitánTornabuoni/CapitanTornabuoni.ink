// ============================================================
// OPCIONAL 4 — CAPITÁN GENOVÉS DEL MUELLE  (farming post-M14)
// NPC: Giovanni Tornabuoni  |  questId: "CapitanTornabuoni"
// Recompensa: cartas MÍTICAS NEGRAS.  Apuesta: 2 sueldos (gana 1 sueldo y medio).
// requiredOutfit: Any  |  isRepeatable = true
// NOTA: es también el capitán que lleva a Caterina a Sevilla al final del juego.
// Plantilla: NOBLES Y PROHOMBRES / TAHÚRES
// ============================================================


== CapitanTornabuoni ==
Caterina: Buen día, capitán. ¿Os apetece jugar una partida de Naipes?
Capitán Tornabuoni: Sin duda. Tenéis pinta de zampalimosnas, pero si ponéis dinero y cartas, no hay problema. Mis condiciones son 2 sueldos y una carta mítica negra. ¿Estáis de acuerdo?
Caterina: Sí, juguemos.
~ StartQuest("CapitanTornabuoni")
-> END

= victoria
Capitán Tornabuoni: Per Bacco! Jugáis mejor que muchos lobos de mar. Aquí tenéis vuestra carta mítica negra y las monedas.
Caterina: Gracias. Con Dios.
~ FinishQuest("CapitanTornabuoni")
-> END

= derrota
Caterina: Quizá en otra ocasión. Aquí tenéis lo acordado.
Capitán Tornabuoni: Volved cuando queráis, marinero de agua dulce.
~ FinishQuest("CapitanTornabuoni")
-> END

= rechazo
Caterina: Quizá en otra ocasión.
-> END
