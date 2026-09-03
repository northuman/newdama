// ============================================================
// SECUNDARIO — ERMITAÑO / FRAILE / MONJA / CURA (mundo abierto, repetible)
// NPC: Ermitaño Extra  |  questId: "ErmitanoExtra"
// requiredOutfit: Any  |  isRepeatable = true
// Plantilla: ERMITAÑOS – FRAILES – MONJAS - CURAS. Apuesta configurable.
// ============================================================


== ErmitanoExtra ==
Caterina: Buen día. Dios esté con vos.
Religioso: ¿Qué os trae por aquí?
Caterina: Estoy buscando rivales para jugar a los Naipes.
Religioso: Perfecto. Tengo un rato libre. Acepto jugar. Mis condiciones son 6 dineros y 2 cartas raras. ¿Estáis de acuerdo?
Caterina: Sí, juguemos.
~ StartQuest("ErmitanoExtra")
-> END

= victoria
Religioso: Que el Altísimo perdone mi afición al juego. Aquí tenéis las monedas y las cartas.
~ FinishQuest("ErmitanoExtra")
-> END

= derrota
Caterina: Quizá en otra ocasión.
~ FinishQuest("ErmitanoExtra")
-> END
