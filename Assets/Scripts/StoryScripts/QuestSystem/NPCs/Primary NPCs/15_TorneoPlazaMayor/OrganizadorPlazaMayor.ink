// ============================================================
// MISIÓN 15 — EL TORNEO DE LA PLAZA MAYOR
// NPC: Organizador de la Plaza Mayor  |  questId: "OrganizadorPlazaMayor"
// requiredOutfit: Josep + requiredItems: "invitacion_plazamayor"
// Inscripción: 7 sueldos. Torneo de 6 rondas eliminatorias.
//   Ganar la 6ª (la final) -> 354 dineros + 3 cartas míticas + 9 raras +
//   PASAJE a Sevilla (capitán Tornabuoni). FIN DEL JUEGO.
// ============================================================


== OrganizadorPlazaMayor ==
Caterina: Buen día.
Organizador: Buen día.
Caterina: Vengo a jugar el torneo de Naipes.
Organizador: ¿Tenéis la invitación?
Caterina: Sí, la gané en el torneo clasificatorio del mercado.
Organizador: Cierto. Está todo en regla. La inscripción cuesta 7 sueldos. ¿Los tenéis?
* [Tengo el dinero] -> conDinero
+ [No tengo el dinero] -> sinDinero

= sinDinero
Caterina: Ahora mismo no, pero los traeré en breve. Con Dios.
Organizador: Con Dios.
-> END

= conDinero
Caterina: Sí, aquí están las monedas.
Organizador: Perfecto. Estas son las instrucciones del torneo. El torneo tiene 6 rondas eliminatorias. Si perdéis la primera, os iréis de vacío. Si ganáis la primera, os llevaréis 9 dineros y 2 cartas raras. Si ganáis la segunda, recibiréis 21 dineros y 5 cartas raras. Si ganáis la tercera, conseguiréis 39 dineros y 9 cartas raras. Si ganáis la cuarta, os llevaréis 84 dineros, 1 carta mítica y 9 cartas raras. Si ganáis la quinta (la semifinal), recibiréis 174 dineros, 2 cartas míticas y 9 cartas raras. Y si ganáis la sexta partida, la final, conseguiréis 354 dineros, 3 cartas míticas, 9 cartas raras y un pasaje para viajar en el barco del capitán Tornabuoni a Sevilla.
Organizador: ¿Estáis preparado? Señores, barajen sus mazos.
~ StartCombat("OrganizadorPlazaMayor.resolucion")
-> DONE

= resolucion
{ combat_won: -> victoria | -> derrota }

// ------------------------------------------------------------
// RAMA VICTORIA (gana las 6 rondas) -> pasaje a Sevilla -> FIN DEL JUEGO
// ------------------------------------------------------------
= victoria
Organizador: ¡Enhorabuena! Has conseguido 354 dineros, 3 cartas míticas, 9 cartas raras y un pasaje para viajar en el barco del capitán Tornabuoni a Sevilla. Id al muelle, hablad con el capitán y zarpad para Sevilla. Una nueva vida os espera.
Caterina: Capitán Tornabuoni, aquí está el pasaje.
Capitán Tornabuoni: Embarcad. Zarpamos en seguida...
Caterina: Capitán, ¿os apetece una partida de Naipes?
// [FIN DEL JUEGO]
~ FinishQuest("OrganizadorPlazaMayor", true)
-> END

// ------------------------------------------------------------
// RAMA DERROTA (no gana las 6) -> recompensa parcial; puede reintentar
// ------------------------------------------------------------
= derrota
Organizador: Has conseguido tus dineros y tus cartas. Vuelve a intentarlo cuando tu mazo sea más fuerte.
~ FinishQuest("OrganizadorPlazaMayor", false)
-> END
