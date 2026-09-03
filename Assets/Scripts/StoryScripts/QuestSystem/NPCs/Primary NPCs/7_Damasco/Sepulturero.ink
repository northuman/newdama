// ============================================================
// MISIÓN 7 — BUSCANDO LA ESPADA DE DAMASCO EN EL CEMENTERIO DE LEVANTE
// NPC: Joan el Malparit (sepulturero)  |  questId: "Sepulturero"
// requiredOutfit: Josep (si va de mujer -> knot "Sepulturero_rechazo")
// Apuesta info: si gana Josep -> ubicación de la tumba + 1 carta rara
//               si gana el sepulturero -> Josep entrega 3 cartas raras
// Tras ganar -> cavar la tumba de Ginés de la Santa Cruz Dorada (espada de Damasco)
// isRepeatable = true (rematch: 8 dineros + 2 cartas raras)
// ============================================================


== Sepulturero ==
Caterina: Buen día.
Sepulturero: Buen día para los vivos. ¿Qué se os ha perdido por aquí?
Caterina: Busco una tumba por encargo del señor marqués de Dos Aguas.
Sepulturero: ¿Y cuál de estos muertos interesa a una persona tan ilustre? Si aquí no hay más que apestados, gente sin nombre, moriscos y marranos. La gente de orden está enterrada en las iglesias de Levante.
Caterina: Busco la tumba de un converso llamado Ginés de la Santa Cruz Dorada.
Sepulturero: Sí, conozco esa tumba.
Caterina: ¿Y dónde está?
Sepulturero: Toda información tiene un precio.
Caterina: Veo que tenéis una baraja de Naipes. ¿Nos jugamos la información a los Naipes? Si os gano, me diréis dónde está la tumba y me daréis una carta rara. Si me ganáis, os daré tres cartas raras. ¿Os place el acuerdo? Si sois bueno, poco tenéis que temer.
Sepulturero: Acepto.
~ StartCombat("Sepulturero.resolucion")
-> DONE

= resolucion
{ combat_won: -> victoria | -> derrota }

// ------------------------------------------------------------
// RAMA VICTORIA -> ubicación de la tumba + 1 carta rara
// ------------------------------------------------------------
= victoria
Sepulturero: Diantre, no sé cómo me habéis podido ganar. Estáis aliado con las fuerzas negras. Aquí tenéis vuestra carta rara. La tumba del marrano está en la parte del cementerio más cercana a la ermita.
Caterina: Gracias. Ha sido un placer jugar con vos.
~ SetFlag("tumba_conocida", true)
// Abre la puerta de Levante solo al completar por primera vez la misión principal.
~ FinishQuest("Sepulturero", true)
-> END

// ------------------------------------------------------------
// RAMA DERROTA -> Josep entrega 3 cartas raras; reintento disponible
// ------------------------------------------------------------
= derrota
Sepulturero: Sí, sí, os he derrotado. Sois un botarate. Dadme las 3 cartas raras.
Caterina: Aquí tenéis.
Sepulturero: Si queréis saber dónde está la tumba de ese malnacido, tendréis que ganarme una partida de Naipes, en otra ocasión.
Caterina: Con Dios.
~ FinishQuest("Sepulturero", false)
-> END

// ------------------------------------------------------------
// REINTENTO tras derrota (mismo acuerdo)
// ------------------------------------------------------------
= reintento
Caterina: Quiero intentarlo de nuevo. ¿Mantenemos el mismo acuerdo?
Sepulturero: Por supuesto, soy hombre de palabra. Juguemos.
~ StartCombat("Sepulturero.resolucion")
-> DONE

// ------------------------------------------------------------
// REMATCH (isRepeatable) tras conseguir la espada: 8 dineros + 2 cartas raras
// ------------------------------------------------------------
= revancha
Caterina: Buen día. ¿Qué tal va la faena?
Sepulturero: Tan mal como siempre.
Caterina: ¿Os apetece jugar una partida de Naipes?
Sepulturero: Desde luego. ¿Nos jugamos 8 dineros y 2 cartas raras?
Caterina: Hecho.
~ StartCombat("Sepulturero.resolucion_revancha")
-> DONE

= resolucion_revancha
{ combat_won: -> revancha_victoria | -> revancha_derrota }

= revancha_victoria
Sepulturero: Aquí tenéis las monedas y las cartas. No entiendo cómo me habéis podido ganar.
Caterina: Gracias. Con Dios.
~ FinishQuest("Sepulturero", true)
-> END

= revancha_derrota
Sepulturero: Dadme lo mío.
Caterina: Aquí tenéis. A más ver.
~ FinishQuest("Sepulturero", false)
-> END

// ============================================================
// VARIANTE DE RECHAZO — Caterina llega VESTIDA DE MUJER
// ============================================================
== Sepulturero_rechazo ==
Caterina: Buen día.
Sepulturero: Buen día, joven. ¿Qué buscáis por aquí?
Caterina: Quiero honrar la memoria de un pariente, rezando ante su tumba.
Sepulturero: Este no es un sitio seguro para una dama.
Caterina: Pensaba que sí, al estar tan cerca de Levante.
Sepulturero: Tan cerca y a la vez tan lejos... Si queréis saber dónde está la tumba que buscáis, deberéis pagar un precio. 1 sueldo o un poco de placer.
Caterina: Mi virtud no está en venta. Adiós.
-> END
