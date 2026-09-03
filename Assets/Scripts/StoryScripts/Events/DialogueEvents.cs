using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueEvents
{
    public event Action<string, QuestInfoSO> OnDialogueStart;

    public void StartDialogue(string knot, QuestInfoSO questInfo)
    {
        OnDialogueStart?.Invoke(knot, questInfo);
    }

    public event Action OnDialogueStarted;
    public void DialogueStarted()
    {
        OnDialogueStarted?.Invoke();
    }

    public event Action OnDialogueEnded;
    public void DialogueEnded()
    {
        OnDialogueEnded?.Invoke();
    }

    public event Action<string> OnDisplayDialogue;
    public void DisplayDialogue(string dialogue)
    {
        OnDisplayDialogue?.Invoke(dialogue);
    }

    public event Action<string> OnCombatRequested;
    public void CombatRequested(string resumeKnot)
    {
        OnCombatRequested?.Invoke(resumeKnot);
    }

    public event Action<bool> OnCombatResolved;
    public void CombatResolved(bool win)
    {
        OnCombatResolved?.Invoke(win);
    }

    public event Action<PlayerOutfit> OnOutfitChangedByInk;
    public void OutfitChangedByInk(PlayerOutfit outfit)
    {
        OnOutfitChangedByInk?.Invoke(outfit);
    }
}
