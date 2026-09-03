// ============================================================
// SECUNDARIO — GUARDA DE TORRE DE VIGILANCIA (mundo abierto, repetible)
// NPC: Guarda de Torre  |  questId: "GuardaTorre"
// requiredOutfit: Any  |  isRepeatable = true
// Plantilla: GENÉRICO.  Apuesta: configurable (xx dineros + xx cartas).
// Plantilla reutilizable para las distintas torres del mapa.
// ============================================================


== GuardaTorre ==
Caterina: Buen día. ¿Os apetece jugar una partida de Naipes?
Guarda de Torre: Sí, por supuesto. Mis condiciones son 4 dineros y 1 carta rara. ¿Estáis de acuerdo?
Caterina: Sí, juguemos.
~ StartQuest("GuardaTorre")
-> END

= victoria
Guarda de Torre: Aquí tenéis las monedas y la carta. Buen viaje.
~ FinishQuest("GuardaTorre")
-> END

= derrota
Caterina: Quizá en otra ocasión.
~ FinishQuest("GuardaTorre")
-> END
