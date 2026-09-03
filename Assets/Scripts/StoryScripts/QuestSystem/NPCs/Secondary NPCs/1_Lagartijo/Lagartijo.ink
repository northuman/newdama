// ============================================================
// SECUNDARIO — LADRÓN (mundo abierto, repetible)
// NPC: Lagartijo  |  questId: "Lagartijo"
// requiredOutfit: Any  |  isRepeatable = true
// Plantilla: LADRONES.  Apuesta: 4 dineros + 1 carta poco común.
// ============================================================


== Lagartijo ==
Lagartijo: ¿La bolsa o la vida?
Caterina: Os daré todo lo que llevo si me ganáis a una partida de Naipes. ¿Jugáis o sois acaso un cagalindes?
Lagartijo: Nadie me llama cobarde. Mis condiciones son 4 dineros y 1 carta poco común. ¿Estáis de acuerdo?
Caterina: Sí, barajad vuestro mazo.
~ StartQuest("Lagartijo")
-> END

= victoria
Lagartijo: Maldita sea mi suerte. Tomad, marchaos antes de que cambie de idea.
~ FinishQuest("Lagartijo")
-> END

= derrota
Caterina: Aquí tenéis. Volveremos a vernos en el camino.
~ FinishQuest("Lagartijo")
-> END
