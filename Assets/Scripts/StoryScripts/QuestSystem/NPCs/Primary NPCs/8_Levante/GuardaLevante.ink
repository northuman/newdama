// ============================================================
// MISIÓN 8 — ENTRANDO EN LEVANTE POR LA PUERTA NORTE
// NPC: Germán del Puñal Romo (guarda de la puerta norte)  |  questId: "GuardaLevante"
// requiredOutfit: Josep + requiredItems: "salvoconducto"
//   - Vestida de mujer -> knot "GuardaLevante_rechazo" (acoso, bloqueo permanente)
//   - Sin salvoconducto -> knot "GuardaLevante_sinSalvoconducto"
// Apuesta: si gana Josep -> paso franco + 1 carta rara + indicaciones casa D. Martín
//          si gana el guarda -> Josep paga 10 dineros + 1 carta rara
// isRepeatable = true (rematch: 8 dineros + 2 cartas raras)
// ============================================================


== GuardaLevante ==
Guarda: Deteneos.
Caterina: Buen día. Quiero entrar en la ciudad. Vengo por orden del marqués de Dos Aguas. Aquí tengo su salvoconducto para acceder a Levante por la puerta norte.
Guarda: Mostrádmelo. Ummm. Sí, es el sello del señor marqués. Pero a Levante no se entra sin pagar las tasas.
Caterina: ¿No os vale la carta del marqués?
Guarda: No, habéis de pagar las tasas de la Guardia.
Caterina: ¿Y cuáles son estas tasas?
Guarda: 10 dineros.
Caterina: Veo que tenéis Naipes ahí.
Guarda: ¿Quién no? Este juego se ha extendido por la ciudad más que una pestilencia.
Caterina: ¿Qué os parece que nos juguemos el acceso a los Naipes? Si me ganáis, os daré los 10 dineros y una carta rara. Si os gano, me franquearéis el paso y me daréis una carta rara. ¿Aceptáis el trato?
Guarda: Acepto. No soy una gallina.
{ HasGold(10): -> jugar | -> sinDinero }

= jugar
~ StartCombat("GuardaLevante.resolucion")
-> DONE

= resolucion
{ combat_won: -> victoria | -> derrota }

= sinDinero
Caterina: Creo que me han robado la bolsa. Volveré con los 10 dineros. Esperadme.
Guarda: Con Dios.
-> END

// ------------------------------------------------------------
// RAMA VICTORIA -> paso franco + indicaciones a casa de D. Martín
// ------------------------------------------------------------
= victoria
Guarda: Jugáis como un fullero de taberna. Tomad vuestra carta rara, embrollón. Y pasad a la ciudad.
Caterina: Busco la casa de D. Martín de la Santa Cruz, un arrendador de impuestos.
Guarda: Ah, sí. Aquí todos le conocen. Es una persona muy odiada por el pueblo. Se ha enriquecido a base de recaudar dinero para la corona. Id recto por esta calle principal, hasta que no podáis caminar más. A vuestra derecha, veréis el palacio de D. Martín de la Santa Cruz.
~ RaiseWorldEvent("abrir_puerta_levante")
~ SetFlag("levante_abierto", true)
~ FinishQuest("GuardaLevante", true)
-> END

// ------------------------------------------------------------
// RAMA DERROTA -> Josep paga 10 dineros + 1 carta rara; reintento disponible
// ------------------------------------------------------------
= derrota
Guarda: Dadme los 10 dineros y la carta rara. Sois un catacaldos. Jugando así, podéis volver cuando queráis.
Caterina: Volveré. Con Dios.
~ FinishQuest("GuardaLevante", false)
-> END

= reintento
Caterina: Tengo el dinero preparado. ¿Jugamos?
Guarda: Siempre es un placer jugar con vos.
~ StartCombat("GuardaLevante.resolucion")
-> DONE

// ------------------------------------------------------------
// REMATCH (isRepeatable): 8 dineros + 2 cartas raras
// ------------------------------------------------------------
= revancha
Guarda: ¿Quién va?
Caterina: Un aficionado a los Naipes que quiere jugar con vos.
Guarda: De acuerdo. Nos jugaremos 8 dineros y 2 cartas raras. ¿Aceptáis las condiciones?
Caterina: Sin duda.
~ StartCombat("GuardaLevante.resolucion_revancha")
-> DONE

= resolucion_revancha
{ combat_won: -> revancha_victoria | -> revancha_derrota }

= revancha_victoria
Guarda: Aquí tenéis las monedas y las cartas. Salid pitando de aquí, que tengo mal perder.
Caterina: Con Dios.
~ FinishQuest("GuardaLevante", true)
-> END

= revancha_derrota
Caterina: Aquí tenéis las monedas y las cartas. Que tengáis buen día.
Guarda: Ahora será mejor, gracias a vuestra donación, mequetrefe.
~ FinishQuest("GuardaLevante", false)
-> END

// ============================================================
// VARIANTE DE RECHAZO — Caterina llega VESTIDA DE MUJER
// (tras esto, deshabilitar volver a hablar con esta puerta de mujer)
// ============================================================
== GuardaLevante_rechazo ==
Guarda: Deteneos.
Caterina: Buen día. Tengo un salvoconducto del marqués de Dos Aguas para entrar en la ciudad.
Guarda: Dejadme verlo. Ummm. Sí, parece que está en regla. Es el sello del marqués. Pero..., si queréis entrar, tendréis que ser generosa con nosotros.
Caterina: ¿Cómo de generosa?
Guarda: Solo tenéis que mostrarnos vuestra gratitud, detrás de la taberna.
Caterina: Buahhh. En la vida. No soy ese tipo de mujer. De hecho, ninguna mujer debería estar dispuesta a ser generosa así con vosotros.
Guarda: Quitaos de mi vista. Por aquí no entráis a Levante. Aunque siempre podéis cambiar de opinión.
Caterina: Ni muerta.
-> END

// ============================================================
// VARIANTE — intentan entrar SIN salvoconducto (Caterina o Josep)
// ============================================================
== GuardaLevante_sinSalvoconducto ==
Guarda: Alto el paso.
Caterina: Buen día. Quiero entrar en la ciudad.
Guarda: ¿Tenéis licencia o salvoconducto?
Caterina: No.
Guarda: Entonces no entráis. Con Dios.
Caterina: Con Dios.
-> END
