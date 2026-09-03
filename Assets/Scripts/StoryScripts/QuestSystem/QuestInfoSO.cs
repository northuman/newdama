using System;
using UnityEngine;

[Serializable]
public class QuestResultProfile
{
    [Header("Victory")]
    public int goldReward;
    public bool levelUp;
    public string[] itemsReward = Array.Empty<string>();
    public string[] cardsReward = Array.Empty<string>();

    [Header("Defeat")]
    public int loseGoldPenalty;
    public int loseHealthPenalty;
    public string[] cardsPenalty = Array.Empty<string>();
    public bool loseIsGameOver;
}

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Quests/Quest Info", order = 1)]
public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }
    [Header("General")]
    public string questName;
    public string inkKnotName;

    [Header("Requirements")]
    public int levelRequirement;
    public QuestInfoSO[] questPrerequisites;
    public PlayerOutfit requiredOutfit;
    public string[] requiredItems;

    [Header("Dialogue visuals")]
    public Sprite backgroundSprite;
    public Sprite npcPortraitSprite;

    [Header("Behavior")]
    public bool isRepeatable;
    public bool launchesCombat = true;
    public bool suppressResultPanel;

    [Header("Results")]
    public QuestResultProfile initialResult = new();
    public QuestResultProfile repeatResult = new();

    [Header("First victory unlocks")]
    public bool unlocksOutfitChange;
    public bool exitsNunPhase;

    public QuestResultProfile GetResultProfile(bool isRepeat)
    {
        QuestResultProfile profile = isRepeat ? repeatResult : initialResult;
        return profile ?? new QuestResultProfile();
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (string.IsNullOrWhiteSpace(id))
        {
            id = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

    }
}

