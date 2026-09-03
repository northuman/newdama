// ============================================================
// SECUNDARIO — GUARDA DEL CASTILLO DEL MARQUÉS (M6, farming, repetible)
// NPC: Lope de Aguas Muertas  |  questId: "LopeDeAguasMuertas"
// requiredOutfit: Josep  |  isRepeatable = true
// Plantilla: GENÉRICO.  Apuesta: 4 dineros + 1 carta rara.
// ============================================================


== LopeDeAguasMuertas ==
Caterina: Busco contrincante para una partida de Naipes. ¿Os animáis?
Lope de Aguas Muertas: Si nos jugamos 4 dineros y 1 carta rara... Hecho.
Caterina: Sí, juguemos.
~ StartQuest("LopeDeAguasMuertas")
-> END

= victoria
Lope de Aguas Muertas: Aquí tenéis las monedas y la carta. Mala suerte la mía.
~ FinishQuest("LopeDeAguasMuertas")
-> END

= derrota
Caterina: Aquí tenéis. En otra ocasión.
~ FinishQuest("LopeDeAguasMuertas")
-> END
