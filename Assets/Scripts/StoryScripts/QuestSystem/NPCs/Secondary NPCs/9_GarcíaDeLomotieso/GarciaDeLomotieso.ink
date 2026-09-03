// ============================================================
// SECUNDARIO — GUARDA DEL CASTILLO DEL MARQUÉS (M6, farming, repetible)
// NPC: García de Lomotieso  |  questId: "GarciaDeLomotieso"
// requiredOutfit: Josep  |  isRepeatable = true
// Plantilla: GENÉRICO.  Apuesta: 3 dineros + 1 carta rara.
// ============================================================


== GarciaDeLomotieso ==
Caterina: Busco contrincante para una partida de Naipes. ¿Os animáis?
García de Lomotieso: Si nos jugamos 3 dineros y 1 carta rara... Hecho.
Caterina: Sí, juguemos.
~ StartQuest("GarciaDeLomotieso")
-> END

= victoria
García de Lomotieso: Aquí tenéis las monedas y la carta. Mala suerte la mía.
~ FinishQuest("GarciaDeLomotieso")
-> END

= derrota
Caterina: Aquí tenéis. En otra ocasión.
~ FinishQuest("GarciaDeLomotieso")
-> END
