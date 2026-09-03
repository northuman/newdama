using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestInfoSO questInfo;
    public QuestState questState;
    public bool hasCompletedPrimary { get; private set; }
    public int resolvedCombatCount { get; private set; }

    public Quest(QuestInfoSO questInfo)
    {
        this.questInfo = questInfo;
        this.questState = QuestState.REQUIREMENTS_NOT_MET;
        this.hasCompletedPrimary = false;
        this.resolvedCombatCount = 0;
    }

    public void RecordResult(bool completesPrimary)
    {
        resolvedCombatCount++;
        if (completesPrimary)
            hasCompletedPrimary = true;
    }
}
