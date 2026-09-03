// ============================================================
// SECUNDARIO — CABALLERO ANDANTE (mundo abierto, repetible)
// NPC: Caballero Andante 3  |  questId: "CaballeroAndante3"
// requiredOutfit: Any  |  isRepeatable = true
// Plantilla: CABALLEROS ANDANTES.  Apuesta: 10 dineros + 2 cartas raras.
// ============================================================


== CaballeroAndante3 ==
Caterina: Buen día. ¿Quién sois?
Caballero: Soy el caballero Roldán de la Peña. Si queréis pasar por aquí, tendréis que demostrarme en un duelo que lo merecéis.
Caterina: ¿Un duelo con armas? ¿No podría ser con cartas?
Caballero: ¿Sabéis jugar a los Naipes?
Caterina: ¿Quién no sabe jugar a los Naipes en Levante?
Caballero: Mis condiciones son 10 dineros y 2 cartas raras. ¿Estáis de acuerdo?
Caterina: Sí, juguemos.
~ StartQuest("CaballeroAndante3")
-> END

= victoria
Caballero: Sois digno de paso. Tomad vuestro premio y seguid vuestro camino con honor.
~ FinishQuest("CaballeroAndante3")
-> END

= derrota
Caterina: Quizá en otra ocasión. Me voy por donde he venido. Con Dios.
~ FinishQuest("CaballeroAndante3")
-> END
