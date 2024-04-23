# Dama

* Hacer tienda con sobres
* Creador de mazos
* Arena

# Fases
1. Inicio ->Automatico (No se puede usar instantaneo)
	1. Enderezar (Desgirar cartas utilizadas con anterioridad: ataques, mana, efectos)
	2. Mantenimiento (Efectos que se activen en ese momento por sus características.)
	3. Robo (Robar carta de la biblioteca)
   
3. Principal 1
	Se puede sacar una tierra
	Ya se pueden girar tierras para gastarlos (Mana no usado resta vida, pero vamos no hacerlo)
	(Los pros no sacan mucho mana aquí, lo guardan para el combate)

4. Combate (5 pasos)
   
	1. Paso de Comienzo de Combate: Activar habilidades de cartas (artefactos ya dentro activables actes del combate, instantaneos etc)
    
	2. Paso de Declaracion de Atacantes: Mover de tu parte del tablero al centro de la mesa y se giran para indicar que son atacantes. Si tienen vigilancia no giran.
		* (Entre medio de estos pasos se pueden lanzar instantáneos, paso extra? Primero tira defensor(IA) luego el atacante)

	3. Paso de Declaración de Bloqueadores: Se declaran criaturas que bloqueen (Hay efecto que pueden hacer agro a ellas) **Recordar instantáneos que se pueden lanzar por ambas partes, tanto aquí como resto de fases
		* [Como en el anterior](Entre medio de estos pasos se pueden lanzar instantáneos)

	4. Paso de Daño de Combate: Se ejecutan daños atacantes->bloqueadores. Efectos (Daña primero, tiene prioridad/Doble golpe, daña dos veces, incluso si muere) Hay dos rondas de asignación de daño.
		* [Como en el anterior, mucho más raro](Entre medio de estos pasos se pueden lanzar instantáneos)

	5. Paso de Fin de Combate: Efectos que duran 'hasta el final del combate' acaban aquí. (Los efectos temporales(generalmente intantaneos) se van aquí, el resto son permanentes a menos que se indique lo contrario, distinguir de encantamientos etc). ->Automatico (No se puede usar instantaneo)
    
6. Principal 2
   
	Similar a la 1, lanzar tierra si no se hizo en la 1, otras cartas etc. Y hechizos.

8. Fase Final
   
	1. Paso de Fin de Turno: Se resuelven/realizan habilidades de "Al comienzo del final del paso final", [No se pueden lanzar instantaneos]
    
	3. Paso de Limpieza: Solo se pueden quedar 7 cartas, se descartan las que sobren. Se reinician daños (ejemplo, cartas que tienen 4 de defensa y 2 daños causados (2 de vida total), se reinicia la vida a 4) ->Automatico (No se puede usar instantaneo)

# Otros

2 formas de ganar:
1. Quitarle toda la vida al contrincante
2. (Generalmente con blancas y pros)Cuando un rival se queda sin cartas en la baraja para robar

Tener en cuenta:
* que continuar se hace en cada paso no automatico
* que continua automaticamente si no puedes hacer nada (no hay atacando, por lo que no se tiene que bloquear etc)
* que debe de haber una pausa incluso en las fases automaticas para que el jugador sepa lo que pasa
* que se puede girar/desgirar, cambiar bloqueadores etc, cambios que se quiera dentro del turno **Excepto tierras, una vez activadas se deben usar
* que se podría hacer automático el elegir cartas y que se giren las tierras necesarias de forma automatica
* que existen replicas (posibles de implementar) en cada turno de defenda, ataque etc.
* que decides a que atacar primero si te defienden con varios
* que se decide al azar quien ataca primero
* que una carta solo puede defender de uno

* que instantaneos se pueden utilizar en cualquier momento, incluso antes de declarar ataques

Consejos (Sacar del MTG arena):
* Indicaciones visuales
* Botón se siguiente indica accion

Efectos, a considerar:
* mareo-> no atacar en primer turno y tambien no utilizar ciertas habilidades que requieren de girar
