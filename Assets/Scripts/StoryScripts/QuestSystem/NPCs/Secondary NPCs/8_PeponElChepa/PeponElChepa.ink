// ============================================================
// SECUNDARIO — TAHÚR DE LA VENTA DEL DESTRIPAT (M4, repetible)
// NPC: Pepón el Chepa  |  questId: "PeponElChepa"
// requiredOutfit: Josep  |  isRepeatable = true
// Plantilla: TAHÚRES.  Apuesta: 8 dineros + 1 carta mítica.
// ============================================================


== PeponElChepa ==
Caterina: Buen día, tahúr. ¿Os apetece jugar una partida de Naipes?
Pepón el Chepa: Sin duda. Tenéis pinta de zampalimosnas, pero si ponéis dinero y cartas, no hay problema. Mis condiciones son 8 dineros y 1 carta mítica. ¿Estáis de acuerdo?
Caterina: Sí, juguemos.
~ StartQuest("PeponElChepa")
-> END

= victoria
Pepón el Chepa: Tenéis suerte de fullero. Aquí están las monedas y las cartas.
~ FinishQuest("PeponElChepa")
-> END

= derrota
Caterina: Aquí tenéis. Quizá en otra ocasión.
~ FinishQuest("PeponElChepa")
-> END
