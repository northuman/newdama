// ============================================================
// SECUNDARIO — LADRÓN (mundo abierto, repetible)
// NPC: Tropezones  |  questId: "Tropezones"
// requiredOutfit: Any  |  isRepeatable = true
// Plantilla: LADRONES.  Apuesta: 8 dineros + 2 cartas raras.
// ============================================================


== Tropezones ==
Tropezones: ¿La bolsa o la vida?
Caterina: Os daré todo lo que llevo si me ganáis a una partida de Naipes. ¿Jugáis o sois acaso un cagalindes?
Tropezones: Nadie me llama cobarde. Mis condiciones son 8 dineros y 2 cartas raras. ¿Estáis de acuerdo?
Caterina: Sí, barajad vuestro mazo.
~ StartQuest("Tropezones")
-> END

= victoria
Tropezones: Maldita sea mi suerte. Tomad, marchaos antes de que cambie de idea.
~ FinishQuest("Tropezones")
-> END

= derrota
Caterina: Aquí tenéis. Volveremos a vernos en el camino.
~ FinishQuest("Tropezones")
-> END
