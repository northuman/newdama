// ============================================================
// SECUNDARIO — CABALLERO ANDANTE (mundo abierto, repetible)
// NPC: Caballero Andante 4  |  questId: "CaballeroAndante4"
// requiredOutfit: Any  |  isRepeatable = true
// Plantilla: CABALLEROS ANDANTES.  Apuesta: 12 dineros + 2 cartas raras.
// ============================================================


== CaballeroAndante4 ==
Caterina: Buen día. ¿Quién sois?
Caballero: Soy el caballero Amadís del Valle. Si queréis pasar por aquí, tendréis que demostrarme en un duelo que lo merecéis.
Caterina: ¿Un duelo con armas? ¿No podría ser con cartas?
Caballero: ¿Sabéis jugar a los Naipes?
Caterina: ¿Quién no sabe jugar a los Naipes en Levante?
Caballero: Mis condiciones son 12 dineros y 2 cartas raras. ¿Estáis de acuerdo?
Caterina: Sí, juguemos.
~ StartQuest("CaballeroAndante4")
-> END

= victoria
Caballero: Sois digno de paso. Tomad vuestro premio y seguid vuestro camino con honor.
~ FinishQuest("CaballeroAndante4")
-> END

= derrota
Caterina: Quizá en otra ocasión. Me voy por donde he venido. Con Dios.
~ FinishQuest("CaballeroAndante4")
-> END
