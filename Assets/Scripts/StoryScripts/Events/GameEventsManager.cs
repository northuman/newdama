using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    public QuestEvents questEvents;
    public DialogueEvents dialogueEvents;
    public PlayerEvents playerEvents;
    [HideInInspector] public string ActiveQuestId;
    [HideInInspector] public bool DebugMode;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of GameEventsManager detected.");
        }
        instance = this;
        Debug.Log("GameEventsManager instance created.");

        questEvents = new QuestEvents();
        playerEvents = new PlayerEvents();
        dialogueEvents = new DialogueEvents();
    }
}
