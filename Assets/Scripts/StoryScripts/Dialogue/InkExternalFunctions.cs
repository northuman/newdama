using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkExternalFunctions
{
    private PlayerStats playerStats;

    public InkExternalFunctions(PlayerStats playerStats)
    {
        this.playerStats = playerStats;
    }
    public void Bind(Story story)
    {
        story.BindExternalFunction("StartCombat", (string resumeKnot) => StartCombat(resumeKnot));
        story.BindExternalFunction("FinishQuest", (string questId, bool won) => FinishQuest(questId, won));
        story.BindExternalFunction("SetOutfit", (string outfit) => SetOutfit(outfit));
        story.BindExternalFunction("GiveItem", (string id) => GiveItem(id));
        story.BindExternalFunction("TakeItem", (string id) => TakeItem(id));
        story.BindExternalFunction("HasItem", (string id) => HasItem(id));
        story.BindExternalFunction("HasGold", (int amount) => HasGold(amount));
        story.BindExternalFunction("SpendGold", (int amount) => SpendGold(amount));
        story.BindExternalFunction("SetFlag", (string id, bool value) => SetFlagVerb(id, value));
        story.BindExternalFunction("GetFlag", (string id) => GetFlagVerb(id));
        story.BindExternalFunction("RaiseWorldEvent", (string id) => RaiseWorldEvent(id));
    }

    public void Unbind(Story story)
    {
        story.UnbindExternalFunction("StartCombat");
        story.UnbindExternalFunction("FinishQuest");
        story.UnbindExternalFunction("SetOutfit");
        story.UnbindExternalFunction("GiveItem");
        story.UnbindExternalFunction("TakeItem");
        story.UnbindExternalFunction("HasItem");
        story.UnbindExternalFunction("HasGold");
        story.UnbindExternalFunction("SpendGold");
        story.UnbindExternalFunction("SetFlag");
        story.UnbindExternalFunction("GetFlag");
        story.UnbindExternalFunction("RaiseWorldEvent");
    }

    private void StartCombat(string resumeKnot)
    {
        GameEventsManager.instance.dialogueEvents.CombatRequested(resumeKnot);
    }

    private void FinishQuest(string questId, bool won)
    {
        GameEventsManager.instance.questEvents.FinishQuest(questId, won);
    }

    private void SetOutfit(string outfit)
    {
        if (playerStats == null) return;
        if (System.Enum.TryParse<PlayerOutfit>(outfit, out PlayerOutfit parsedOutfit))
        {
            playerStats.SetOutfit(parsedOutfit, true);
            GameEventsManager.instance.dialogueEvents.OutfitChangedByInk(parsedOutfit);
        }
        else
        {
            Debug.LogWarning($"[InkExternalFunctions] Outfit inválido: {outfit}");
        }
    }

    private void GiveItem(string id)
    {
        if (playerStats == null) return;
        playerStats.AddItem(id);
    }

    private void TakeItem(string id)
    {
        if (playerStats == null) return;
        playerStats.RemoveItem(id);
    }

    private bool HasItem(string id)
    {
        if (playerStats == null) return false;
        return playerStats.HasItem(id);
    }
    private bool HasGold(int amount)
    {
        return playerStats != null && amount >= 0 && playerStats.Gold >= amount;
    }
    private void SpendGold(int amount)
    {
        if (playerStats == null) return;
        playerStats.SpendGold(amount);
    }
    private void SetFlagVerb(string id, bool value)
    {
        playerStats?.SetFlag(id, value);
        Debug.Log($"[Ink] SetFlag({id}, {value})");
    }

    private bool GetFlagVerb(string id)
    {
        return playerStats != null && playerStats.GetFlag(id);
    }

    private void RaiseWorldEvent(string id)
    {
        Debug.Log($"[Ink] RaiseWorldEvent({id})");
        WorldEventBus.Raise(id);
    }
}
