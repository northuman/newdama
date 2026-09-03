// ============================================================
// MISIÓN 1 — ADIÓS A LA VIDA MONÁSTICA
// NPC: Madre Superiora  |  questId: "MadreSuperiora"
// loseIsGameOver = true  (perder = Game Over absoluto)
// requiredOutfit: Caterina (única, todavía no hay disfraz)
// ============================================================


== MadreSuperiora ==
Caterina: Madre superiora, querida tía, no soporto más la clausura del monasterio. Amo a Dios, pero no sé si estoy preparada para dedicar mi vida a la oración y al trabajo. Mi relación con las novicias no es buena y sé que es por mi culpa, porque los muros de este santo edificio me exasperan. Necesito volar y aprender a vivir. Mi sueño es conocer otros lugares y poder vivir mi propia vida.
Caterina: Estoy muy agradecida por todo lo que me habéis enseñado. He aprendido a hablar con Dios, a leer, a escribir, a realizar las tareas del hogar. Pero ahora, necesito conocer el mundo y antes o después volaré.
Madre Superiora: Caterina, tu madre me confió tu educación. No hay mejor futuro que dedicar la vida al Altísimo y a rezar por los demás, por todos los que viven en este mundo marcado por el pecado original. Tu comportamiento está siendo muy indecoroso y ya no sé cómo disciplinarte. Creo que tu decisión de escapar es una manifestación de inmadurez y de falta de inteligencia.
Caterina: Si pensáis que no estoy en mis cabales o que mi cabeza es la de una niña caprichosa, permitidme demostraros que os equivocáis.
Madre Superiora: ¿Cómo?
Caterina: Juguemos una partida de Naipes y que Dios nos perdone. Si os gano, me dejaréis escapar; me daréis unos dineros y una baraja, y con ella me ganaré la vida.
Madre Superiora: Os sobrevaloráis. Acepto, pero solo para daros una nueva lección.
~ StartCombat("MadreSuperiora.resolucion")
-> DONE

// ── Stitch de resolución: bifurca según combat_won (fijado por DialogueManager)
= resolucion
{combat_won:
    -> victoria
-else:
    -> derrota
}

// ────────────────────────────────────────────────────────────
// RAMA VICTORIA (Caterina gana el combate)
// ────────────────────────────────────────────────────────────
= victoria
Caterina: Madre Superiora, cumplid con vuestra palabra. Dejadme elegir una baraja y escapar del monasterio. Con 16 años, debo ser responsable de mi vida y de mi futuro, y mi sueño siempre ha sido embarcarme en un navío y viajar a los Reinos de Indias.
Madre Superiora: Caterina, tenéis la cabeza llena de pájaros, pero la palabra dada es palabra sagrada. Elegid la baraja y coged algo de comida. Aquí tenéis 1 sueldo; no os puedo dar más. Que Dios os guíe y os acompañe en esta locura que vais a iniciar. Escribidme cuando lleguéis al Nuevo Mundo.
Caterina: Gracias, Madre Superiora. Iré a la ciudad de Levante. Desde su muelle salen barcos hacia Sevilla. Intentaré conseguir un pasaje.
Madre Superiora: Con Dios.
// Abre la puerta de la muralla del monasterio para que Caterina pueda salir.
~ RaiseWorldEvent("abrir_puerta_monasterio")
~ FinishQuest("MadreSuperiora", true)
-> END

// ────────────────────────────────────────────────────────────
// RAMA DERROTA (la Madre Superiora gana) -> GAME OVER
// ────────────────────────────────────────────────────────────
= derrota
Madre Superiora: Os lo dije, criatura. Volved a vuestra celda y rezad por vuestra soberbia.
~ FinishQuest("MadreSuperiora", false)
-> END
