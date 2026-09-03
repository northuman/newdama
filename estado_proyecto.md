# Documentación de Estado: `StoryScene.unity`

**Fecha:** Junio 2026 | **Proyecto:** NewDama (Unity, URP)  
**Autor:** Análisis Automatizado | **Propósito:** Definición de sistemas, pendientes y errores

---

## Tabla de Contenidos

1. [Visión General](#1-visión-general)
2. [Jerarquía de GameObjects](#2-jerarquía-de-gameobjects-en-escena)
3. [Scripts del Proyecto](#3-scripts-del-proyecto--estado-por-sistema)
4. [Archivos Ink](#4-archivos-ink--duplicados)
5. [Diagrama de Flujo](#5-diagrama-de-flujo-del-sistema-narrativo)
6. [Problemas Detectados](#6-resumen-de-problemas-detectados)
7. [Roadmap de Desarrollo](#7-por-dónde-continuar-prioridades)

---

## 1. Visión General

\`\`\`StoryScene\`\` es la escena de campaña del juego NewDama. El jugador controla a **Caterina** en un entorno 3D rural con aldea, monasterio y ciudad texturizada. El sistema permite:

- **Exploración 3D**: Click-to-move sobre un terreno con NavMesh
- **Interacción Narrativa**: Diálogos basados en Ink que ramifican a misiones
- **Sistema de Quests**: Máquina de estados (REQUIREMENTS_NOT_MET → CAN_START → IN_PROGRESS → CAN_FINISH → FINISHED)
- **Card Game integrado**: Partidas de cartas tácticas como mécanica de resolución de conflictos
- **Gestión de Eventos**: Bus de eventos centralizado para desacoplamiento

**Acceso**: Desde \`MenuInicio\` → botón "Campaña" → carga \`StoryScene\`

---

## 2. Jerarquía de GameObjects en Escena

### 2.1 Entorno 3D (\`Environment\`)

| GameObject | Componentes Relevantes | Notas |
|---|---|---|
| \`Grupo de Terrenos\` | Terrain, NavMeshObstacle | Layer 6; escala ×2; NavMesh baked |
| \`Zona rural y Ciudad\` | Terrain, TerrainCollider | Terreno principal |
| \`Edificios\` | Transform (padre) | 17 prefabs: Ermita, CasaPaja×2, CasaRuralPiedra×2, CasaRural3/4, ciudad_texturizada, Monasterio, Cortijo, Venta |
| \`Agua\` | MeshRenderer, MeshFilter, MeshCollider | Layer 6; malla de agua |
| \`Post procesado\` | Volume (URP) | Perfil de post-procesado global |

### 2.2 Personaje y Cámara

| GameObject | Componentes Relevantes | Estado |
|---|---|---|
| \`Player\` | BoxCollider, NavMeshAgent, Animator, Rigidbody, \`PlayerStats\` | Tag: Player; pos (204, 6, 100); hijo: modelo \`dama\` |
| \`Main Camera\` | Camera, AudioListener, \`CameraFollow\` | FOV 50°; **⚠️ Script ref. rota** |
| \`Directional Light\` | Light (URP) | Rot (50°, -30°); intensidad 1.5 |

### 2.3 UI de Diálogo (\`Canvas - UI\`)

| GameObject | Componentes | Estado |
|---|---|---|
| \`Canvas - UI\` | Canvas (Overlay), CanvasScaler (1920×1080), GraphicRaycaster | Activo |
| \`Dialogue - UI\` | \`DialoguePanelUI\` | **⚠️ Ref. como \`DialogueUIManager\` en escena** |
| \`ContentParent\` | RectTransform | Se activa/desactiva con diálogos |
| \`Background\` | Image | Deshabilitado (sin sprite) |
| \`NPC\` | Image | Deshabilitado (sin sprite) |
| \`Player\` | Image | Deshabilitado (sin sprite) |
| \`DialogueText\` | TextMeshProUGUI | Font Arial, size 36 |
| \`PresionarEspacioBillboard\` | Image, CanvasGroup | Alpha 0; hint visual |

### 2.4 Card Game UI (\`CardGame\` Canvas)

**Estado**: Desactivado por defecto. Se activa cuando Ink llama \`~ StartQuest()\`.

- **Mano**: HorizontalLayoutGroup, spacing -92
- **Zonas**: Tierra, Batalla, Cementerio, Pila (×2: jugador + oponente)
- **Estadísticas**: Vida, Maná ×4 colores (Jugador + Oponente)
- Prefabs de cartas con imagen, border, nombre, descripción, símbolo

### 2.5 Managers (\`GameManagers\`)

| GameObject | Script | Propósito |
|---|---|---|
| \`GameManagers\` | — | Contenedor de managers |
| \`DialogueManager\` | \`DialogueManager.cs\` | Motor Ink, serializa \`jsonInk\` |
| \`Player\` (dentro) | \`PlayerStats.cs\` | Stats narrativas (Level, HP, Gold, Speed) |
| \`Oponente\` | — | Oponente del card game |

### 2.6 Quest Points

| GameObject | Script | Posición | Propósito |
|---|---|---|---|
| \`GuardaQuestPoint\` | \`QuestPoint\` | (193.5, 5.69, 125.7) | Quest "Guarda" |
| \`MadreSuperioraQuestPoint\` | \`QuestPoint\` | (206.79, 5.69, 103.77) | Quest "MadreSuperiora" |

### 2.7 Event Systems — **⚠️ DUPLICADO**

| GameObject | Estado | Análisis |
|---|---|---|
| \`EventSystem\` | **Activo** | Principal (StandaloneInputModule, GraphicRaycaster) |
| \`PartidaEventSystem\` | **Activo** | Para card game — debería estar desactivado o eliminado |

**Problema**: Dos EventSystems activos → warnings en consola, comportamiento de input imprevisible.

---

## 3. Scripts del Proyecto — Estado por Sistema

### 3.1 Sistema de Diálogo ✅ Funcional

#### \`DialogueManager.cs\`
- Carga Ink compilado (\`jsonInk\`) en Awake → \`new Story(jsonInk.text)\`
- Escucha \`GameEventsManager.dialogueEvents.OnDialogueStart\`
- Navega al knot, deshabilita movimiento del jugador
- Spacebar avanza líneas; al terminar, restaura movimiento
- Vincula funciones externas vía \`InkExternalFunctions\`

#### \`InkExternalFunctions.cs\`
- Bindea 3 funciones Ink → C#: \`StartQuest\`, \`AdvanceQuest\`, \`FinishQuest\`
- \`StartQuest(id)\`: dispara \`questEvents.StartQuest()\` + activa Canvas de CardGame
- \`FinishQuest(id)\`: dispara \`questEvents.FinishQuest()\` + desactiva Canvas de CardGame

#### \`DialoguePanelUI.cs\`
- Escucha eventos: \`OnDialogueStarted\` / \`OnDialogueEnded\` / \`OnDisplayDialogue\`
- Muestra/oculta \`ContentParent\` y actualiza \`dialogueText\`
- Alinea a derecha si contiene "Caterina:", a izquierda si no
- **⚠️ Línea duplicada**: \`dialogueText.text = line;\` aparece dos veces

#### \`DialogueEvents.cs\` (POCO)
- 4 eventos públicos: \`OnDialogueStart\`, \`OnDialogueStarted\`, \`OnDialogueEnded\`, \`OnDisplayDialogue\`

---

### 3.2 Sistema de Eventos (Bus Central) ✅ Funcional

#### \`GameEventsManager.cs\`
- **Singleton**: instancia única en \`GameEventsManager.instance\`
- Instancia \`QuestEvents\` y \`DialogueEvents\` en Awake
- Punto central de comunicación entre sistemas desacoplados

#### \`QuestEvents.cs\`
- 4 eventos: \`OnStartQuest(string)\`, \`OnAdvanceQuest(string)\`, \`OnFinishQuest(string)\`, \`OnQuestStateChange(Quest)\`

#### \`InputEvents.cs\`
- **⚠️ Vacío**: solo Start() y Update() sin implementación
- Nunca se utiliza

#### \`InputContext.cs\`
- Enum: \`DEFAULT\`, \`DIALOGUE\`
- **Nunca se usa** en ningún script

---

### 3.3 Sistema de Quests ⚠️ En Desarrollo

#### \`QuestManager.cs\`
**Responsabilidades:**
- Carga todos los \`QuestInfoSO\` desde \`Resources/StoryResources/Quests/\`
- Gestiona estados de quests: máquina de 5 estados
- \`CheckRequirementsMet()\`: verifica nivel y requisitos previos
- \`StartQuest()\`: instancia step prefab, cambia estado a IN_PROGRESS
- \`AdvanceQuest()\`: pasa al siguiente step o marca CAN_FINISH
- \`FinishQuest()\`: reclama recompensas, marca FINISHED

**Problemas:**
- \`ClaimRewards()\` solo hace \`playerStats.Level += 1\` — gold y experience están comentados
- **No llama a \`LevelManager.levelUp()\`** → \`currentPlayerLevel\` nunca se actualiza en runtime
- Referencias a \`LevelManager.instance\` asumen que existe (puede ser null)

#### \`Quest.cs\`
- Envuelve \`QuestInfoSO\` a runtime
- Gestiona \`currentStepIndex\` y transiciones de steps
- \`InstantiateCurrentQuestStep()\` crea el prefab como hijo de QuestManager

#### \`QuestInfoSO.cs\`
- **ScriptableObject** con:
  - \`id\` (auto-asignado al nombre del asset)
  - \`questName\`, \`levelRequirement\`, \`questPrerequisites[]\`, \`questSteps[]\`
  - \`goldReward\`, \`experienceReward\`, \`cardsReward[]\`

#### \`QuestPoint.cs\`
**Funcionalidad:**
- Collider esférico, detecta proximidad del jugador
- Spacebar: si tiene \`knotToStartDialogue\` → lanza diálogo; si no → inicia/finaliza quest
- Se suscribe a \`OnQuestStateChange\` del bus de eventos

**🐛 Bug Crítico:**
\`\`\`csharp
// INCORRECTO — compara id consigo mismo, SIEMPRE TRUE:
if (quest.questInfo.id.Equals(quest.questInfo.id))

// CORRECTO:
if (quest.questInfo.id.Equals(questId))
\`\`\`
**Consecuencia**: todos los QuestPoints actualizan su estado simultáneamente con cualquier cambio de quest, sin filtrar por ID. Causa:
- Múltiples QuestPoints activos al mismo tiempo
- Estados incorrectos para quests no relacionadas
- Comportamiento impredecible en interacciones

#### \`QuestStep.cs\`
- Clase abstracta base
- \`FinishQuestStep()\`: dispara \`questEvents.AdvanceQuest(questId)\`, destruye el GameObject

#### \`MadreSuperioraStep.cs\`
- Implementa \`QuestStep\`
- Completa el step cuando el jugador pulsa **Escape**
- **⚠️ Solución temporal de prueba**, sin lógica real
- **Conflicto con MenuPausa**: Escape abre el menú de pausa Y completa el step

#### \`GuardaStep.cs\`
- Implementa \`QuestStep\`
- Método \`EndOfQuest()\` solo hace \`Debug.Log\`, nunca llama a \`FinishQuestStep()\`
- **⚠️ El step nunca se completa automáticamente** — requiere lógica de victoria en partida

#### \`QuestIcon.cs\`
- CanvasGroup con fade in/out cuando el jugador entra/sale del trigger
- Escucha \`input.Main.Submit.performed\` (Space)
- **⚠️ Conflicto**: QuestPoint también escucha Submit → doble disparo en la misma zona

---

### 3.4 LevelManager 🔴 Incompleto

#### \`LevelManager.cs\`
- **Singleton** con único método público: \`levelUp(int level)\`
- Dispara evento \`onLevelChange\`

**Problemas graves:**
- **No almacena el nivel actual** — si algo pregunta "¿nivel actual?" no puede responder
- **Nunca se llama en runtime** → \`QuestManager.ClaimRewards()\` actualiza \`PlayerStats.Level\` pero no invoca \`LevelManager.levelUp()\`
- En \`QuestManager\`, el nivel se lee de \`PlayerStats\` en Awake y nunca se re-sincroniza
- Resultado: el sistema de prerequisitos de quests se basa en un nivel que puede estar desincronizado

---

### 3.5 Sistema de Jugador ⚠️ Mínimo

#### \`PlayerStats.cs\`
- **Solo propiedades**: \`Level\`, \`MaxHealth\`, \`CurrentHealth\`, \`Gold\`, \`Speed\`
- Sin lógica: no hay curación, persistencia, ni conexión con card game
- Usada por \`QuestManager\` para verificar requisitos

#### \`PlayerController.cs\`
- Click-to-move: Raycast desde cámara → \`NavMeshAgent.destination\`
- Animaciones: Idle/Walk según velocidad del agente
- \`Alive\` property: deshabilita movimiento durante diálogos
- Valida NavMesh antes de asignar destino

#### \`CameraController.cs\`
- Follow suave con \`Vector3.Lerp\`
- Aplica offset configurable desde el target
- **⚠️ En escena se llama \`CameraFollow\` pero el archivo es \`CameraController.cs\`** → posible referencia rota

---

### 3.6 Sistema de Cartas ✅ Implementado Completo (desconectado de narrativa)

#### \`Partida.cs\` (~700 líneas)
**Funciones:**
- Gestiona flujo de turno: 6 fases (INICIO, PRINCIPAL_1, BLOQUEO, COMBATE, PRINCIPAL_2, FINAL)
- Mana, robo de cartas, juego de tierras
- Lógica de combate: bloqueo, asignación de daño, efectos especiales
- IA del oponente (corrutina \`CerebroIA\`)
- Condiciones de victoria/derrota
- Debug: K para dañar oponente, L para dañarse a sí mismo

**Problema:**
- **No notifica al sistema de quests el resultado** (victoria/derrota)
- No hay forma de que un quest sepa si ganó o perdió la partida

#### \`Jugador.cs\`
- Datos: vida, maná[], baraja, mano, cementerio, pila, tierras, batalla
- Roba cartas con animación (corrutina \`RobarCartasSecuencialmente\`)
- \`RellenarBaraja()\`: 40 cartas (20 tierras + 20 no-tierras) aleatorias

#### \`Carta.cs\`
- Tipos: TIERRA, CRIATURA, CONJURO, INSTANTANEO, ARTEFACTO, ENCANTAMIENTO
- Colores: INCOLORO, BLANCO, NEGRO, ROJO, VERDE
- Listas de \`IAtributo\`, \`IHabilidad\`, \`ITrigger\`
- ID autoincrementable

#### \`CartasJugadas.cs\`
- Instancia en juego: fuerzaActual, resistenciaActual, girada, mareo, defensor, vuelo, indestructible, toqueMortal, etc.
- Métodos: \`Atacar()\`, \`RecibirDanio()\`, \`DestruirCarta()\`, \`Bloquear()\`, \`TratarVida()\`
- Ejecuta atributos al inicializar

#### \`CartaDatabase.cs\`
- Carga cartas desde TextAsset con formato:
  \`\`\`
  nombre#tipo#rareza#coste#color#fuerza#resistencia#descripcion#flavour
  \`\`\`

#### \`UtilCartas.cs\`
- Extensiones: \`Randomizar<T>()\`, \`Intercambio<T>()\`, \`NumAleatorio()\`, \`ColorearCarta()\`

#### **Efectos de Cartas (35+ scripts)**
- **Atributos** (11 scripts): Prisa, Amenaza, Vuelo, Indestructible, ToqueMortal, Vigilancia, Arrolla, VínculoVital, Destello, Defensor, NoSerBloqueada
- **Triggers** (14 scripts): CuandoAtaca, CuandoMuere, CuandoEntra, etc.
- **Habilidades** (20 scripts): gestión de cartas, buffs, combate, daño

---

### 3.7 Input ✅ Funcional

#### \`CustomActions.cs\`
- **Auto-generado** por Unity Input System desde \`CustomActions.inputactions\`
- Mapa \`Main\` con 3 acciones:
  - \`Move\` → click izquierdo del ratón
  - \`Submit\` → Spacebar
  - \`Escape\` → tecla Escape

---

### 3.8 Menú y Transiciones

#### \`MenuPausa.cs\`
- Escape abre/cierra menú de pausa
- \`Time.timeScale = 0/1\`
- **⚠️ Conflicto con MadreSuperioraStep** que también usa Escape

#### \`MenuInicio.cs\`
- Botones: Campaña → \`StoryScene\`, Arena → \`Juego\`, Opciones, Salir
- Transición animada

#### \`Transicion.cs\`
- Transición visual entre escenas

#### \`SceneLoader.cs\`
- Método \`cargarPartida()\` **privado y nunca se llama** → código muerto

---

## 4. Archivos Ink — Duplicados

| Archivo | EXTERNAL | Estado |
|---|---|---|
| \`Assets/Scripts/StoryScripts/Dialogue/ALL_DIALOGUE.ink\` | ✅ StartQuest, AdvanceQuest, FinishQuest | **ACTIVO y CORRECTO** |
| \`Assets/Scripts/StoryScripts/InkDialogue/ALL_DIALOGUE.ink\` | ❌ Sin EXTERNAL | **OBSOLETO — incompleto, sin knot Guarda** |

**Problema**: Dos versiones del archivo Ink. La carpeta \`InkDialogue/\` es residuo.

**Contenido del Ink activo:**
- Knot \`== MadreSuperiora ==\`: diálogo entre Caterina y la Madre Superiora, al terminar llama \`~ StartQuest("MadreSuperiora")\`
- Knot \`== Guarda ==\`: diálogo entre Caterina y el Guarda, al terminar llama \`~ StartQuest("Guarda")\`

---

## 5. Diagrama de Flujo del Sistema Narrativo

\`\`\`
┌─────────────────────────────────────────────────────────────┐
│ JUGADOR SE ACERCA A QUESTPOINT (trigger esférico)           │
└────────────────────┬────────────────────────────────────────┘
                     │
                     ▼
            [SPACEBAR] Submit
                     │
        ┌────────────┴────────────┐
        │                         │
        ▼                         ▼
¿knot configurado?      ¿quest sin diálogo?
        │                         │
     SÍ│                         │ SÍ
        │                         │
        ▼                         ▼
StartDialogue(knot)   questEvents.StartQuest(id)
        │             o FinishQuest(id) directamente
        │
        ▼
DialogueManager recibe OnDialogueStart
        │
        ▼
story.ChoosePathString(knot)
        │
        ▼
[SPACEBAR] avanza líneas → story.Continue()
        │
        ▼
¿Ink llama funciones externas?
        │
        ├── ~ StartQuest("ID")
        │       │
        │       ▼
        │   InkExternalFunctions.StartQuest(id)
        │       │
        │       ├── questEvents.StartQuest(id)
        │       └── CardGame.SetActive(true) ──► PARTIDA COMIENZA
        │
        ├── ~ AdvanceQuest("ID")
        │       │
        │       └── questEvents.AdvanceQuest(id)
        │
        └── ~ FinishQuest("ID")
                │
                └── questEvents.FinishQuest(id)
                    │
                    └── CardGame.SetActive(false)

┌───────────────────────────────────────────────┐
│ FLUJO DE QUEST STATE MACHINE                  │
└───────────────────────────────────────────────┘

REQUIREMENTS_NOT_MET
        │
        ▼
(Update loop verifica requisitos)
¿Level suficiente? ¿Prereqs completados?
        │
        ▼
CAN_START
        │
    [Space]
        │
        ▼
questEvents.StartQuest(id)
        │
        ▼
QuestManager.StartQuest()
        │
        ├── Quest.InstantiateCurrentQuestStep()
        │       │
        │       └── prefab.GetComponent<QuestStep>().Initialize(questId)
        │
        └── Cambiar estado a IN_PROGRESS

IN_PROGRESS
        │
    QuestStep espera condición de completado
        │
        ▼
[Escape] MadreSuperioraStep.EscPressed()
    o [Victoria partida] GuardaStep.[??]
        │
        ▼
QuestStep.FinishQuestStep()
        │
        ├── questEvents.AdvanceQuest(questId)
        └── Destroy(gameObject)

QuestManager.AdvanceQuest()
        │
        ├── MoveToNextStep()
        │
        ▼
¿CurrentStepExists()?
        │
    ├── SÍ: InstantiateCurrentQuestStep()
    │       └── Estado sigue IN_PROGRESS
    │
    └── NO: Cambiar estado a CAN_FINISH

CAN_FINISH
        │
    [Space en QuestPoint]
        │
        ▼
questEvents.FinishQuest(id)
        │
        ▼
QuestManager.FinishQuest()
        │
        ├── ClaimRewards()
        │       │
        │       └── playerStats.Level += 1
        │           [⚠️ LevelManager.levelUp() NO se llama]
        │
        └── Cambiar estado a FINISHED

FINISHED
\`\`\`

---

## 6. Resumen de Problemas Detectados

### 🔴 Errores Críticos (Bloquean funcionalidad)

| # | Problema | Ubicación | Impacto | Severidad |
|---|---|---|---|---|
| **1** | **EventSystem duplicado** — \`EventSystem\` y \`PartidaEventSystem\` ambos activos | StoryScene.unity | Warnings en consola, input impredecible | CRÍTICA |
| **2** | **Bug en QuestPoint** — \`quest.questInfo.id.Equals(quest.questInfo.id)\` siempre TRUE | \`QuestPoint.cs\` línea ~65 | Todos los QuestPoints se actualizan con cualquier quest | CRÍTICA |
| **3** | **Referencia script rota** — \`CameraFollow\` en escena vs. \`CameraController.cs\` en disco | StoryScene.unity + CameraController.cs | Cámara puede no funcionar | ALTA |
| **4** | **Referencia script rota** — \`DialogueUIManager\` en escena vs. \`DialoguePanelUI.cs\` en disco | StoryScene.unity + DialoguePanelUI.cs | Panel de diálogo puede no responder | ALTA |
| **5** | **Conflicto Escape** — ambos \`MadreSuperioraStep\` y \`MenuPausa\` escuchan Escape | MadreSuperioraStep.cs + MenuPausa.cs | Escape abre pausa Y completa quest step simultáneamente | MEDIA |
| **6** | **Doble asignación de texto** — \`dialogueText.text = line;\` dos veces | \`DialoguePanelUI.cs\` línea ~50 | Inocuo pero redundante | BAJA |
| **7** | **Double Submit disparado** — QuestPoint y QuestIcon ambos en \`input.Main.Submit.performed\` | QuestPoint.cs + QuestIcon.cs | Spacebar ejecuta dos acciones simultáneamente | MEDIA |

### 🟡 Incompleto / En Pañales (Funciona pero requiere expansión)

| # | Sistema | Componente | Estado Actual | Pendiente | Criticidad |
|---|---|---|---|---|---|
| **1** | Quest | LevelManager | Solo dispara evento | Guardar nivel actual; conectar con ClaimRewards() | ALTA |
| **2** | Quest | GuardaStep | \`EndOfQuest()\` solo loguea | Conectar resultado de Partida (victoria) → \`FinishQuestStep()\` | ALTA |
| **3** | Quest | MadreSuperioraStep | Escape como placeholder | Implementar lógica real (victoria en partida) | ALTA |
| **4** | Card Game | Partida | Sin notificación de resultado | Comunicar victoria/derrota al sistema de quests | ALTA |
| **5** | Player | PlayerStats | Solo propiedades | Conectar con resultado del card game (Gold, Health) | MEDIA |
| **6** | Quest | Recompensas | Gold/Experience comentados | Implementar cuando PlayerStats esté completo | MEDIA |
| **7** | UI | Diálogo | Imágenes deshabilitadas, sin sprites | Asignar sprites por personaje según knot | MEDIA |
| **8** | Scene | SceneLoader | \`cargarPartida()\` privada y sin llamadas | Evaluar necesidad o eliminar | BAJA |
| **9** | Events | InputEvents.cs | Vacío | Implementar o eliminar | BAJA |
| **10** | Events | InputContext enum | Sin usar | Determinar propósito o eliminar | BAJA |
| **11** | Ink | InkDialogue/ | Versión duplicada y obsoleta | Eliminar carpeta completa | BAJA |

### 🟢 Funcional y Completado

✅ Motor de diálogo Ink (avance de texto, bloqueo de movimiento, reset)  
✅ Bus de eventos GameEventsManager (QuestEvents, DialogueEvents)  
✅ Sistema de cartas completo (Partida, combate, IA, 35+ efectos)  
✅ Movimiento del jugador (click-to-move, NavMesh, animaciones)  
✅ Máquina de estados de quests (QuestManager, Quest, QuestInfoSO)  
✅ Input System (CustomActions, 3 acciones mapeadas)  
✅ Estructura 3D del mundo (terreno, edificios, post-procesado, NavMesh)  
✅ Menú de inicio y transiciones de escena

---

## 7. Por Dónde Continuar (Prioridades)

### Fase 1 — Arreglar errores críticos (1-2 días)

**Objetivo:** Escena estable sin errores de referencia.

1. **[URGENTE] Eliminar EventSystem duplicado**
   - En StoryScene, eliminar o desactivar \`PartidaEventSystem\`
   - Si el card game necesita su propio EventSystem, cargarlo additively junto con la escena

2. **[URGENTE] Corregir bug de QuestPoint**
   - Archivo: \`QuestPoint.cs\`, método \`OnQuestStateChange\`, línea ~65
   - Cambiar: \`if (quest.questInfo.id.Equals(quest.questInfo.id))\`
   - Por: \`if (quest.questInfo.id.Equals(questId))\`

3. **[URGENTE] Renombrar o actualizar referencias de scripts**
   - \`CameraController.cs\` → renombrar a \`CameraFollow.cs\` O actualizar referencia en escena
   - \`DialoguePanelUI.cs\` → renombrar a \`DialogueUIManager.cs\` O actualizar referencia en escena

4. **[BAJA PRIORIDAD] Limpiar código duplicado**
   - Eliminar línea duplicada en \`DialoguePanelUI.DisplayDialogue()\` (línea ~50)
   - Eliminar carpeta \`Assets/Scripts/StoryScripts/InkDialogue/\` (Ink obsoleto)
   - Eliminar \`InputEvents.cs\` y \`InputContext.cs\` (sin usar)
   - Hacer \`SceneLoader.cargarPartida()\` public o eliminar

### Fase 2 — Conectar Card Game con Quest System (2-3 días)

**Objetivo:** Las partidas de cartas resuelven quests automáticamente.

1. **Pasar questId a Partida**
   - Modificar \`InkExternalFunctions.StartQuest()\` para guardar el questId activo globalmente
   - Pasar questId a \`CardGame\` cuando se activa

2. **Notificar resultado de partida**
   - En \`Partida\`, cuando hay victoria: llamar \`GameEventsManager.instance.questEvents.AdvanceQuest(questId)\`
   - En \`Partida\`, cuando hay derrota: opcionalmente \`ResetQuest(questId)\` o mostrar retry UI

3. **Implementar steps reales**
   - \`MadreSuperioraStep\`: destruir cuando victoria detectada
   - \`GuardaStep\`: destruir cuando victoria detectada
   - Opcional: agregar rewards (cards, gold) según el resultado

### Fase 3 — Completar LevelManager (1 día)

**Objetivo:** Sistema de niveles funcionalmente completo.

1. **Agregar estado al LevelManager**
   \`\`\`csharp
   public int currentLevel { get; private set; } = 1;
   
   public void levelUp(int newLevel)
   {
       currentLevel = newLevel;
       onLevelChange?.Invoke(newLevel);
   }
   \`\`\`

2. **Conectar con QuestManager**
   - En \`QuestManager.ClaimRewards()\`, después de \`playerStats.Level += 1\`:
     \`\`\`csharp
     LevelManager.instance.levelUp(playerStats.Level);
     \`\`\`

3. **Re-sincronizar en Update**
   - En \`QuestManager.Update()\`, verificar si \`currentPlayerLevel != playerStats.Level\`
   - Si difiere, actualizar y re-evaluar requisitos de quests

### Fase 4 — Enriquecer UI de Diálogo (1 día)

**Objetivo:** Diálogos con sprites y animaciones.

1. **Asignar sprites por personaje**
   - Crear un diccionario: \`personajeName → sprite\`
   - En \`DialoguePanelUI.DisplayDialogue()\`, extraer el nombre (antes de \`:\`)
   - Activar Image correspondiente con el sprite

2. **Animaciones de entrada**
   - Fade in/out suave (CanvasGroup.alpha)
   - Slide in desde laterales

### Fase 5 — Resolver conflictos de Input (1 día)

**Objetivo:** Cada tecla hace una sola cosa.

1. **Separar Escape de Submit**
   - \`MadreSuperioraStep\` debería escuchar un evento diferente (ej. victoria en Partida) en lugar de Escape
   - Dejar Escape únicamente para MenuPausa

2. **Desactivar QuestIcon.Submit**
   - QuestIcon debería solo hacer fade visual
   - El Submit debería procesarlo únicamente QuestPoint

### Fase 6 — Persistencia y Progreso (2 días)

**Objetivo:** Guardar/cargar estado de quests y jugador.

1. **Serializar estado de quests**
   - JSON con: \`questId\`, \`questState\`, \`currentStepIndex\`

2. **Guardar PlayerStats**
   - JSON con: \`Level\`, \`Gold\`, \`Health\`, \`Cards\`

3. **Sistema de saves**
   - Slot-based, auto-save, load-on-start

---

## Métricas del Proyecto

| Métrica | Valor | Notas |
|---|---|---|
| **Scripts totales en StoryScene** | 23 ficheros | Excluyendo efectos de cartas |
| **Efectos de cartas** | 35+ | Atributos (11), Triggers (14), Habilidades (20) |
| **Líneas de código (Partida.cs)** | ~700 | Mayor script del proyecto |
| **GameObjects en escena** | ~40 | Terreno, edificios, UI, managers |
| **Eventos en el bus** | 7 | 4 en QuestEvents, 3 en DialogueEvents |
| **Estados de Quest** | 5 | REQUIREMENTS_NOT_MET, CAN_START, IN_PROGRESS, CAN_FINISH, FINISHED |
| **Fases de turno** | 6 | INICIO, PRINCIPAL_1, BLOQUEO, COMBATE, PRINCIPAL_2, FINAL |
| **Colores de maná** | 5 | INCOLORO, BLANCO, NEGRO, ROJO, VERDE |

---

## Conclusión

**NewDama** es un proyecto ambicioso con arquitectura bien pensada (bus de eventos, máquina de estados, separación de concerns). El sistema de cartas está **95% completo**, el motor narrativo Ink está **funcional**, y el sistema de quests está **en esqueleto con bugs solucionables**.

**Bloqueantes actuales:**
1. Bugs de referencia de scripts (3 minutos para arreglar)
2. EventSystem duplicado (1 minuto para eliminar)
3. Bug de QuestPoint (5 minutos para corregir)
4. Desconexión entre Partida y Quest System (2-3 horas de integración)

**Una vez resueltos estos puntos**, el sistema será **jugable end-to-end**: exploración → diálogo → partida de cartas → quest completa → reward → siguiente quest.

**Estimación total de trabajo:**
- **Fase 1 (Arreglar bugs)**: 30 minutos
- **Fase 2 (Card Game ↔ Quest)**: 8-16 horas
- **Fase 3-6 (Completar sistemas)**: 15-20 horas

**Total: 2-3 semanas** de desarrollo centrado en estas áreas.

---