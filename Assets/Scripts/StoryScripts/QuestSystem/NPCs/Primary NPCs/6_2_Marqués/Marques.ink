// ============================================================
// MISIÓN 6.2 — CON EL MARQUÉS DE DOS AGUAS
// NPC: Marqués de Dos Aguas | questId: "Marques"
// Primera partida: 12 dineros + 1 carta mítica.
// Victoria principal: nivel, salvoconducto y encargo de la espada de Damasco.
// Revancha tras completar el encargo: +/-12 y carta mítica, sin nivel ni objeto.
// ============================================================

VAR marques_vencido = false
VAR marques_desafiado = false

== Marques ==
{ required_outfit != "Any" && outfit != required_outfit: -> rechazo }
{ marques_vencido:
    { GetFlag("encargo_hijo"): -> revancha | -> sinEncargo }
}
{ GetFlag("cantarida_entregada"):
    { marques_desafiado: -> reintento | -> desafio }
}
{ not HasItem("cantarida"): -> sin_cantarida }
Marqués: ¿Quién sois?
Caterina: Me llamo Josep Ferrández de Mesa. Vengo de parte de fray Lucio del Santo Calvario, para daros este brebaje de cantárida.
Marqués: Ah, sí, lo estaba esperando. ¿Cómo puedo agradeceros el servicio? ¿Sois hábil con la espada o con el arco? ¿Queréis formar parte de mi hueste?
Caterina: Os agradezco el ofrecimiento, señor marqués, pero soy soldado de fortuna y mi objetivo es conseguir llegar a Sevilla para embarcarme a las Indias.
Marqués: Entiendo. ¿Sabéis que en Levante va a haber un gran torneo de Naipes y que el ganador conseguirá un pasaje para ir a Sevilla?
Caterina: No tenía ni idea. Muchas gracias por la información. Se me dan bien los Naipes. Quizá lo intente.
~ TakeItem("cantarida")
~ SetFlag("cantarida_entregada", true)
-> desafio

= desafio
Marqués: ¿Se os dan bien los Naipes? ¿Queréis jugar una partida conmigo?
Caterina: Sería todo un honor, señor.
Marqués: Estas son mis condiciones. Nos jugaremos 1 sueldo y 1 carta mítica. ¿Os place?
{ not HasGold(12): -> farmear }
Caterina: Por supuesto, señor. Juguemos.
~ StartCombat("Marques.resolucion")
-> DONE

= resolucion
{ combat_won: -> victoria | -> derrota }

= farmear
Caterina: Es mucho dinero para mí, señor. Pero voy a ganárselo a los guardas y vuelvo a veros.
-> END

= reintento
Caterina: Señor marqués, ¿os apetece una partida de Naipes?
Marqués: Las condiciones son las mismas: 1 sueldo y 1 carta mítica. ¿Vamos allá?
{ not HasGold(12): -> farmear }
Caterina: Estoy preparado.
~ StartCombat("Marques.resolucion")
-> DONE

= victoria
Marqués: Habéis tenido mucha suerte. Los astros se han alineado con vos. Pero no volverá a pasar. Ya me daréis la revancha más adelante.
Caterina: Cuando queráis, señor.
Marqués: Si vais a Levante, tengo un encargo para vos. Es algo bastante especial. ¿Estáis dispuesto a hacerlo?
Caterina: Por supuesto, señor. Estoy a vuestras órdenes.
Marqués: Debéis ir al cementerio que hay extramuros de Levante. Hablad allí con el sepulturero. Conseguid que os diga dónde está la tumba de un converso llamado Ginés de la Santa Cruz Dorada. Id allí y cavad en su tumba hasta que encontréis una espada de Damasco. Cogedla y llevádsela a su hijo, D. Martín de la Santa Cruz. Es un arrendador de impuestos, que vive en el palacio del arrabal sur de Levante.
Caterina: Lo haré, señor.
Marqués: Os daré un salvoconducto para que podáis entrar en Levante por la puerta norte.
Caterina: Gracias, señor. Parto hacia el cementerio. Con Dios.
Marqués: Con Dios.
~ marques_vencido = true
~ FinishQuest("Marques", true)
-> END

= derrota
Marqués: Jajaja, un simple guarda no me puede ganar nunca; tengo las mejores cartas en mi mazo.
Caterina: Tenéis razón, señor. Es un gran mazo. ¿Me daréis la revancha en otra ocasión?
Marqués: Claro que sí. Volved a retarme cuando queráis.
~ marques_desafiado = true
~ FinishQuest("Marques", false)
-> END

= rechazo
Marqués: No recibo en audiencia a una desconocida. Hablad primero con mis guardas.
Caterina: Con Dios.
-> END

= sin_cantarida
Marqués: Fray Lucio no os habría enviado sin el preparado que estoy esperando.
Caterina: Volveré con el encargo, mi señor.
-> END

= sinEncargo
Marqués: ¿Le habéis entregado ya la espada al converso?
Caterina: No, mi señor.
Marqués: ¿A qué estáis esperando? No quiero veros por aquí.
-> END

= revancha
{ not HasGold(12): -> farmear }
Marqués: ¿Le habéis entregado ya la espada al converso?
Caterina: Sí, mi señor. ¿Os apetece una partida de Naipes?
Marqués: Cómo voy a deciros que no. Por mi honor, que os tengo que ganar. Ya sabéis el acuerdo: 1 sueldo y 1 carta mítica.
Caterina: Adelante.
~ StartCombat("Marques.resolucion_revancha")
-> DONE

= resolucion_revancha
{ combat_won: -> revancha_victoria | -> revancha_derrota }

= revancha_victoria
Marqués: Seguís aliado con la fortuna. Aquí tenéis las monedas y la carta.
Caterina: Gracias, señor, un placer, como siempre.
Marqués: Con Dios.
~ FinishQuest("Marques", true)
-> END

= revancha_derrota
Marqués: Vuestra suerte ha terminado. Me encanta derrotaros. Volved cuando queráis.
Caterina: Aquí tenéis el sueldo y la carta mítica. Con Dios.
~ FinishQuest("Marques", false)
-> END
