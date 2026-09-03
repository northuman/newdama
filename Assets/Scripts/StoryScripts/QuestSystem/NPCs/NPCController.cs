using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class NPCController : MonoBehaviour
{
    private static NPCController promptOwner;

    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfo;

    // Exposed as read-only data so systems such as the minimap can discover
    // the quest represented by this NPC without duplicating the assignment.
    public QuestInfoSO QuestInfo => questInfo;

    [Header("Prompt")]
    [SerializeField] private GameObject interactPrompt;

    [Header("Interaction")]
    [SerializeField] private float interactionRange = 2.5f;

    [Header("CallBacks (optional)")]
    public UnityEvent onQuestWon;

    private Transform playerTransform;
    private bool playerIsNear = false;
    private bool dialogueActive = false;
    private QuestState currentQuestState = QuestState.REQUIREMENTS_NOT_MET;
    private CustomActions input;

    private void Awake()
    {
        input = new();
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }
    
    private void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            playerTransform = p.transform;
        }
        else
        {
            Debug.LogError("Player not found in the scene. Make sure the player has the 'Player' tag.");
        }
    }

    private void OnEnable()
    {
        input.Enable();
        input.Main.Submit.performed += OnSubmitPressed;
        GameEventsManager.instance.questEvents.OnQuestStateChanged += OnQuestAdvanced;
        GameEventsManager.instance.dialogueEvents.OnDialogueEnded += OnDialogueEnded;
    }

    private void OnDisable()
    {
        SetPromptVisible(false);
        input.Disable();
        input.Main.Submit.performed -= OnSubmitPressed;
        GameEventsManager.instance.questEvents.OnQuestStateChanged -= OnQuestAdvanced;
        GameEventsManager.instance.dialogueEvents.OnDialogueEnded -= OnDialogueEnded;
    }

    private void Update()
    {
        if(playerTransform == null || dialogueActive) return;

        if (currentQuestState != QuestState.CAN_START)
        {
            if (playerIsNear) { playerIsNear = false; SetPromptVisible(false); }
            return;
        }
        
        Vector3 diff = playerTransform.position - transform.position;
        diff.y = 0f;
        bool near = diff.sqrMagnitude <= interactionRange * interactionRange;

        if (near != playerIsNear)
        {
            playerIsNear = near;
            SetPromptVisible(near);
        }
    }

    private void OnSubmitPressed(InputAction.CallbackContext context)
    {
        if (!playerIsNear || dialogueActive) return;
        if (currentQuestState != QuestState.CAN_START) return;
        if(questInfo == null)
        {
            Debug.LogWarning("QuestInfoSO is not assigned to the NPCController.");
            return;
        }

        dialogueActive = true;
        playerIsNear = false;
        SetPromptVisible(false);
        GameEventsManager.instance.dialogueEvents.StartDialogue(questInfo.inkKnotName, questInfo);
    }

    private void OnQuestAdvanced(Quest quest)
    {
        if (questInfo == null || quest.questInfo.id != questInfo.id) return;
        currentQuestState = quest.questState;

        if (currentQuestState != QuestState.CAN_START)
        {
            playerIsNear = false;
            SetPromptVisible(false);
        }
    }

    private void OnDialogueEnded()
    {
        dialogueActive = false;
    }

    private void SetPromptVisible(bool visible)
    {
        if (interactPrompt == null) return;

        if (visible)
        {
            if (promptOwner != null && promptOwner != this)
                promptOwner.playerIsNear = false;

            promptOwner = this;
            interactPrompt.SetActive(true);
            return;
        }

        if (promptOwner == this)
        {
            interactPrompt.SetActive(false);
            promptOwner = null;
        }
    }
}
