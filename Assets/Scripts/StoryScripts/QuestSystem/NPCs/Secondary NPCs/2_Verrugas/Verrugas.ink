// ============================================================
// SECUNDARIO — LADRÓN (mundo abierto, repetible)
// NPC: Verrugas  |  questId: "Verrugas"
// requiredOutfit: Any  |  isRepeatable = true
// Plantilla: LADRONES.  Apuesta: 6 dineros + 1 carta rara.
// ============================================================


== Verrugas ==
Verrugas: ¿La bolsa o la vida?
Caterina: Os daré todo lo que llevo si me ganáis a una partida de Naipes. ¿Jugáis o sois acaso un cagalindes?
Verrugas: Nadie me llama cobarde. Mis condiciones son 6 dineros y 1 carta rara. ¿Estáis de acuerdo?
Caterina: Sí, barajad vuestro mazo.
~ StartQuest("Verrugas")
-> END

= victoria
Verrugas: Maldita sea mi suerte. Tomad, marchaos antes de que cambie de idea.
~ FinishQuest("Verrugas")
-> END

= derrota
Caterina: Aquí tenéis. Volveremos a vernos en el camino.
~ FinishQuest("Verrugas")
-> END
