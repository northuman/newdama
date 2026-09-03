// ============================================================
// SECUNDARIO — TAHÚR DE LA VENTA DEL DESTRIPAT (M4, repetible)
// NPC: Antoni Tres Dientes  |  questId: "AntoniTresDientes"
// requiredOutfit: Josep  |  isRepeatable = true
// Plantilla: TAHÚRES.  Apuesta: 3 dineros + 1 carta rara.
// ============================================================


== AntoniTresDientes ==
Caterina: Buen día, tahúr. ¿Os apetece jugar una partida de Naipes?
Antoni Tres Dientes: Sin duda. Tenéis pinta de zampalimosnas, pero si ponéis dinero y cartas, no hay problema. Mis condiciones son 3 dineros y 1 carta rara. ¿Estáis de acuerdo?
Caterina: Sí, juguemos.
~ StartQuest("AntoniTresDientes")
-> END

= victoria
Antoni Tres Dientes: Tenéis suerte de fullero. Aquí están las monedas y las cartas.
~ FinishQuest("AntoniTresDientes")
-> END

= derrota
Caterina: Aquí tenéis. Quizá en otra ocasión.
~ FinishQuest("AntoniTresDientes")
-> END
