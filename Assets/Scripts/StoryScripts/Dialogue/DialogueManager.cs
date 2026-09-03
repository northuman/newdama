using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("Ink Dialogue")]
    [SerializeField] private TextAsset jsonInk;

    [Header("Player")]
    [SerializeField] private Sprite playerPortrait;
    [SerializeField] private string playerName = "Caterina";
    [SerializeField] private Sprite josepPortrait;
    [SerializeField] private string josepName = "Josep";
    [SerializeField] private PlayerStats playerStats;

    [Header("Dialogue Visual UI")]
    [SerializeField] private DialogueVisualUI dialogueVisualUI;

    [Header("Debug")]
    [SerializeField] private bool debugMode = true;

    private Story story;

    private bool dialogueStarted = false;
    private int dialogueStartedFrame = -1;
    private string resumeKnot = string.Empty;
    private bool waitingForCombat = false;

    private QuestInfoSO currentQuestInfo;
    private CustomActions input;

    private InkExternalFunctions externalFunctions;
    

    private void Awake()
    {
        input = new();
        story = new Story(jsonInk.text)
        {
            allowExternalFunctionFallbacks = true
        };
        externalFunctions = new InkExternalFunctions(playerStats);
        externalFunctions.Bind(story);
    }

    private void OnDestroy()
    {
    
        externalFunctions.Unbind(story);
    }

    private void OnEnable()
    {
        input.Enable();
        GameEventsManager.instance.dialogueEvents.OnDialogueStart += StartDialogue;
        GameEventsManager.instance.dialogueEvents.OnCombatRequested += OnCombatRequested;
        GameEventsManager.instance.dialogueEvents.OnCombatResolved  += OnCombatResolved;
        GameEventsManager.instance.dialogueEvents.OnOutfitChangedByInk += OnOutfitChangedByInk;
        input.Main.Submit.performed += OnSubmitPerformed;

    }

    private void OnDisable()
    {
        input.Disable();
        GameEventsManager.instance.dialogueEvents.OnDialogueStart -= StartDialogue;
        GameEventsManager.instance.dialogueEvents.OnCombatRequested -= OnCombatRequested;
        GameEventsManager.instance.dialogueEvents.OnCombatResolved -= OnCombatResolved;
        GameEventsManager.instance.dialogueEvents.OnOutfitChangedByInk -= OnOutfitChangedByInk;
        input.Main.Submit.performed -= OnSubmitPerformed;
    }

    private void OnSubmitPerformed(InputAction.CallbackContext context)
    {

        if (dialogueStarted && Time.frameCount == dialogueStartedFrame)
            return;

        SubmitPressed();
    }

    private void SubmitPressed()
    {
        if (!dialogueStarted)
        {
            Debug.Log("Dialogue has not started yet.");
            return;
        }
        ContinueOrExitStory();
    }

    private void SyncOutfitVars(QuestInfoSO questInfo)
    {
        if (questInfo == null) return;
        if (story == null) return;
        if (playerStats == null)
        {
            Debug.LogWarning("[DialogueManager] playerStats no asignado en el Inspector. Omitiendo sincronización.");
            return;
        }

        story.variablesState["outfit"] = playerStats.CurrentOutfit.ToString();
        story.variablesState["required_outfit"] = questInfo.requiredOutfit.ToString();
    }
    private void StartDialogue(string knotName, QuestInfoSO questInfo)
    {
        if (dialogueStarted) return;
        if (string.IsNullOrEmpty(knotName))
        {
            Debug.LogError("Knot name is empty. Cannot start dialogue.");
            return;
        }

        dialogueStarted = true;
        dialogueStartedFrame = Time.frameCount;
        story.ChoosePathString(knotName);
        currentQuestInfo = questInfo;

        if (dialogueVisualUI == null)
        {
            Debug.LogError("[DialogueManager] Falta asignar Dialogue Visual UI en el Inspector.");
            return;
        }

        SyncOutfitVars(questInfo);
        PlayerOutfit currentOutfit = playerStats != null
            ? playerStats.CurrentOutfit
            : PlayerOutfit.Caterina;
        Debug.Log($"[DialogueManager] Iniciando diálogo en '{knotName}' con outfit: {currentOutfit}");
        dialogueVisualUI.SetupDialogue(
            currentQuestInfo,
            playerPortrait,
            josepPortrait,
            currentOutfit,
            playerName,
            josepName);
        GameEventsManager.instance.dialogueEvents.DialogueStarted();
        DisablePlayerMovement();
        ContinueOrExitStory();
    }

    private void ContinueOrExitStory()
    {
        if (story.canContinue)
    {
        string text = story.Continue();
        if (string.IsNullOrWhiteSpace(text))
        {
            ContinueOrExitStory();
            return;
        }
        GameEventsManager.instance.dialogueEvents.DisplayDialogue(text);
        }
        else if (waitingForCombat)
        {
            Debug.Log("[DialogueManager] Esperando resolución de combate. No se puede continuar el diálogo.");
        }
        else
        {
            StartCoroutine(ExitDialogue());
        }

    }

    private IEnumerator ExitDialogue()
    {
        yield return null;
        Debug.Log("Exiting dialogue.");
        dialogueStarted = false;
        GameEventsManager.instance.dialogueEvents.DialogueEnded();

        input.Main.Move.Enable();
        EnablePlayerMovement();
    }

    private void EnablePlayerMovement()
    {
        GameObject player = GameObject.FindWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.Alive = true;
        }
        else
        {
            Debug.LogError("PlayerController component not found on Player.");
        }
    }

    private void DisablePlayerMovement()
    {
        GameObject player = GameObject.FindWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.Alive = false; // Disable player movement
        }
        else
        {
            Debug.LogError("PlayerController component not found on Player.");
        }
    }

    private void OnCombatRequested(string knot)
    {
        resumeKnot = knot;
        waitingForCombat = true;
        GameEventsManager.instance.DebugMode = debugMode;
        GameEventsManager.instance.ActiveQuestId = currentQuestInfo != null
            ? currentQuestInfo.id
            : string.Empty;

        GameEventsManager.instance.dialogueEvents.DialogueEnded();

        StartCoroutine(LoadCombatScene());
    }

    private IEnumerator LoadCombatScene()
    {
        if (FadeManager.instance != null)
            yield return FadeManager.instance.StartFadeOut();

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) p.GetComponent<PlayerController>().Alive = false;

        AsyncOperation op = SceneManager.LoadSceneAsync("Juego", LoadSceneMode.Additive);
        while (!op.isDone) yield return null;

        Camera.main?.gameObject.SetActive(false);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Juego"));
        Debug.Log($"[DialogueManager] Juego cargada. Reanudar en '{resumeKnot}'.");

        if (FadeManager.instance != null)
            yield return FadeManager.instance.StartFadeIn();
    }

    private void OnCombatResolved(bool win)
    {
        waitingForCombat = false;

        story.variablesState["combat_won"] = win;

        dialogueStarted = true;
        story.ChoosePathString(resumeKnot);

        GameEventsManager.instance.dialogueEvents.DialogueStarted();
        ContinueOrExitStory();
    }
    private void OnOutfitChangedByInk(PlayerOutfit outfit)
    {
        story.variablesState["outfit"] = outfit.ToString();
    }
}
