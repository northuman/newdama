using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questsMap;
    private int currentPlayerLevel;

    [Header("Quests Info SO")]
    [SerializeField] private QuestInfoSO[] questInfos;

    private void Awake()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats instance not found in the scene.");
            return;
        }

        questsMap = CreateQuestsMap();
        currentPlayerLevel = playerStats.Level;
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnStartQuest += StartQuest;
        GameEventsManager.instance.questEvents.OnFinishQuest += FinishQuest;
        GameEventsManager.instance.playerEvents.OnLevelChanged += PlayerLevelChange;
        LevelManager.instance.onLevelChange += PlayerLevelChange;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnStartQuest -= StartQuest;
        GameEventsManager.instance.questEvents.OnFinishQuest -= FinishQuest;
        GameEventsManager.instance.playerEvents.OnLevelChanged -= PlayerLevelChange;
        LevelManager.instance.onLevelChange -= PlayerLevelChange;
    }

    private void PlayerLevelChange(int newLevel)
    {
        currentPlayerLevel = newLevel;
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        Debug.Log($"[QuestManager] Check {quest.questInfo.id}: currentPlayerLevel={currentPlayerLevel} vs req={quest.questInfo.levelRequirement}");
        if (currentPlayerLevel < quest.questInfo.levelRequirement)
            return false;

        if (quest.questInfo.questPrerequisites == null)
            return true;

        foreach (QuestInfoSO prerequisite in quest.questInfo.questPrerequisites)
        {
            if (prerequisite == null)
            {
                Debug.LogWarning($"[QuestManager] {quest.questInfo.id} contiene un prerrequisito nulo.");
                return false;
            }

            Quest prerequisiteQuest = GetQuestById(prerequisite.id);
            if (prerequisiteQuest == null || !prerequisiteQuest.hasCompletedPrimary)
                return false;
        }

        return true;
    }

    private void Update()
    {
        if (questsMap == null) return;

        foreach (Quest quest in questsMap.Values)
        {
            if (quest.questState == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
                ChangeQuestState(quest.questInfo.id, QuestState.CAN_START);
        }
    }

    private void Start()
    {
        if (questsMap == null) return;

        foreach (Quest quest in questsMap.Values)
            GameEventsManager.instance.questEvents.ChangeQuestState(quest);
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        if (quest == null) return;

        quest.questState = state;
        GameEventsManager.instance.questEvents.ChangeQuestState(quest);
    }

    private void StartQuest(string questId)
    {
        Quest quest = GetQuestById(questId);
        if (quest == null) return;
        ChangeQuestState(questId, QuestState.IN_PROGRESS);
    }

    private bool ApplyResult(Quest quest, bool win, bool isRepeat)
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null) return false;

        QuestInfoSO info = quest.questInfo;
        QuestResultProfile profile = info.GetResultProfile(isRepeat);
        QuestResultData result = new QuestResultData(info.id, win)
        {
            isRepeat = isRepeat,
            newLevel = 0,
            goldChanged = 0,
            healthLost = 0
        };

        if (win)
        {
            if (profile.goldReward != 0)
            {
                int previousGold = playerStats.Gold;
                if (profile.goldReward > 0)
                    playerStats.Gold += profile.goldReward;
                else
                    playerStats.SpendGold(-profile.goldReward);
                result.goldChanged = playerStats.Gold - previousGold;
            }

            if (profile.levelUp)
            {
                playerStats.Level += 1;
                result.newLevel = playerStats.Level;
            }

            if (profile.itemsReward != null)
            {
                foreach (string item in profile.itemsReward)
                {
                    if (string.IsNullOrWhiteSpace(item)) continue;
                    playerStats.AddItem(item);
                    result.itemsObtained.Add(item);
                }
            }

            if (profile.cardsReward != null)
            {
                foreach (string card in profile.cardsReward)
                {
                    if (string.IsNullOrWhiteSpace(card)) continue;
                    result.cardsObtained.Add(card);
                }
            }

            if (!isRepeat && info.unlocksOutfitChange)
                playerStats.UnlockOutfitChange();

            if (!isRepeat && info.exitsNunPhase)
                playerStats.ExitNunPhase();

            Debug.Log($"[QuestManager] Victoria ({(isRepeat ? "revancha" : "principal")}): {result.goldChanged} oro, +{(profile.levelUp ? 1 : 0)} nivel, {result.itemsObtained.Count} items");
        }
        else
        {
            if (profile.loseGoldPenalty > 0)
            {
                int previousGold = playerStats.Gold;
                playerStats.SpendGold(profile.loseGoldPenalty);
                result.goldChanged = playerStats.Gold - previousGold;
            }

            if (profile.loseHealthPenalty > 0)
            {
                playerStats.CurrentHealth -= profile.loseHealthPenalty;
                result.healthLost = profile.loseHealthPenalty;
            }

            Debug.Log($"[QuestManager] Derrota ({(isRepeat ? "revancha" : "principal")}): {result.goldChanged} oro, -{profile.loseHealthPenalty} salud");

            if (playerStats.CurrentHealth <= 0 || profile.loseIsGameOver)
            {
                Debug.LogWarning($"[QuestManager] GAME OVER en {info.id}");
                GameEventsManager.instance.questEvents.GameOver();
                return true;
            }
        }

        if (!info.suppressResultPanel)
        {
            GameEventsManager.instance.questEvents.QuestResult(result);
        }
        return false;
    }

    private void FinishQuest(string questId, bool win)
    {
        Quest quest = GetQuestById(questId);
        if (quest == null) return;

        bool isRepeat = quest.hasCompletedPrimary;
        if (ApplyResult(quest, win, isRepeat))
            return;

        bool completesPrimary = !isRepeat && (win || !quest.questInfo.isRepeatable);
        quest.RecordResult(completesPrimary);

        QuestState nextState = quest.questInfo.isRepeatable
            ? QuestState.CAN_START
            : QuestState.FINISHED;
        ChangeQuestState(questId, nextState);

        Debug.Log($"[QuestManager] Quest resuelta: {questId} | Victoria: {win} | Revancha: {isRepeat} | Estado: {nextState}");
    }

    private Dictionary<string, Quest> CreateQuestsMap()
    {
        Dictionary<string, Quest> idToQuestMap = new();
        if (questInfos == null) return idToQuestMap;

        foreach (QuestInfoSO questInfo in questInfos)
        {
            if (questInfo == null) continue;

            if (!idToQuestMap.ContainsKey(questInfo.id))
                idToQuestMap.Add(questInfo.id, new Quest(questInfo));
            else
                Debug.LogWarning($"Duplicate quest ID found: {questInfo.id}. Skipping this quest.");
        }

        return idToQuestMap;
    }

    private Quest GetQuestById(string questId)
    {
        if (questsMap != null && questsMap.TryGetValue(questId, out Quest quest))
            return quest;

        Debug.LogWarning($"Quest with ID {questId} not found.");
        return null;
    }
}
