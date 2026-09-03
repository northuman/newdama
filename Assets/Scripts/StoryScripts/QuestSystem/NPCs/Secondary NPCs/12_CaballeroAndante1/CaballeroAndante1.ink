// ============================================================
// SECUNDARIO — CABALLERO ANDANTE (mundo abierto, repetible)
// NPC: Caballero Andante 1  |  questId: "CaballeroAndante1"
// requiredOutfit: Any  |  isRepeatable = true
// Plantilla: CABALLEROS ANDANTES.  Apuesta: 6 dineros + 1 carta rara.
// ============================================================


== CaballeroAndante1 ==
Caterina: Buen día. ¿Quién sois?
Caballero: Soy el caballero Tirante el Bravo. Si queréis pasar por aquí, tendréis que demostrarme en un duelo que lo merecéis.
Caterina: ¿Un duelo con armas? ¿No podría ser con cartas?
Caballero: ¿Sabéis jugar a los Naipes?
Caterina: ¿Quién no sabe jugar a los Naipes en Levante?
Caballero: Mis condiciones son 6 dineros y 1 carta rara. ¿Estáis de acuerdo?
Caterina: Sí, juguemos.
~ StartQuest("CaballeroAndante1")
-> END

= victoria
Caballero: Sois digno de paso. Tomad vuestro premio y seguid vuestro camino con honor.
~ FinishQuest("CaballeroAndante1")
-> END

= derrota
Caterina: Quizá en otra ocasión. Me voy por donde he venido. Con Dios.
~ FinishQuest("CaballeroAndante1")
-> END
