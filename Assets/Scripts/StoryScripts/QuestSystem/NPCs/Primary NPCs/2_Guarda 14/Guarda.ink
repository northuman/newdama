// ============================================================
// MISIÓN 2 — CATERINA INTENTA CONSEGUIR ROPA DE HOMBRE
// NPC: Guarda de la Torre  |  questId: "Guarda"
// SO: initialResult.loseIsGameOver = true ; initialResult.levelUp = true
//     initialResult.itemsReward = [ropa_guarda]
//     unlocksOutfitChange = true ; isRepeatable = true ; requiredOutfit = Caterina
// Recompensas (dinero/nivel/items) las aplica el QuestInfoSO al ganar.
// Revancha casual: 3 dineros + 1 carta rara (SIN Game Over).
// ============================================================

VAR guarda_vencido = false

== Guarda ==
{ guarda_vencido: -> revancha }
{ not HasGold(12): -> sin_dinero }
Guarda: ¿Quién va? ¡Alto!
Caterina: Sosegaos, señor guarda. Solo soy una joven.
Guarda: ¿Qué hace una joven como vos, sola en medio de la nada?
Caterina: Voy hacia mi aldea, tras hacer un encargo para mi padre. He tenido miedo en el camino y me he acercado a la torre en busca de protección.
Guarda: Habéis hecho bien. Aquí podéis estar segura.
Caterina: ¿Estáis solo aquí? ¿Vigilando?
Guarda: Sí, a la espera de que venga un compañero a relevarme y pueda volver al castillo del marqués de Dos Aguas.
Caterina: Con la compañía del vino y de las cartas, por lo que veo.
Guarda: ¿Me juzgáis?
Caterina: No. A mí me encanta jugar a los Naipes.
Guarda: No creo que sepáis jugar.
Caterina: Yo misma tengo mi baraja. ¿Queréis jugar?
Guarda: No sería un combate justo. Una dama contra un soldado. De todos es sabido que las mujeres no destacan por su inteligencia, ni por su habilidad en el juego.
Caterina: Poned un precio y lo pagaré, siempre que no esté en juego mi virtud.
Guarda: Algo de dinero tendréis, si venís de hacer el encargo de vuestro padre. ¿Tenéis alguna carta rara en vuestro mazo?
Caterina: Sí, tengo dinero y me puedo jugar una carta mítica. Pero quiero algo a cambio... Si os gano, quiero una carta mítica de vuestro mazo y la vestimenta de guarda.
Guarda: Estáis de broma...
Caterina: No. Si me visto de guarda, iré más segura por los caminos. ¿Acaso tenéis miedo?
Guarda: Miedo, nunca. Ni tampoco escrúpulos de ganar a una loca. Si os gano, ¿me daréis 1 sueldo?
Caterina: Es todo lo que tengo, pero... acepto. Juguemos.
~ StartCombat("Guarda.resolucion")
-> DONE

// ── Resolución del combate (combat_won lo fija DialogueManager) ──
= resolucion
{ combat_won: -> victoria | -> derrota }

// ------------------------------------------------------------
// RAMA VICTORIA -> el SO otorga ropa_guarda + carta + desbloquea outfit
// ------------------------------------------------------------
= victoria
Guarda: Jugáis como el mismo demonio. Alejaos de mi vista. No os quiero volver a ver.
Caterina: Tengo hambre. ¿Dónde puedo comprar algo de pan?
Guarda: Id hacia el norte, hasta llegar al río. Entonces seguid su curso hacia el este hasta encontrar unos molinos. Allí podréis conseguir comida. Además, en esa zona hay jugadores de Naipes.
Caterina: Con Dios, señor guarda.
Guarda: Que el Altísimo os proteja, joven tahúr.
~ guarda_vencido = true
// Abre la puerta de la torre para que Caterina/Josep pueda continuar hacia el norte.
~ RaiseWorldEvent("abrir_puerta_torre")
~ FinishQuest("Guarda", true)
-> END

// ------------------------------------------------------------
// RAMA DERROTA -> GAME OVER (loseIsGameOver = true en el SO)
// ------------------------------------------------------------
= derrota
Guarda: Una loca es una loca. Alejaos antes de que cambie de humor.
~ FinishQuest("Guarda", false)
-> END

= sin_dinero
Guarda: Volved cuando tengáis el sueldo completo que queréis apostar.
Caterina: Así lo haré. Con Dios.
-> END

// ------------------------------------------------------------
// REMATCH (isRepeatable): Caterina vuelve a la torre a jugar
// Apuesta: 3 dineros + 1 carta rara (SIN Game Over)
// ------------------------------------------------------------
= revancha
{ not HasGold(3): -> sin_dinero_revancha }
Caterina: Señor guarda, Dios os proteja. ¿Os acordáis de mí?
Guarda: ¿Cómo iba a olvidaros? Me ganasteis la ropa.
Caterina: ¿Os apetece jugar a los Naipes?
Guarda: ¿Os jugaríais mi ropa?
Caterina: No, pero sí 3 dineros y 1 carta rara. ¿Aceptáis?
Guarda: Acepto.
~ StartCombat("Guarda.resolucion_revancha")
-> DONE

= resolucion_revancha
{ combat_won: -> revancha_victoria | -> revancha_derrota }

= revancha_victoria
Guarda: Debéis usar algún amuleto de la buena fortuna. Tenéis mucha suerte. Aquí están el dinero y la carta. Adiós.
~ FinishQuest("Guarda", true)
-> END

= revancha_derrota
Caterina: Aquí tenéis vuestro botín. Con Dios.
~ FinishQuest("Guarda", false)
-> END

= sin_dinero_revancha
Guarda: Sin tres dineros no hay partida. Volved cuando podáis pagar la apuesta.
Caterina: De acuerdo. Con Dios.
-> END
