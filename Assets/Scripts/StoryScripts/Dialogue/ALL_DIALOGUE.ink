// ============================================================
// ALL_DIALOGUE.ink  —  FICHERO MASTER
// ============================================================

EXTERNAL StartCombat(resumeKnot)

EXTERNAL FinishQuest(questId, won)

EXTERNAL SetOutfit(outfit)

EXTERNAL GiveItem(id)
EXTERNAL TakeItem(id)
EXTERNAL HasItem(id)
EXTERNAL HasGold(amount)

EXTERNAL SetFlag(id, value)
EXTERNAL GetFlag(id)
EXTERNAL RaiseWorldEvent(id)
EXTERNAL SpendGold(amount)

VAR outfit = "Caterina"
VAR required_outfit = "Any"
VAR combat_won = false

// ── Flags de historia globales (leídos/escritos por los Ink de misión) ──
// Se declaran aquí porque las VAR de Ink son globales a todo el árbol de INCLUDEs.
VAR setas_entregadas = false

// ---------------- PRIMARY NPCs (misiones M1–M15) ----------------
INCLUDE ../QuestSystem/NPCs/Primary NPCs/1_MadreSuperiora/MadreSuperiora.ink
INCLUDE ../QuestSystem/NPCs/Primary NPCs/2_Guarda 14/Guarda.ink
INCLUDE ../QuestSystem/NPCs/Primary NPCs/3_Molinero/Molinero.ink
INCLUDE ../QuestSystem/NPCs/Primary NPCs/4_Destripat/Destripat.ink
INCLUDE ../QuestSystem/NPCs/Primary NPCs/5_Ermitaño/Ermitano.ink
INCLUDE ../QuestSystem/NPCs/Primary NPCs/6_1_GuardaMarqués/GuardaMarques.ink
INCLUDE ../QuestSystem/NPCs/Primary NPCs/6_2_Marqués/Marques.ink
INCLUDE ../QuestSystem/NPCs/Primary NPCs/7_Damasco/Sepulturero.ink
INCLUDE ../QuestSystem/NPCs/Primary NPCs/8_Levante/GuardaLevante.ink
INCLUDE ../QuestSystem/NPCs/Primary NPCs/9_DMartin/DMartin.ink
INCLUDE ../QuestSystem/NPCs/Primary NPCs/10_Prostíbulo/Paca.ink
INCLUDE ../QuestSystem/NPCs/Primary NPCs/10_2_Inés/Ines.ink
INCLUDE ../QuestSystem/NPCs/Primary NPCs/11_1_Hospital/SorMaria.ink
INCLUDE ../QuestSystem/NPCs/Primary NPCs/11_2_Hospital/FrayArsacio.ink
INCLUDE ../QuestSystem/NPCs/Primary NPCs/12_Terciopelero/Terciopelero.ink
INCLUDE ../QuestSystem/NPCs/Primary NPCs/13_NiñoDMartín/DMartinFinal.ink
INCLUDE ../QuestSystem/NPCs/Primary NPCs/14_TorneoMercado/OrganizadorMercado.ink
INCLUDE ../QuestSystem/NPCs/Primary NPCs/15_TorneoPlazaMayor/OrganizadorPlazaMayor.ink

// ---------------- OPTIONAL NPCs (farming post-M14) ----------------
// TODO: Comentados temporalmente hasta actualizar todos los Inks al nuevo patrón
// INCLUDE ../QuestSystem/NPCs/Optional NPCs/1_Chantre/Chantre.ink
// INCLUDE ../QuestSystem/NPCs/Optional NPCs/2_Catedrático/Catedratico.ink
// INCLUDE ../QuestSystem/NPCs/Optional NPCs/3_Jurado/Jurado.ink
// INCLUDE ../QuestSystem/NPCs/Optional NPCs/4_CapitánTornabuoni/CapitanTornabuoni.ink

// ---------------- SECONDARY NPCs (mundo abierto repetible) ----------------
// TODO: Comentados temporalmente hasta actualizar todos los Inks al nuevo patrón
// INCLUDE ../QuestSystem/NPCs/Secondary NPCs/1_Lagartijo/Lagartijo.ink
// INCLUDE ../QuestSystem/NPCs/Secondary NPCs/2_Verrugas/Verrugas.ink
// INCLUDE ../QuestSystem/NPCs/Secondary NPCs/3_Tropezones/Tropezones.ink
// INCLUDE ../QuestSystem/NPCs/Secondary NPCs/4_PereElTuerto/PereElTuerto.ink
// INCLUDE ../QuestSystem/NPCs/Secondary NPCs/5_AntoniTresDientes/AntoniTresDientes.ink
//INCLUDE ../QuestSystem/NPCs/Secondary NPCs/6_BatisteCaraRata/BatisteCaraRata.ink
//INCLUDE ../QuestSystem/NPCs/Secondary NPCs/7_JoanMedioHuevo/JoanMedioHuevo.ink
//INCLUDE ../QuestSystem/NPCs/Secondary NPCs/8_PeponElChepa/PeponElChepa.ink
//INCLUDE ../QuestSystem/NPCs/Secondary NPCs/9_GarcíaDeLomotieso/GarciaDeLomotieso.ink
//INCLUDE ../QuestSystem/NPCs/Secondary NPCs/10_LopeDeAguasMuertas/LopeDeAguasMuertas.ink
//INCLUDE ../QuestSystem/NPCs/Secondary NPCs/11_PeroDelPeral/PeroDelPeral.ink
//INCLUDE ../QuestSystem/NPCs/Secondary NPCs/12_CaballeroAndante1/CaballeroAndante1.ink
//INCLUDE ../QuestSystem/NPCs/Secondary NPCs/13_CaballeroAndante2/CaballeroAndante2.ink
//INCLUDE ../QuestSystem/NPCs/Secondary NPCs/14_CaballeroAndante3/CaballeroAndante3.ink
//INCLUDE ../QuestSystem/NPCs/Secondary NPCs/15_CaballeroAndante4/CaballeroAndante4.ink
//INCLUDE ../QuestSystem/NPCs/Secondary NPCs/16_GuardaTorre/GuardaTorre.ink
//INCLUDE ../QuestSystem/NPCs/Secondary NPCs/17_ErmitañoExtra/ErmitanoExtra.ink
//INCLUDE ../QuestSystem/NPCs/Secondary NPCs/18_Aldeano/Aldeano.ink
//INCLUDE ../QuestSystem/NPCs/Secondary NPCs/19_CombateGenerico/CombateGenerico.ink
