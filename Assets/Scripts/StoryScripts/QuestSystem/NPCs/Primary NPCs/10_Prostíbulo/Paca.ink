// ============================================================
// MISIÓN 10 — EN EL PROSTÍBULO DEL ESTE
// NPC: Paca la Buen Gusto (alcahueta)  |  questId: "Paca"
// requiredOutfit: Caterina (la misión se resuelve con el atuendo femenino)
//   - Vestida de Josep -> diálogo alternativo "Paca_Josep" (Paca no revela que Inés está allí)
// Apuesta: si gana Paca -> 1 sueldo + 1 carta mítica
//          si gana Caterina -> desbloquea el diálogo independiente de Inés
// isRepeatable = true
// ============================================================


== Paca ==
{ outfit == "Josep": -> Paca_Josep | -> Paca_Caterina }

== Paca_Caterina ==
Caterina: Buen día.
Paca: Buen día. ¿Buscáis trabajo? Podéis ser camarera, pero si queréis ganar unos dineros más, podríais sacar partido de vuestro cuerpo.
Caterina: No es esa mi intención. De momento, me gano la vida al servicio del marqués de Dos Aguas.
Paca: ¿Y qué os trae por aquí?
Caterina: Estoy buscando a una mujer con la que compartí penurias en la Casa de Recogidas de Santa María Magdalena. Se llama Inés. Su padre acaba de morir y su madre quiere verla.
Paca: Si queréis información, tendréis que pagar un precio por ella.
Caterina: ¿Os gustan las cartas? ¿Qué os parece si nos lo jugamos a los Naipes? Si os gano, me dejaréis hablar con Inés. Si me ganáis, os pagaré un precio justo.
Paca: De acuerdo. Si os gano me daréis 1 sueldo y 1 carta mítica.
Caterina: Es un precio alto, pero confío en mis posibilidades.
{ HasGold(12): -> jugar | -> sinDinero }

= jugar
~ StartCombat("Paca.resolucion")
-> DONE

= sinDinero
Caterina: Volveré cuando tenga el sueldo y la carta preparados.
Paca: Volved cuando queráis. El precio será el mismo.
-> END

= resolucion
{ combat_won: -> victoria | -> derrota }

// ------------------------------------------------------------
// RAMA VICTORIA -> Paca desbloquea el diálogo independiente de Inés
// ------------------------------------------------------------
= victoria
Paca: Jugáis como un hombre. Seguro que me habéis hecho alguna trampa.
Caterina: Cumplid ahora con vuestra palabra.
Paca: Inés vive aquí. Hablad con ella cuando esté preparada.
~ FinishQuest("Paca", true)
-> END

// ------------------------------------------------------------
// RAMA DERROTA -> Caterina paga; rematch disponible (mismo precio)
// ------------------------------------------------------------
= derrota
Paca: ¡Ueeeeeee! Dadme las monedas y la carta, y volved cuando queráis. El precio será el mismo.
Caterina: Me volveréis a ver. Con Dios.
Paca: Y con la Virgen.
~ FinishQuest("Paca", false)
-> END

= revancha
Caterina: ¿Jugamos, señora?
Paca: ¿Queréis que os humille de nuevo? De acuerdo. El precio será el mismo. Si os gano me daréis 1 sueldo y 1 carta mítica. Si ganáis, hablaréis con Inés.
Caterina: De acuerdo, barajad vuestras cartas.
~ StartCombat("Paca.resolucion")
-> DONE

// ============================================================
// VARIANTE DE RECHAZO — Josep llega vestido de hombre
// ============================================================
== Paca_Josep ==
Josep: Buen día.
Paca: Buen día.
Josep: Busco a una mujer llamada Inés.
Paca: Aquí hay muchas mujeres, pero ninguna llamada así. Si buscáis placer, podéis elegir entre rubias, morenas y pelirrojas; flacas y gordas; jóvenes y experimentadas; baratas o caras. Tenemos de todo, para contentar a un señor como vos.
Josep: Y si os doy unos dineros, ¿no buscaríais a Inés entre vuestras meretrices?
Paca: No hay ninguna Inés.
Josep: De acuerdo. Volveré en otro momento a buscar solaz.
Paca: Con Dios.
Josep: Dios os guarde.
-> END
