// ============================================================
// MISIÓN 4 — GANANDO DINERO Y CARTAS EN LA VENTA DEL DESTRIPAT
// NPC: Jaume el Destripat (ventero)  |  questId: "Destripat"
// SO: requiredOutfit = Josep ; levelUp = true ; goldReward = 4 ; loseGoldPenalty = 4
//     isRepeatable = true
// Si va de mujer -> rama = rechazo.
// El farmeo de tahúres (Pere, Antoni, Batiste, Joan, Pepón) vive en Secondary NPCs
//   (ficheros aparte, fase posterior).
// ============================================================

VAR destripat_vencido = false
VAR destripat_rechazo_mujer = false

== Destripat ==
{ required_outfit != "Any" && outfit != required_outfit:
    { destripat_rechazo_mujer: -> rechazo_repetido | -> rechazo }
}
{ destripat_vencido: -> revancha }
{ not HasGold(4): -> sin_dinero }
Caterina: Ventero, he venido a beber buen vino y a jugar a los Naipes. ¿Hay partidas hoy?
Destripat: ¿Os he visto alguna vez por aquí? Me suena vuestra cara.
Caterina: No creo. Soy nuevo por estos lares. Vengo del norte. El viejo molinero me dijo que aquí hay buenas partidas de Naipes y mucho vino de la zona. Y no me pude resistir.
Destripat: Sois bienvenido. ¿Cuál es vuestro nombre?
Caterina: Mi nombre es Josep Ferrández de Mesa, soy soldado de fortuna y vengo del Maestrat.
Destripat: Id a aquel rincón y esperadme. Hoy vais a tener el honor de jugar conmigo a los Naipes. Os llevo vino.
Caterina: Gracias. Os espero.
Destripat: Aquí tenéis. ¿Os parece que nos juguemos 4 dineros y 2 cartas raras? ¿Es mucho para vos?
Caterina: No, en absoluto; acepto.
~ StartCombat("Destripat.resolucion")
-> DONE

= resolucion
{ combat_won: -> victoria | -> derrota }

// ------------------------------------------------------------
// RAMA VICTORIA -> info del castillo del marqués de Dos Aguas
// ------------------------------------------------------------
= victoria
Destripat: Habéis tenido mucha suerte, pero un trato es un trato. Aquí tenéis el dinero y las cartas. ¿En qué dirección vais?
Caterina: Busco señor al cual servir con mis brazos y mis armas.
Destripat: Al noroeste, siguiendo el curso del río está el castillo del marqués de Dos Aguas. Es un señor principal y os puede reclutar para su hueste.
Caterina: Gracias por la información. Ahora debo partir. Un placer, ventero. Con Dios.
~ destripat_vencido = true
~ FinishQuest("Destripat", true)
-> END

// ------------------------------------------------------------
// RAMA DERROTA -> el SO aplica -4 dineros; sigue siendo bienvenido
// ------------------------------------------------------------
= derrota
Destripat: No sois tan bueno como esperaba. Venid otro día y os presentaré a otros jugadores. Siempre tendréis un lugar en esta mesa, cuando vengáis a mi venta.
Caterina: Gracias, hasta la próxima.
~ FinishQuest("Destripat", false)
-> END

// ============================================================
// VARIANTE DE RECHAZO — Caterina entra VESTIDA DE MUJER
// ============================================================
= rechazo
Caterina: Ventero, ponedme algo para comer y vino.
Destripat: ¿Viajáis sola? ¿Dónde están vuestro marido o vuestro padre?
Caterina: No estoy casada y mi padre está impedido y me ha enviado a hacer algunos encargos. Tengo dinero y hambre. ¿Me podéis servir?
Destripat: Aquí tenéis, guiso de caracoles y vino de la casa.
Caterina: Están buenos estos caracoles, ventero. El vino, no tanto. Ha llegado a mis oídos que a esta venta vienen los mejores jugadores de Naipes de la región. ¿Es así?
Destripat: Y tanto que es así. ¿Por qué preguntáis? ¿Vuestro padre sabe jugar?
Caterina: No, soy yo la que sabe jugar.
Destripat: Vos, ¡ja, ja, ja! ¿Cómo osáis decir eso? Por todos es sabido que las mujeres no tenéis inteligencia y que vuestro carácter emocional os hace perder siempre en los juegos, sean cuales sean.
Caterina: Eso son tonterías.
Destripat: ¿Me estáis llamando tonto? Acabad pronto la comida y la bebida. No os quiero ver por aquí. Son 6 dineros.
Caterina: Aquí tenéis. Dios se apiade de vuestra cerrazón.
~ destripat_rechazo_mujer = true
~ SpendGold(6)
-> END

= rechazo_repetido
Destripat: Aquí no sois bien recibida. Marchaos.
-> END

= sin_dinero
Destripat: Aquí no se juega fiado. Volved cuando tengáis cuatro dineros.
Caterina: Volveré. Con Dios.
-> END

// ------------------------------------------------------------
// REMATCH (isRepeatable): 4 dineros + 2 cartas raras
// ------------------------------------------------------------
= revancha
{ not HasGold(4): -> sin_dinero }
Caterina: Ventero, ¿hay partida hoy?
Destripat: Para vos, siempre. Las condiciones de siempre: 4 dineros y 2 cartas raras.
Caterina: Aceptado. Juguemos.
~ StartCombat("Destripat.resolucion_revancha")
-> DONE

= resolucion_revancha
{ combat_won: -> revancha_victoria | -> revancha_derrota }

= revancha_victoria
Destripat: Otra vez la fortuna de vuestra parte. Aquí tenéis. Volved cuando gustéis.
~ FinishQuest("Destripat", true)
-> END

= revancha_derrota
Caterina: Aquí tenéis lo apostado. Hasta la próxima.
~ FinishQuest("Destripat", false)
-> END
