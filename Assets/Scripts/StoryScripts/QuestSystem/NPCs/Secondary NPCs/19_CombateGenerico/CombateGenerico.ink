// ============================================================
// SECUNDARIO — COMBATE GENÉRICO (plantilla reutilizable, repetible)
// NPC: Rival Genérico  |  questId: "CombateGenerico"
// requiredOutfit: Any  |  isRepeatable = true
// Plantilla: GENÉRICO (TEXTOS PARA COMBATES SUELTOS).
// Úsese como base para rivales sin diálogo propio. Ajustar apuesta por instancia.
// ============================================================


== CombateGenerico ==
Caterina: Buen día. ¿Os apetece jugar una partida de Naipes?
Rival: Sí, por supuesto. Mis condiciones son 4 dineros y 1 carta rara. ¿Estáis de acuerdo?
Caterina: Sí, juguemos.
~ StartQuest("CombateGenerico")
-> END

= victoria
Rival: Habéis ganado en buena lid. Aquí tenéis las monedas y las cartas.
~ FinishQuest("CombateGenerico")
-> END

= derrota
Caterina: Quizá en otra ocasión.
~ FinishQuest("CombateGenerico")
-> END
