// ============================================================
// MISIÓN 11.2 — FRAY ARSACIO
// ============================================================

VAR fray_arsacio_desafiado = false

== FrayArsacio ==
{ outfit != "Caterina": -> rechazo }
{ fray_arsacio_desafiado: -> revancha }
Sor María: Está dispuesto a daros la información, pero solo si le ganáis a los Naipes. Si os gana, le daréis 2 sueldos y 2 cartas míticas. Si ganáis vos, os dará 1 carta mítica y la información sobre el paradero del niño. ¿Aceptáis?
Caterina: El precio es muy alto, pero necesito la información. Por favor, avisad a fray Arsacio. Estoy dispuesta para jugar.
Sor María: Os deseo mucha suerte.
~ fray_arsacio_desafiado = true
{ HasGold(24): -> jugar | -> sinDinero }

= jugar
~ StartCombat("FrayArsacio.resolucion")
-> DONE

= sinDinero
Caterina: Volveré cuando tenga los 24 dineros preparados.
Sor María: Con Dios.
-> END

= resolucion
{ combat_won: -> victoria | -> derrota }

= victoria
Fray Arsacio: ¿Qué tipo de brujería me habéis lanzado? Seguro que el demonio está detrás de vuestros movimientos.
Caterina: Solo mi entendimiento me guía. Cumplid con vuestra palabra. Dadme la carta mítica y la información.
Fray Arsacio: El bebé que buscáis fue comprado hace 8 años por un francés llamado Jean-Jacques Delon, que tiene su taller en la calle de los terciopeleros. Tomad vuestra carta. Espero que la sepáis utilizar bien.
Caterina: Gracias por la información y por la carta. Con Dios, fray Arsacio.
Fray Arsacio: Tenemos una cuenta pendiente. Quizá nos volvamos a ver en una mesa de juego. Con Dios.
~ SetFlag("info_terciopelero", true)
~ FinishQuest("FrayArsacio", true)
-> END

= derrota
Fray Arsacio: Lo tenéis bien merecido. ¡Cómo osa una mujerzuela enfrentarse a mí! Dadme mis monedas y mis cartas.
Caterina: Aquí tenéis. ¿Me daréis la oportunidad de volver a intentarlo?
Fray Arsacio: Por supuesto, sois una buena fuente de ingresos. ¡Mujer estólida!
Caterina: ¿Las condiciones serán las mismas?
Fray Arsacio: Lo serán.
~ FinishQuest("FrayArsacio", false)
-> END

= revancha
Caterina: Buen día, fray Arsacio.
Fray Arsacio: ¿Venís a alegrarme la existencia? Sois una mujer muy rentable para mí.
Caterina: ¿Jugamos?
Fray Arsacio: De acuerdo.
{ HasGold(24): -> jugar_revancha | -> sinDinero_revancha }

= jugar_revancha
~ StartCombat("FrayArsacio.resolucion")
-> DONE

= sinDinero_revancha
Caterina: Volveré cuando tenga los 24 dineros preparados.
Fray Arsacio: Os esperaré.
-> END

= rechazo
Fray Arsacio: No tengo nada que tratar con vos mientras ocultéis quién sois.
-> END
