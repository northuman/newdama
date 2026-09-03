// ============================================================
// MISIÓN 13 — CON EL NIÑO ANTE D. MARTÍN DE LA SANTA CRUZ
// NPC: D. Martín (entrega del hijo)  |  questId: "DMartinFinal"  |  DMartinFinalNpc
// requiredOutfit: Josep + requiredItems: "nino"
// Combate de cierre: solo cartas, 1 mítica para el ganador.
//   - Si gana Martín -> revancha hasta que gane Caterina.
//   - Si gana Caterina -> recibe la carta de recomendación para el torneo del mercado (M14).
// ============================================================


== DMartinFinal ==
{ required_outfit != "Any" && outfit != required_outfit: -> rechazo }
{ not HasItem("nino"): -> sinNino }
Caterina: Buen día, D. Martín. Os presento a vuestro hijo.
Martín: Mi hijo. ¿Es eso cierto?
Caterina: Sí, es vuestro hijo. Se lo he comprado a un maestro de la Calle de los Terciopeleros.
Martín: ¿Y cómo sabéis que es él?
Caterina: Hablé con Inés, en el prostíbulo de la puerta este. Está muy enferma. Después, estuve en la inclusa, donde le compré información a un fraile llamado fray Arsacio de la Santa Faz Divina, quien me llevó al citado maestro. Y a este le compré al mozo a buen precio, tras ganarle una partida de naipes. Haríais una obra de caridad si os trajeseis a Inés a vuestra casa, para serviros. No puede ganarse la vida con su cuerpo y acabará muriendo en la calle.
Martín: Veré qué puedo hacer por ella. Lavad y vestid a mi hijo. Caterina, habéis cumplido con la misión que os encomendé. Ahora he de cumplir yo con mi palabra. Haré las gestiones oportunas para que podáis inscribiros en el torneo clasificatorio del mercado. Pero antes..., deberéis derrotarme de nuevo. ¿Estáis preparada para ganarme?
Caterina: Sí, por supuesto. ¿Cuál es el acuerdo?
Martín: Esta vez no habrá dinero en juego. Solo cartas. 1 mítica para el que gane.
Caterina: Adelante, juguemos.
~ StartCombat("DMartinFinal.resolucion")
-> DONE

= resolucion
{ combat_won: -> victoria | -> derrota }

// ------------------------------------------------------------
// RAMA VICTORIA -> carta de recomendación para el torneo del mercado (M14)
// ------------------------------------------------------------
= victoria
Martín: Aquí tenéis vuestra carta mítica y mi escrito de recomendación para jugar el torneo del mercado. A más ver.
Caterina: Hasta pronto, señor.
~ TakeItem("nino")
~ FinishQuest("DMartinFinal", true)
-> END

// ------------------------------------------------------------
// RAMA DERROTA -> revancha obligatoria (hay que ganar para avanzar)
// ------------------------------------------------------------
= derrota
Caterina: Aquí tenéis vuestra carta mítica.
Martín: ¿Queréis la revancha?
Caterina: Desde luego.
~ StartCombat("DMartinFinal.resolucion")
-> DONE

= rechazo
Martín: Disculpadme, señora, pero este asunto requiere que venga vuestro marido.
Caterina: Volveré con Josep. Con Dios.
-> END

= sinNino
Martín: No podéis presentaros ante mí sin traer al niño. Volved cuando lo tengáis con vos.
Caterina: Tenéis razón. Volveré con él. Con Dios.
-> END
