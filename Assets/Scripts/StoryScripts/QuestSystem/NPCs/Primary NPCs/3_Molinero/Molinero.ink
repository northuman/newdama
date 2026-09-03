// ============================================================
// MISIÓN 3 — CATERINA SE CONVIERTE EN JOSEP Y BUSCA COMIDA Y CARTAS
// NPC: Molinero  |  questId: "Molinero"
// SO: requiredOutfit = Josep ; levelUp = true ; goldReward = 4 ; loseGoldPenalty = 4
//     itemsReward = [pan_negro, setas, queso] ; isRepeatable = true
// Si va de mujer -> rama = rechazo. Reentrada (ya vencido):
//   - setas aún NO entregadas -> = sinEncargo
//   - setas ya entregadas      -> = revancha (4 dineros + 1 carta rara)
// ============================================================

VAR molinero_vencido = false
VAR molinero_desafiado = false

== Molinero ==
{ required_outfit != "Any" && outfit != required_outfit: -> rechazo }
{ molinero_vencido:
    { setas_entregadas: -> revancha | -> sinEncargo }
}
{ molinero_desafiado: -> reintento }
{ not HasGold(4): -> sin_dinero }
Caterina: Buen día, señor molinero. Quería comprar algo de pan de trigo o de centeno, lo que tengáis a bien venderme.
Molinero: Esto no es una tahona, guarda. Molemos la harina, pero no fabricamos pan. La vendemos a quienes nos pagan buen precio por ella.
Caterina: Pero algo de comer y de beber tendréis...
Molinero: Sí, pero solo lo necesario para dar de comer a mi familia.
Caterina: ¿Y no me podéis vender nada?
Molinero: Bueno, todo es negociable si hay dinero por delante.
Caterina: Lo hay. Veo que tenéis cartas. ¿Sabéis jugar a los Naipes?
Molinero: ¿Os burláis de mí? Soy el mejor jugador de Naipes de estos parajes. Nadie en las aldeas del castillo del marqués me gana.
Caterina: Yo también juego a los Naipes entre vigilancia y vigilancia. ¿Os jugaríais algo de pan y alguna carta?
Molinero: ¿Tenéis dinero y cartas poco comunes en vuestra baraja?
Caterina: Sí, la duda ofende.
Molinero: Estas son mis condiciones. Nos jugaremos 4 dineros y una carta poco común. No quiero abusar de vos.
Caterina: De acuerdo, pero si os gano, me daréis también un poco de pan negro.
Molinero: Trato hecho.
~ StartCombat("Molinero.resolucion")
-> DONE

= resolucion
{ combat_won: -> victoria | -> derrota }

// ------------------------------------------------------------
// RAMA VICTORIA -> el SO otorga pan_negro + setas + queso (encargo)
// ------------------------------------------------------------
= victoria
Molinero: Diantre, guarda, me habéis ganado en buena lid. Aquí tenéis vuestra carta, vuestros 4 dineros y vuestro mendrugo de pan negro.
Caterina: ¡Gracias, señor molinero! De bien nacidos es pagar las deudas del juego.
Molinero: ¿Hacia dónde vais? Si os dirigís hacia el oeste, ¿me podríais hacer un encargo? Tengo que hacerle llegar estas setas de amanita muscaria al monje que encontraréis en la ermita de la Santa Cruz, que está al sur del castillo del marqués. No os las comáis; son venenosas y os causarían una muerte terrible. El ermitaño se llama fray Lucio del Santo Calvario.
Caterina: ¿Y qué gano yo haciéndoos el favor?
Molinero: Os daré otro mendrugo de pan y un trozo de queso, para que no paséis hambre por el camino.
Caterina: Acepto. El ruido de las tripas me impide deciros que no. Le llevaré las setas al ermitaño. Señor molinero, ¿dónde puedo jugar a los Naipes? Me gusta jugar a las cartas y si vos sois el mejor de estas tierras, creo que puedo ganar algo de dinero extra.
Molinero: Podéis ir a las ventas que hay al sur del meandro del río. Preguntad en la que está entre los dos puentes por Jaume el Destripat. Es el mesonero. Organiza partidas y en ellas vende su vino dulce y sus viandas. No dejéis de probar su guiso de caracoles.
Caterina: De acuerdo. Gracias por la información.
Molinero: Que Dios os acompañe.
Caterina: Con Dios.
~ molinero_vencido = true
~ molinero_desafiado = false
// Abre la puerta del molino para que Josep pueda seguir su camino.
~ RaiseWorldEvent("abrir_puerta_molino")
~ FinishQuest("Molinero", true)
-> END

// ------------------------------------------------------------
// RAMA DERROTA -> el SO aplica -4 dineros; rematch disponible
// ------------------------------------------------------------
= derrota
~ molinero_desafiado = true
~ FinishQuest("Molinero", false)
-> END

// ------------------------------------------------------------
// REINTENTO tras una derrota: mismo acuerdo de la primera partida
// ------------------------------------------------------------
= reintento
{ not HasGold(4): -> sin_dinero }
Caterina: ¿Me dais la revancha?
Molinero: Por supuesto. Estas son mis condiciones. Nos jugaremos 4 dineros y una carta poco común. No quiero abusar de vos.
Caterina: ¡Hecho!
~ StartCombat("Molinero.resolucion")
-> DONE

// ============================================================
// VARIANTE DE RECHAZO — Caterina vestida de MUJER
// ============================================================
= rechazo
Molinero: ¿Qué buscáis, joven?
Caterina: Quería jugar a los Naipes. Me han dicho que sois un maestro.
Molinero: ¿Jugar con vos? No me hagáis reír. Id a casa con vuestro padre o vuestro marido y no me hagáis perder el tiempo. ¿Jugar con una mujer? No estáis a mi nivel.
-> END

= sin_dinero
Molinero: Sin cuatro dineros no puedo aceptar vuestra apuesta.
Caterina: Volveré cuando los tenga. Con Dios.
-> END

// ------------------------------------------------------------
// REENTRADA (Josep) — aún NO ha entregado las setas al ermitaño
// ------------------------------------------------------------
= sinEncargo
Molinero: Buen día. ¿Ya le habéis entregado las setas al ermitaño?
Caterina: No, aún no.
Molinero: Entonces, ¿qué hacéis aquí? Marchaos antes que os dé con esta vara. No os quiero volver a ver hasta que cumpláis con el encargo.
Caterina: Con Dios.
-> END

// ------------------------------------------------------------
// REMATCH (Josep) — ya entregó las setas: 4 dineros + 1 carta rara
// ------------------------------------------------------------
= revancha
{ not HasGold(4): -> sin_dinero }
Molinero: ¿Qué os trae por aquí?
Caterina: Quería jugar con vos a los Naipes. ¿Os viene bien?
Molinero: Claro que sí. El trato es 4 dineros y una carta rara.
Caterina: De acuerdo. Juguemos.
~ StartCombat("Molinero.resolucion_revancha")
-> DONE

= resolucion_revancha
{ combat_won: -> revancha_victoria | -> revancha_derrota }

= revancha_victoria
Molinero: Aquí tenéis las monedas y la carta. Tenéis mucha suerte.
Caterina: Gracias. Con Dios.
~ FinishQuest("Molinero", true)
-> END

= revancha_derrota
Caterina: Tomad: vuestro dinero y vuestra carta. Nos volveremos a ver.
Molinero: Cuando queráis. Aquí os espero.
~ FinishQuest("Molinero", false)
-> END
