// ============================================================
// MISIÓN 14 — TORNEO CLASIFICATORIO DEL MERCADO
// NPC: Organizador del Mercado  |  questId: "OrganizadorMercado"
// requiredOutfit: Josep + requiredItems: "recomendacion_martin"
//   - Vestida de mujer -> knot "OrganizadorMercado_rechazo"
// Inscripción: 2 sueldos. Torneo de 3 partidas (recompensas escalonadas).
//   Ganar las 3 -> 3 sueldos y 3 dineros + 9 cartas raras + invitación al torneo
//   de la Plaza Mayor (M15). Habilita el farming con los NPCs opcionales.
// ============================================================


== OrganizadorMercado ==
Caterina: Buen día.
Organizador: Buen día.
Caterina: Quiero jugar el torneo de Naipes.
Organizador: ¿Tenéis alguna carta de recomendación?
Caterina: Sí, de D. Martín de la Santa Cruz. Esta es.
Organizador: A ver. Sí, está todo en regla. La inscripción cuesta 2 sueldos. ¿Los tenéis?
* [Tengo el dinero] -> conDinero
+ [No tengo el dinero] -> sinDinero

= sinDinero
Caterina: Ahora mismo no, pero lo traeré en breve. Con Dios.
Organizador: Con Dios.
-> END

= conDinero
Caterina: Sí, aquí están las monedas.
Organizador: Perfecto. Estas son las instrucciones del torneo. El torneo consta de 3 partidas. Si perdéis la primera, os iréis de vacío. Si ganáis la primera, os llevaréis 9 dineros y 2 cartas raras. Si ganáis la segunda, os llevaréis 21 dineros y 5 nuevas cartas raras. Y si ganáis la tercera, conseguiréis otros 39 dineros, 9 cartas raras y la carta de recomendación para jugar el torneo final de la Plaza Mayor.
Caterina: De acuerdo, estoy preparado.
~ StartCombat("OrganizadorMercado.resolucion")
-> DONE

= resolucion
{ combat_won: -> victoria | -> derrota }

// ------------------------------------------------------------
// RAMA VICTORIA (gana los 3 combates) -> recomendación Plaza Mayor + info farming
// ------------------------------------------------------------
= victoria
Organizador: ¡Enhorabuena! Has conseguido 3 sueldos y 3 dineros, 9 cartas raras y la invitación para el torneo de la Plaza Mayor.
Caterina: Gracias.
Organizador: El torneo de la Plaza Mayor es el más importante de la ciudad. La inscripción cuesta 7 sueldos y habréis de tener un mazo muy fuerte. Si queréis conseguir cartas míticas blancas, os recomiendo que juguéis con el chantre de la Catedral, fray Tomás del Canto Gregoriano. Si queréis cartas míticas rojas, retad al catedrático de la Universidad, el doctor Enrique de Vitoria. Si buscáis cartas míticas verdes, jugad con el jurado de la ciudad, D. Ferran de la Clau. Si necesitáis cartas negras, jugad en el muelle con el capitán genovés Giovanni Tornabuoni. A más ver.
Caterina: Con Dios.
~ FinishQuest("OrganizadorMercado", true)
-> END

// ------------------------------------------------------------
// RAMA DERROTA (no gana los 3 combates) -> recompensa parcial; puede reintentar
// ------------------------------------------------------------
= derrota
Organizador: Has conseguido tus dineros y tus cartas raras. Vuelve cuando quieras a intentarlo de nuevo.
~ FinishQuest("OrganizadorMercado", false)
-> END

// ============================================================
// VARIANTE DE RECHAZO — Caterina llega VESTIDA DE MUJER
// ============================================================
== OrganizadorMercado_rechazo ==
Caterina: Buen día.
Organizador: Buen día.
Caterina: Quiero jugar el torneo de Naipes.
Organizador: Imposible. Es solo para hombres. Vete a tu casa a coser o a rezar y no me hagas perder el tiempo.
Caterina: Con Dios.
-> END
