using System;
using System.Collections.Generic;
public struct QuestResultData 
{
    public string questId;
    public bool win;
    public bool isRepeat;
    public int newLevel;
    public int goldChanged;
    public int healthLost; 
    public List<string> itemsObtained;
    public List<string> cardsObtained;

    public QuestResultData(string questId, bool win)
    {
        this.questId = questId;
        this.win = win;
        this.isRepeat = false;
        this.newLevel = 0;
        this.goldChanged = 0;
        this.healthLost = 0;
        this.itemsObtained = win ? new List<string>() : null;
        this.cardsObtained = win ? new List<string>() : null;
    }
}
public class QuestEvents
{
    public event Action<string> OnStartQuest;

    public void StartQuest(string questId)
    {
        OnStartQuest?.Invoke(questId);
    }

    public event Action<string, bool> OnFinishQuest;
    public void FinishQuest(string questId, bool win)
    {
        OnFinishQuest?.Invoke(questId, win);
    }

    public event Action<Quest> OnQuestStateChanged;
    public void ChangeQuestState(Quest quest)
    {
        OnQuestStateChanged?.Invoke(quest);
    }

    public event Action OnGameOver;
    public void GameOver()
    {
        OnGameOver?.Invoke();
    }

    public event Action<QuestResultData> OnQuestResult;
    public void QuestResult(QuestResultData resultData)
    {
        OnQuestResult?.Invoke(resultData);
    }
}

