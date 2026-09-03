// ============================================================
// SECUNDARIO — TAHÚR DE LA VENTA DEL DESTRIPAT (M4, repetible)
// NPC: Batiste Cara Rata  |  questId: "BatisteCaraRata"
// requiredOutfit: Josep  |  isRepeatable = true
// Plantilla: TAHÚRES.  Apuesta: 4 dineros + 1 carta rara.
// ============================================================


== BatisteCaraRata ==
Caterina: Buen día, tahúr. ¿Os apetece jugar una partida de Naipes?
Batiste Cara Rata: Sin duda. Tenéis pinta de zampalimosnas, pero si ponéis dinero y cartas, no hay problema. Mis condiciones son 4 dineros y 1 carta rara. ¿Estáis de acuerdo?
Caterina: Sí, juguemos.
~ StartQuest("BatisteCaraRata")
-> END

= victoria
Batiste Cara Rata: Tenéis suerte de fullero. Aquí están las monedas y las cartas.
~ FinishQuest("BatisteCaraRata")
-> END

= derrota
Caterina: Aquí tenéis. Quizá en otra ocasión.
~ FinishQuest("BatisteCaraRata")
-> END
