# Dama - Sistema de Combate por Cartas

Sistema de combate por cartas con temática histórica para el videojuego Dama.

## 📋 Tabla de Contenidos

- [Descripción General](#descripción-general)
- [Características Implementadas](#características-implementadas)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Sistemas de Juego](#sistemas-de-juego)
- [Trabajo Pendiente](#trabajo-pendiente)
- [Notas Técnicas](#notas-técnicas)

## Descripción General

Dama es un juego de cartas basado en turnos donde dos jugadores compiten usando mazos de cartas con diferentes efectos y habilidades. El proyecto está desarrollado en **Unity** con **C#** como lenguaje principal.

### Objetivos del Juego
- Reducir la vida del oponente a 0
- Gestionar recursos (maná) para jugar cartas
- Utilizar habilidades y efectos especiales para ganar ventaja

## Características Implementadas

### Interfaz de Usuario
- ✅ Menú de inicio con transiciones animadas
- ✅ Menú de pausa durante la partida
- ✅ Visualización de cartas con zoom automático
- ✅ Mostrado de flavour text (descripción de cartas) con click derecho
- ✅ Sistema de mensajes en pantalla
- ✅ Panel de información de cartas en la mano

### Mecánicas de Mano y Mazo
- ✅ Robo inicial de cartas
- ✅ Sistema de robo de cartas durante la partida
- ✅ Límite de cartas en mano (descartes automáticos)
- ✅ Visualización del mazo y zona de descartes

### Drag and Drop
- ✅ Arrastrar cartas desde la mano al campo
- ✅ Zoom dinámico durante el arrastre
- ✅ Validación de zonas de juego
- ✅ Colocación automática en zonas correctas

### Sistema de Cartas
- ✅ Base de datos de cartas (CartaDatabase)
- ✅ Cartas con múltiples efectos simultáneos
- ✅ Cálculo automático de costes de maná
- ✅ Sistema de girar cartas (tap/untap)
- ✅ Cartas de tierras (generadores de maná)
- ✅ Cartas de criaturas y conjuros

### Sistema de Combate
- ✅ Selección de cartas atacantes
- ✅ Selección de cartas bloqueadoras
- ✅ Cálculo de daño
- ✅ Sistema primitivo de bloqueo implementado
- ✅ Conjuros básicos con selección de objetivo
- ✅ Control de vida del jugador y oponente
- ✅ Detección de victoria/derrota

### Efectos de Cartas
Las cartas pueden tener efectos de tres tipos:

#### Atributos (13 implementados)
- `Amenaza`: Debe ser bloqueada (lógica de bloqueo en bucle de juego)
- `Arrolla`: Daño excedente se transfiere al jugador
- `DaniaDosVeces`: El daño se aplica dos veces
- `Defensor`: No puede atacar
- `Destello`: Entra y ataca en el mismo turno
- `Indestructible`: No puede ser destruida
- `Prisa`: Puede atacar el turno que entra
- `ToqueMortal`: 1 punto de daño es letal
- `Vigilancia`: No se cansa al atacar
- `VinculoVital`: Ganancia de vida igual al daño
- `Vuelo`: Solo puede ser bloqueada por criaturas con vuelo

#### Habilidades (18 implementadas)
Divididas en categorías:

**Buffs:**
- `ContadoresHabilidad`: Añade contadores a la carta
- `CrearFichaHabilidad`: Crea fichas (tokens)
- `GirarHabilidad`: Gira cartas propias o del oponente
- `HincharDebilitarHabilidad`: Modifica ATK/DEF
- `SacrificarHabilidad`: Sacrifica cartas propias
- `VidaHabilidad`: Gana o pierde vida

**Combate:**
- `IndestructibleHabilidad`: Hace indestructible
- `NoAtacarHabilidad`: Impide atacar
- `NoBloquearHabilidad`: Impide bloquear
- `NoSerBloqueadaHabilidad`: No puede ser bloqueada
- `TodasDebenBloquearHabilidad`: Todas deben bloquear

**Daño:**
- `DanioHabilidad`: Daño directo
- `DestruirHabilidad`: Destruye cartas
- `LucharHabilidad`: Lucha contra otra criatura

**Gestión de Cartas:**
- `AdivinarHabilidad`: Mira cartas del mazo
- `BuscarBibliotecaHabilidad`: Busca en el mazo
- `CementerioHabilidad`: Interactúa con el cementerio
- `DescartarHabilidad`: Descarta cartas
- `DevolverManoHabilidad`: Devuelve a la mano
- `DevolverMazoHabilidad`: Devuelve al mazo
- `RevelarHabilidad`: Revela cartas
- `RobarHabilidad`: Roba cartas

#### Triggers (12 implementados)
Se ejecutan cuando ocurren eventos específicos:
- `CuandoAtaca`: Cuando la carta ataca
- `CuandoBloquea`: Cuando bloquea a un atacante
- `CuandoBloqueado`: Cuando es bloqueada
- `CuandoEntra`: Cuando entra en el campo
- `CuandoGanaVida`: Cuando gana vida
- `CuandoHaceDanio`: Cuando hace daño
- `CuandoMoneda`: Cuando se lanza una moneda
- `CuandoMuere`: Cuando muere/se destruye
- `CuandoOtorgaContador`: Cuando recibe contadores
- `CuandoOtraEntra`: Cuando otra criatura entra
- `CuandoPierdeVida`: Cuando pierde vida
- `CuandoQuitaContador`: Cuando pierde contadores
- `CuandoRecibeDanio`: Cuando recibe daño

### IA del Oponente (Parcialmente Implementada)
- ✅ IA puede jugar cartas de tierra
- ✅ IA puede girar cartas
- ⚠️ IA limitada - necesita lógica de toma de decisiones mejorada

### Sistema de Turnos (Estructura Preparada)
- ✅ Estructura base en `Partida.cs`
- ⚠️ Lógica de fases de turno lista para implementar

## Estructura del Proyecto

```
Assets/
├── Scripts/
│   ├── DragDrop/
│   │   ├── Arrastrar.cs           # Lógica de arrastre de cartas
│   │   └── DropZone.cs            # Zonas válidas para soltar cartas
│   │
│   ├── Gameplay/
│   │   ├── Carta.cs               # Clase principal de carta
│   │   ├── CartaDatabase.cs       # Base de datos de cartas
│   │   ├── CartasJugadas.cs       # Gestión de cartas en campo
│   │   ├── Contador.cs            # Sistema de contadores
│   │   ├── Jugador.cs             # Datos del jugador
│   │   ├── Partida.cs             # Controlador principal del juego
│   │   ├── UtilCartas.cs          # Utilidades para cartas
│   │   │
│   │   └── Efectos/
│   │       ├── Atributos/         # 13 atributos implementados
│   │       ├── Habilidades/       # 18 habilidades en 4 categorías
│   │       │   ├── Buffs/
│   │       │   ├── Combate/
│   │       │   ├── Danio/
│   │       │   └── Gestion_Cartas/
│   │       └── Triggers/          # 12+ triggers implementados
│   │
│   ├── DragDrop/
│   │   ├── Arrastrar.cs
│   │   └── DropZone.cs
│   │
│   ├── Menu/
│   │   ├── Animaciones.cs         # Animaciones de menú
│   │   ├── MenuInicio.cs          # Menú principal
│   │   ├── MenuPausa.cs           # Menú de pausa
│   │   └── Transicion.cs          # Transiciones entre escenas
│   │
│   └── UI/
│       ├── MensajeManager.cs      # Sistema de mensajes
│       ├── MostrarCarta.cs        # Visualización de cartas
│       ├── Reverso.cs             # Parte trasera de cartas
│       ├── Seleccionar.cs         # Sistema de selección
│       └── UIElements.cs          # Elementos de UI generales
│
├── Prefabs/                        # Prefabs reutilizables
├── Scenes/                         # Escenas del juego
├── Resources/                      # Recursos de juego
└── Editor/                         # Herramientas de editor
```

## Sistemas de Juego

### Sistema de Maná
- Cartas de tierra generan maná en la fase de maná
- Cálculo automático de costes totales de la mano
- Validación de recursos antes de jugar cartas

### Sistema de Vida
- Jugadores comienzan con 20 puntos de vida
- Control y actualización de vida en tiempo real
- Condición de victoria al llegar a 0 de vida

### Sistema de Efectos
Cada carta puede tener múltiples efectos que se crean dinámicamente:

```csharp
// Ejemplo de cómo se otorgan efectos a las cartas
carta.AnadirEfecto(new Amenaza());
carta.AnadirEfecto(new VidaHabilidad(2));
carta.AnadirEfecto(new CuandoEntra());
```

### Métodos Principales de Carta
- `Atacar()`: Realiza un ataque
- `RecibirDanio()`: Recibe daño
- `Bloquear()`: Bloquea a otra carta
- `DestruirCarta()`: Destruye la carta
- `TratarVida()`: Modifica vida del jugador

## Trabajo Pendiente

### 🔴 Crítico

#### Bloqueo de Cartas
- Completar lógica de atributo `Amenaza` 
- Implementar validación de bloqueadores en fase de combate
- Instantaneas
- Restar puntos de vida a criaturas bloqueadoras

#### IA del Oponente
- Mejorar lógica de selección de cartas a jugar
- Implementar selección de atacantes (por ahora ataca revisando de izaquierda a derecha su mazo).
- Implementar selección de bloqueadores
- Crear estrategia básica de IA

### 🟡 Importante

#### Integración Completa de Efectos en el Bucle
Estos efectos están implementados pero requieren integración en el bucle de juego:

**Atributos:**
- `Amenaza`: Requiere lógica de bloqueo en bucle
- `DaniaDosVeces`: Aplicar daño en momentos correctos del bucle
- `Arrolla`: Verificar daño excedente en cálculo
- `resto de atributos`: Aunque los 3 anteriores sean los principales, faltan más atributos.

**Habilidades:**
- `TodasDebenBloquearHabilidad`: Validar en fase de bloqueo
- `DestruirHabilidad`: Selección física del jugador
- `DescartarHabilidad`: Acceso a cartas del oponente
- `ElegirAccionHabilidad`: Interfaz de selección

**Triggers:**
- `CuandoAtaca`: Ejecutar cuando carta ataca (método listo)
- `CuandoBloquea`: Ejecutar cuando bloquea (requiere lógica de bloqueo)
- `CuandoHaceDanio`: Detectar cuando hace daño en bucle
- `CuandoOtraEntra`: Suscribirse a entradas de cartas
- `CuandoContador`: Bucle para aplicar efectos de contadores

### 🟢 Mejoras Futuras

#### Editor de Mazos
- Interfaz para crear y editar mazos
- Validación de límites de cartas
- Guardado/carga de mazos

#### Animaciones y Efectos Visuales
- Animaciones de ataque y daño
- Efectos visuales de habilidades
- Animaciones de entrada al campo

#### Sonido y Música
- Efectos de sonido para acciones
- Música de fondo
- Retroalimentación de audio

#### Multijugador/Online
- Sincronización de estado
- Sistema de turnos en red
- Chat in-game

#### Estadísticas y Progresión
- Sistema de logros
- Historial de partidas
- Estadísticas del jugador

## Notas Técnicas

### Convenciones de Código
- Métodos públicos: PascalCase
- Variables privadas: camelCase
- Constantes: UPPER_SNAKE_CASE
- Interfaces: Iprefix (ej: `IHabilidad`, `IAtributo`)

### Dependencias Principales
- Unity 2022 LTS o superior
- TextMeshPro para UI
- Sistema de eventos de Unity

### Configuración de Proyecto
- Resolution: 1920x1080
- Aspect Ratio: 16:9
- Render Pipeline: Universal Render Pipeline (URP)

### Debugging
- Usar `Debug.Log()` con mensajes descriptivos
- Sistema de mensajes en pantalla para eventos importantes
- Validación de estados antes de transiciones

### Próximos Pasos Recomendados

1. **Prioritario:** Implementar bucle de juego en `Partida.cs`
2. **Urgente:** Completar sistema de bloqueo
3. **Importante:** Mejorar IA del oponente
4. **Deseable:** Añadir animaciones visuales
5. **Futuro:** Implementar editor de mazos

### Contacto y Contribuciones
Para dudas sobre la implementación actual, revisar:
- Commit correspondiente en git para ver cambios específicos
- Comentarios en los archivos `.cs` principales
- Métodos de prueba en `Partida.cs` para entender flujo