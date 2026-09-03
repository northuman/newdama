using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardResultPanel : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI itemsText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI healthLostText;

    [Header("Buttons")]
    [SerializeField] private Button continueButton;

    private PlayerController playerController;
    private bool isShowing = false;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
            Debug.LogWarning("[RewardResultPanel] PlayerController not found on the Player GameObject.");

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnQuestResult += ShowResult;
        Hide();
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnQuestResult -= ShowResult;
    }

    private void Update()
    {
        if (isShowing && playerController != null && playerController.Alive)
            playerController.Alive = false;
    }

    public void OnContinueClicked()
    {
        isShowing = false;
        Hide();
        if (playerController != null)
            playerController.Alive = true;
    }

    private void ShowResult(QuestResultData result)
    {
        levelText.text = "";
        itemsText.text = "";
        goldText.text = "";
        healthLostText.text = "";

        if (result.win)
        {
            titleText.text = "¡Victoria!";
            levelText.text = result.newLevel > 0 ? $"Nivel alcanzado: {result.newLevel}" : "";
            itemsText.text = result.itemsObtained != null && result.itemsObtained.Count > 0
                ? $"Objetos obtenidos: {string.Join(", ", result.itemsObtained)}"
                : "";
            goldText.text = result.goldChanged > 0
                ? $"Oro ganado: {result.goldChanged}"
                : result.goldChanged < 0
                    ? $"Oro gastado: {Mathf.Abs(result.goldChanged)}"
                    : "";
        }
        else
        {
            titleText.text = "¡Derrota!";
            levelText.text = "";
            itemsText.text = "";
            goldText.text = result.goldChanged != 0 ? $"Oro perdido: {Mathf.Abs(result.goldChanged)}" : "";
            healthLostText.text = result.healthLost != 0 ? $"Vida perdida: {result.healthLost}" : "";
        }

        Show();
    }

    private void Show()
    {
        isShowing = true;
        if (playerController != null)
            playerController.Alive = false;

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
