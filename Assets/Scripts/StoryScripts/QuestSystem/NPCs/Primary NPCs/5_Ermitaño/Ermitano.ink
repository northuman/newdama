// ============================================================
// MISIÓN 5 — ENTREGANDO LAS SETAS AL ERMITAÑO DE LA SANTA CRUZ
// NPC: Fray Lucio del Santo Calvario  |  questId: "Ermitano"
// SO: requiredOutfit = Any ; requiredItems = [setas] ; levelUp = true
//     goldReward = 6 ; loseGoldPenalty = 6 ; itemsReward = [pan_negro, cantarida]
//     isRepeatable = true
// Mundo abierto: si llega de Josep, el ermitaño la OBLIGA a cambiarse ->
//   ~ SetOutfit("Caterina"). Las setas se ENTREGAN (TakeItem) antes del combate
//   y se marca el VAR global setas_entregadas (lo lee el Molinero en su reentrada).
// ============================================================

VAR ermitano_vencido = false

== Ermitano ==
{ ermitano_vencido:
    { GetFlag("cantarida_entregada"): -> revancha | -> sinEncargo }
}
{ setas_entregadas: -> reintento }
{ not HasItem("setas"): -> sin_setas }
{ not HasGold(6): -> sin_dinero }
Caterina: Buen día. ¿Sois vos fray Lucio del Santo Calvario?
{ outfit == "Josep": -> llegaJosep | -> llegaCaterina }

= llegaCaterina
Ermitaño: Sí, lo soy, gracias a la bondad infinita de Dios y a la sabiduría que ocultan los Santos Evangelios. ¿Quién sois vos?
-> presentacion

// ------------------------------------------------------------
// El ermitaño descubre el disfraz y obliga a cambiarse
// ------------------------------------------------------------
= llegaJosep
Ermitaño: Sí, lo soy, gracias a la bondad infinita de Dios y a la sabiduría que ocultan los Santos Evangelios. ¿Quién sois vos y por qué vais disfrazada de hombre? ¿Qué burla es esta?
Josep: Perdonad, padre, es una historia larga de contar.
Ermitaño: Entrad en la ermita y cambiad vuestro atuendo. Es indecoroso.
~ SetOutfit("Caterina")
-> presentacion

= presentacion
Ermitaño: ¿Quién sois?
Caterina: Mi nombre es Caterina de Eraso. La Madre Superiora del Monasterio de la Merced es la hermana de mi madre.
Ermitaño: ¿Y qué hacéis sola y vestida como un soldado?
Caterina: Le gané los ropajes a un guarda del marqués, jugando a los Naipes, y voy más tranquila por los campos y los bosques vestida como un hombre.
Ermitaño: ¿Y qué os trae aquí? ¿Venís a rezar al Altísimo por la salvación de vuestra alma? ¿O venís a confesar vuestros pecados femeninos para que Dios, omnipotente y magnánimo, os perdone?
Caterina: Vengo de parte de un viejo molinero, que me ha pedido que os entregue estas setas venenosas.
Ermitaño: Ah, sí, las esperaba. Amanita muscaria.
~ TakeItem("setas")
~ setas_entregadas = true
Caterina: ¿Para qué las queréis? ¿Para fabricar veneno?
Ermitaño: Todo lo contrario: para elaborar un remedio contra la falta de virilidad. Estos campos y bosques tienen plantas y hierbas que permiten preparar pócimas y brebajes, que pueden sanar muchos males.
Caterina: Lo sé. En el monasterio, leí un libro sobre ungüentos, cataplasmas y filtros amatorios.
Ermitaño: ¿Sabéis leer?
Caterina: Sí, mi tía dio orden de que me enseñasen a leer y aproveché bien las lecciones. También sé escribir y contar, aparte de los saberes propios del hogar.
Ermitaño: Debéis sentiros muy afortunada, porque la lectura y la escritura no son aptas para todas las mujeres. Vuestra inteligencia es claramente inferior.
Caterina: Eso no es cierto.
Ermitaño: ¿Dudáis de mi palabra? ¿Acaso dudáis de lo que han escrito sobre esta materia los principales sabios de los últimos 500 años?
Caterina: Sí. Ponedme a prueba.
Ermitaño: ¿Cómo?
Caterina: ¿Con algún juego de habilidad? ¿Sabéis jugar a los Naipes?
Ermitaño: ¿Y quién no? Los Naipes y el vino son mis mayores pecados terrenales.
Caterina: Comprobad vos mismo si la inteligencia de una mujer es tan escasa.
Ermitaño: Sois una presuntuosa. De acuerdo. Nos jugaremos 6 dineros y 2 cartas raras. Y si me ganáis, además, os daré una hogaza de pan negro. ¿Aceptáis?
Caterina: El precio es alto, pero debo aceptarlo. Vuestra percepción sobre la capacidad de las mujeres es una provocación.
~ StartCombat("Ermitano.resolucion")
-> DONE

