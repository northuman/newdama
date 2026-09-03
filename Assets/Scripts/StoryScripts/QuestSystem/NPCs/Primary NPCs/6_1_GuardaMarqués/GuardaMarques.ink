// ============================================================
// MISIÓN 6.1 — PUERTA DEL CASTILLO DEL MARQUÉS DE DOS AGUAS
// NPC: Guarda del Marqués | questId: "GuardaMarques"
// Requiere nivel 6, haber completado Ermitano, outfit Josep y cantarida.
// Es un paso de acceso sin combate ni recompensas.
// ============================================================

== GuardaMarques ==
{ required_outfit != "Any" && outfit != required_outfit: -> rechazo }
{ not HasItem("cantarida"): -> sin_cantarida }
Guarda: ¡Alto ahí! ¿Quién va?
Caterina: Soy Josep Ferrández de Mesa y estoy al servicio del señor marqués. Vengo de la ermita de la Santa Cruz, realizando un encargo de fray Lucio del Santo Calvario, con una entrega para el señor.
Guarda: De acuerdo, pasad al patio de caballos y esperad a que el marqués baje de sus aposentos.
Caterina: Gracias, compañero.
// El listener de la puerta del Marqués usa este identificador exacto.
~ RaiseWorldEvent("abrir_puerta_marques")
~ FinishQuest("GuardaMarques", true)
-> END

= rechazo
Guarda: ¡Alto ahí! ¿Quién va?
Caterina: Mi nombre es Caterina de Eraso. Vengo a ver al marqués de parte de fray Lucio del Santo Calvario, el monje que vive en la ermita de la Santa Cruz.
Guarda: ¿Queréis ver al marqués? Jajaja. Como si cualquier mujerzuela de los caminos pudiese tener una audiencia con mi señor. ¿Me tomáis por un mentecato?
Caterina: El marqués espera algo importante del ermitaño. No me hagáis esperar.
Guarda: ¿Y qué me dais a mí por haceros el favor? Un favor se paga con un favor. ¿Sois doncella?
Caterina: Por supuesto que sí. Vengo del Monasterio de la Merced. Mi tía es la Madre Superiora.
Guarda: Así que sois doncella. Pues ya sabéis. Si queréis ver al marqués, antes deberéis dejar de serlo conmigo.
Caterina: Sois un desvergonzado. Vuestro señor os lo hará pagar. Adiós.
-> END

= sin_cantarida
Guarda: Decís venir de parte de fray Lucio, pero no traéis el encargo que espera mi señor.
Caterina: Volveré cuando lo tenga. Con Dios.
-> END
