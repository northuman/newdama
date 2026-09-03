// ============================================================
// SECUNDARIO — TAHÚR DE LA VENTA DEL DESTRIPAT (M4, repetible)
// NPC: Joan Medio Huevo  |  questId: "JoanMedioHuevo"
// requiredOutfit: Josep  |  isRepeatable = true
// Plantilla: TAHÚRES.  Apuesta: 6 dineros + 2 cartas raras.
// ============================================================


== JoanMedioHuevo ==
Caterina: Buen día, tahúr. ¿Os apetece jugar una partida de Naipes?
Joan Medio Huevo: Sin duda. Tenéis pinta de zampalimosnas, pero si ponéis dinero y cartas, no hay problema. Mis condiciones son 6 dineros y 2 cartas raras. ¿Estáis de acuerdo?
Caterina: Sí, juguemos.
~ StartQuest("JoanMedioHuevo")
-> END

= victoria
Joan Medio Huevo: Tenéis suerte de fullero. Aquí están las monedas y las cartas.
~ FinishQuest("JoanMedioHuevo")
-> END

= derrota
Caterina: Aquí tenéis. Quizá en otra ocasión.
~ FinishQuest("JoanMedioHuevo")
-> END