= resolucion
{ combat_won: -> victoria | -> derrota }

// ------------------------------------------------------------
// RAMA VICTORIA -> el SO otorga pan_negro + cantarida (encargo para el Marqués)
// ------------------------------------------------------------
= victoria
Ermitaño: Jugáis como los ángeles. Desde luego, sois inteligente como una ardilla. Aquí tenéis vuestras monedas y las cartas. Os las habéis ganado.
Caterina: Gracias, fray Lucio.
Ermitaño: ¿Hacia dónde vais ahora? ¿Al monasterio, con vuestra tía?
Caterina: No, Padre. Más bien, tenía la intención de ir a Levante. Me esperan parientes en la ciudad.
Ermitaño: ¿Podríais hacerme un recado?
Caterina: Claro, fray Lucio. Pero... ¿qué me daréis a cambio?
Ermitaño: Os daré otras 2 cartas raras.
Caterina: De acuerdo. ¿Qué he de hacer?
Ermitaño: Debéis ir al castillo del marqués de Dos Aguas y darle este preparado de cantárida. Tened mucho cuidado. No debe derramarse una sola gota por el camino.
Caterina: ¿Para qué sirve?
Ermitaño: Eso no os importa. Partid cuanto antes y aquí tenéis vuestras dos cartas. Si no lo entregáis con la mayor brevedad, el marqués hará que os corten el cuello. Id directa al castillo.
Caterina: Así lo haré.
~ ermitano_vencido = true
~ FinishQuest("Ermitano", true)
-> END

// ------------------------------------------------------------
// RAMA DERROTA -> el SO aplica -6 dineros; rematch disponible
// ------------------------------------------------------------
= derrota
Ermitaño: Veis. Las mujeres sois inferiores a los hombres. Debéis estar en casa, cuidando a la prole, o en los conventos, dedicadas al trabajo y a la oración. Volved cuando queráis. Me haréis ganar mucho dinero y me permitiréis mejorar mi mazo.
~ FinishQuest("Ermitano", false)
-> END

// Las setas ya se entregaron antes del primer combate. Si Caterina perdió,
// puede repetir el desafío sin necesitar un segundo objeto de misión.
= reintento
{ not HasGold(6): -> sin_dinero }
Caterina: Buen día, fray Lucio. ¿Jugamos de nuevo?
Ermitaño: Claro que sí, hija. El trato es el mismo: 6 dineros y 2 cartas raras. ¿Aceptáis?
Caterina: Por supuesto.
~ StartCombat("Ermitano.resolucion")
-> DONE

= sinEncargo
Ermitaño: ¿Qué hacéis aquí? Id al castillo.
-> END

= sin_setas
Ermitaño: El molinero no os habrá enviado con las manos vacías. Volved cuando tengáis las setas que espero.
Caterina: Así lo haré. Con Dios.
-> END

= sin_dinero
Ermitaño: No aceptaré el desafío si no podéis cubrir los seis dineros de la apuesta.
Caterina: Volveré cuando los tenga. Con Dios.
-> END

// ------------------------------------------------------------
// REMATCH (isRepeatable): 6 dineros + 2 cartas raras
// ------------------------------------------------------------
= revancha
{ not HasGold(6): -> sin_dinero }
Caterina: Gracias, fray Lucio. ¿Os apetece jugar una partida de Naipes?
Ermitaño: Siempre. Derrotaros es un placer de los pocos que me puedo permitir. ¿Nos jugamos 6 dineros y 2 cartas raras?
Caterina: Trato hecho.
~ StartCombat("Ermitano.resolucion_revancha")
-> DONE

= resolucion_revancha
{ combat_won: -> revancha_victoria | -> revancha_derrota }

= revancha_victoria
Ermitaño: Jugáis demasiado bien para ser una mujer. ¿Estáis poseída por el maligno?
Caterina: Padre, no tengáis mal perder.
Ermitaño: Tenéis razón. Aquí están las monedas y las cartas. Con Dios.
Caterina: Con Dios.
~ FinishQuest("Ermitano", true)
-> END

= revancha_derrota
Caterina: Fray Lucio, esta vez habéis ganado. Aquí tenéis el dinero y las cartas. Volveremos a vernos.
Ermitaño: Cuando queráis. La vida en la ermita es muy solitaria.
~ FinishQuest("Ermitano", false)
-> END
