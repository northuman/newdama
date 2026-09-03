// ============================================================
// SECUNDARIO — CAMPESINO / ARTESANO / ALDEANO (mundo abierto, repetible)
// NPC: Aldeano  |  questId: "Aldeano"
// requiredOutfit: Any  |  isRepeatable = true
// Plantilla: CAMPESINOS – ARTESANOS. Apuesta configurable.
// ============================================================


== Aldeano ==
Caterina: Buen día. ¿Os apetece jugar una partida de Naipes?
Aldeano: No hay nada que me guste más después de una larga jornada de trabajo. Tenéis pinta de robaperas, pero acepto vuestra propuesta. Mis condiciones son 2 dineros y 1 carta poco común. ¿Estáis de acuerdo?
Caterina: Sí, juguemos.
~ StartQuest("Aldeano")
-> END

= victoria
Aldeano: Vaya, qué mala pata. Aquí tenéis lo acordado.
~ FinishQuest("Aldeano")
-> END

= derrota
Caterina: Quizá en otra ocasión.
~ FinishQuest("Aldeano")
-> END
