// ============================================================
// MISIÓN 10.2 — CON INÉS
// NPC: Inés | questId: "Ines"
// Prerrequisito: completar Paca (M10.1)
// ============================================================

== Ines ==
{ outfit == "Josep": -> llegaJosep | -> conversacion }

= llegaJosep
// Inés solo atiende a Caterina. Si se llega con el disfraz masculino,
// la conversación fuerza el cambio antes de mostrar el diálogo.
~ SetOutfit("Caterina")
-> conversacion

= conversacion
Inés: Buen día. ¿Decís que el fementido de mi padre ha muerto y que mi madre está sola?
Caterina: No, lo siento. Tuve que utilizar esa alicantina para poder hablar con vos. No sé nada de vuestro padre. Me envía D. Martín de la Santa Cruz, un antiguo pretendiente vuestro.
Inés: Martín, ese farfante que no hizo nada por mí, más que arrancarme la virtud.
Caterina: D. Martín me ha dicho que cree que vuestro hijo está vivo. Lo quiere reconocer y hacerle heredero de su patrimonio.
Inés: ¿Que está vivo? No os imagináis el dolor que sentí tras el parto, cuando me arrebataron a mi bebé y me dijeron que había muerto. Nació llorando, pero sus gritos dejaron de oírse al poco tiempo. Me destrozaron la vida. ¿Decís que está vivo?
Caterina: Eso parece. ¿Tenéis idea de dónde pudieron llevarlo?
Inés: A las mujeres de la casa de recogidas no nos permitían tener bebés allí. Se rumoreaba que eran llevados a la Inclusa del Hospital de San Miguel, que está junto a la Plaza Mayor. Quizá allí podáis conseguir alguna pista. Preguntad por Sor María de la Cinta. Me conoce desde que éramos niñas y quizá pueda ayudaros.
Caterina: Gracias, Inés. Intentaré encontrar a vuestro hijo.
Inés: Ojalá pueda tener una vida mejor que yo. Tengo el mal francés y no sé si llegaré a la próxima primavera. Id con Dios.
Caterina: Lo siento. Que Dios, la Virgen y todos los santos apacigüen vuestros dolores, y la penitencia de esta vida terrena os haga disfrutar más en el Paraíso.
~ SetFlag("ines_info", true)
~ FinishQuest("Ines", true)
-> END
