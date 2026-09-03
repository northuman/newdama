// ============================================================
// SECUNDARIO — TAHÚR DE LA VENTA DEL DESTRIPAT (M4, repetible)
// NPC: Pere el Tuerto  |  questId: "PereElTuerto"
// requiredOutfit: Josep  |  isRepeatable = true
// Plantilla: TAHÚRES.  Apuesta: 2 dineros + 2 cartas poco comunes.
// ============================================================


== PereElTuerto ==
Caterina: Buen día, tahúr. ¿Os apetece jugar una partida de Naipes?
Pere el Tuerto: Sin duda. Tenéis pinta de zampalimosnas, pero si ponéis dinero y cartas, no hay problema. Mis condiciones son 2 dineros y 2 cartas poco comunes. ¿Estáis de acuerdo?
Caterina: Sí, juguemos.
~ StartQuest("PereElTuerto")
-> END

= victoria
Pere el Tuerto: Tenéis suerte de fullero. Aquí están las monedas y las cartas.
~ FinishQuest("PereElTuerto")
-> END

= derrota
Caterina: Aquí tenéis. Quizá en otra ocasión.
~ FinishQuest("PereElTuerto")
-> END
