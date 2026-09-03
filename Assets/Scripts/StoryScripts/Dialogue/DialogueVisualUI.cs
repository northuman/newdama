using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueVisualUI : MonoBehaviour
{
    [Header("Main panel")]
    [SerializeField] private GameObject dialoguePanel;

    [Header("Background")]
    [SerializeField] private Image background;
    [SerializeField] private Sprite defaultBackground;

    [Header("Characters")]
    [SerializeField] private Image left;
    [SerializeField] private Image right;
    
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private const float ACTIVE_ALPHA = 1f;
    private const float INACTIVE_ALPHA = 0.4f;

    private string playerSpeakerName;
    private string caterinaName;
    private string josepName;
    private Sprite caterinaPortrait;
    private Sprite josepPortrait;

    private void Awake()
    {
        dialoguePanel.SetActive(false);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.dialogueEvents.OnDialogueStarted += OnDialogueStarted;
        GameEventsManager.instance.dialogueEvents.OnDialogueEnded   += OnDialogueEnded;
        GameEventsManager.instance.dialogueEvents.OnDisplayDialogue += OnDisplayDialogue;
        GameEventsManager.instance.playerEvents.OnOutfitChanged += OnOutfitChanged;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.OnDialogueStarted -= OnDialogueStarted;
        GameEventsManager.instance.dialogueEvents.OnDialogueEnded   -= OnDialogueEnded;
        GameEventsManager.instance.dialogueEvents.OnDisplayDialogue -= OnDisplayDialogue;
        GameEventsManager.instance.playerEvents.OnOutfitChanged -= OnOutfitChanged;
    }

    public void SetupDialogue(
        QuestInfoSO questInfo,
        Sprite caterinaPortraitSprite,
        Sprite josepPortraitSprite,
        PlayerOutfit currentOutfit,
        string catName,
        string jName)
    {
        caterinaName = catName;
        josepName = jName;
        caterinaPortrait = caterinaPortraitSprite;
        josepPortrait = josepPortraitSprite;

        background.sprite = (questInfo.backgroundSprite != null)
            ? questInfo.backgroundSprite
            : defaultBackground;

        right.sprite = questInfo.npcPortraitSprite;
        ApplyPlayerOutfit(currentOutfit);

        SetPortraitAlpha(left, ACTIVE_ALPHA);
        SetPortraitAlpha(right, ACTIVE_ALPHA);

        speakerNameText.text = string.Empty;
        dialogueText.text    = string.Empty;
    }

    private void OnDialogueStarted()
    {
        dialoguePanel.SetActive(true);
    }

    private void OnDialogueEnded()
    {
        dialoguePanel.SetActive(false);
        speakerNameText.text = string.Empty;
        dialogueText.text    = string.Empty;
    }

    private void OnDisplayDialogue(string line)
    {
        string speaker;
        string text;
        ParseLine(line, out speaker, out text);

        string speakerTrimmed = speaker.Trim();
        bool isPlayer = speakerTrimmed.Equals(caterinaName, System.StringComparison.OrdinalIgnoreCase)
                     || speakerTrimmed.Equals(josepName,    System.StringComparison.OrdinalIgnoreCase);

        speakerNameText.text = isPlayer ? playerSpeakerName : speaker;
        dialogueText.text    = text;

        SetPortraitAlpha(left,  isPlayer ? ACTIVE_ALPHA : INACTIVE_ALPHA);
        SetPortraitAlpha(right, isPlayer ? INACTIVE_ALPHA : ACTIVE_ALPHA);
    }

    private void OnOutfitChanged(PlayerOutfit outfit)
    {
        ApplyPlayerOutfit(outfit);
    }

    private void ApplyPlayerOutfit(PlayerOutfit outfit)
    {
        bool isJosep = outfit == PlayerOutfit.Josep;
        playerSpeakerName = isJosep ? josepName : caterinaName;

        Sprite portrait = isJosep ? josepPortrait : caterinaPortrait;
        if (left != null && portrait != null)
            left.sprite = portrait;
    }

    private void ParseLine(string line, out string speaker, out string text)
    {
        int colonIndex = line.IndexOf(':');
        if (colonIndex > 0)
        {
            speaker = line.Substring(0, colonIndex).Trim();
            text    = line.Substring(colonIndex + 1).Trim();
        }
        else
        {
            speaker = string.Empty;
            text    = line;
        }
    }

    private void SetPortraitAlpha(Image portrait, float alpha)
    {
        if (portrait == null) return;
        Color c = portrait.color;
        c.a = alpha;
        portrait.color = c;
    }
}
