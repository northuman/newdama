# Dama

Sistema de combate por cartas con temática histórica para el videojuego Dama.

Implementado:
- Menú de inicio y pausa
- Robo inicial
- Drag and drop
- Seleción
- Girar cartas
- Cálculo de costes de maná
- Mostrar mensajes
- Mostrar flavour text de las cartas con click derecho
- Efectos de las cartas
- Cálculo de daños
- "Destruir" cartas

Pendiente:
- Sistema de turnos
- IA del oponente
- Selección de atacantes y bloqueadoras
- Editor de mazos



## Actualización Efectos -------
Se ha creado carpeta de "Efectos" dentro de esta estan los efectos divididos en categorias:
- Atributos: La mayoria son simples booleanos que se tienen en cuenta en los metodos de ataque y bloqueo 
(Esto tiene que tenerlo en cuenta la persona encargada del bucle de juego).
- Habilidades: Que estan divididas en:
	- Buffs
	- Combate
	- Coste
	- Daño
	- Gestion de Cartas

(Esto tiene que tenerlo en cuenta la persona encargada del bucle de juego)
- Triggers: Estos son accionesque se ejecutan al cumplirse una condicion, por ejemplo "Al entrar al campo de batalla".
(Esto tiene que tenerlo en cuenta la persona encargada del bucle de juego).

Se han añadido los metodos Atacar, RecibirDanio, DestruirCarta, TratarVida (optimizado para que se trate directamente al jugador) y Bloqueo
(Este último método no tiene la lógica de bloqueo implementada ya que no hay bucle de juego). -> Esto se ha implementado en el archivo
de "CartasJugadas"

Se ha modificado el fichero de "Carta" para que tenga una lista de efectos, esto se ha hecho para que cada carta pueda tener varios efectos a la vez y no se limite a uno solo. Ademas
de que se crean a partir del tipo de efecto que sea.
Hay un par de ejemplos de como otorgar efectos a las cartas en el constructor.

Se hizo una correcta instanciacion de las cartas en el fichero "DropZone"

Esta preparada la estructura del bucle en el archivo "Partida"

## Cosas por revisar/terminar de Efectos -------
Estas cosas no se han podido implementar del todo a falta de un bucle de juego pero se han dejado lo más preparado posible.
### Atributos
- Amenaza: no tenemos lógica de bloqueo, esta debe tratarse dentro de este metodo.
- DaniaDosVeces: no tenemos bucle de juego, esta debe tratarse dentro del bucle en distintos instantes que se considere.

### Habilidades
- TodasDebenBloquearHabilidad: no tenemos lógica de bloqueo.
- DestruirHabilidad: el metodo de destruir la carta esta terminado pero esta habilidad se debe tener en cuenta en el bucle de juego dependiendo de la seleccion fisica del jugador
- DescartarHabilidad: no se tiene acceso fisico a las cartas de la mano del oponente, esto ocurre en el bucle de juego.
- ElegirAccionHabilidad

### Triggers
- CuandoAtaca: no hay bucle por lo que no sabemos cuando se ataca -> metodo de ataque esta listo
- CuandoBloquea: no hay logica de bloqueo
- CuandoHaceDanio: no sabemos cuando hace daño, esto se trata en el bucle de juego
- CuandoOtraEntra: no sabemos esta informacion sin el bucle de juego
- CuandoContador: falta bucle del juego para tratar esta habilidad

El bucle se debe hacer en el fichero "Partida", esta todo preparado para su implemetacion