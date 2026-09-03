// ============================================================
// SECUNDARIO — GUARDA DEL CASTILLO DEL MARQUÉS (M6, farming, repetible)
// NPC: Pero del Peral  |  questId: "PeroDelPeral"
// requiredOutfit: Josep  |  isRepeatable = true
// Plantilla: GENÉRICO.  Apuesta: 6 dineros + 2 cartas raras.
// ============================================================


== PeroDelPeral ==
Caterina: Busco contrincante para una partida de Naipes. ¿Os animáis?
Pero del Peral: Si nos jugamos 6 dineros y 2 cartas raras... Hecho.
Caterina: Sí, juguemos.
~ StartQuest("PeroDelPeral")
-> END

= victoria
Pero del Peral: Aquí tenéis las monedas y las cartas. Mala suerte la mía.
~ FinishQuest("PeroDelPeral")
-> END

= derrota
Caterina: Aquí tenéis. En otra ocasión.
~ FinishQuest("PeroDelPeral")
-> END
