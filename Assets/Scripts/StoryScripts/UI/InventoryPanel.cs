using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Content")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI itemsText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Buttons")]
    [SerializeField] private Button viewInventoryButton;
    [SerializeField] private Button viewDeckButton;
    [SerializeField] private Button closeButton;

    private PlayerController playerController;
    private bool isShowing;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            playerController = player.GetComponent<PlayerController>();

        if (playerController == null)
            Debug.LogWarning("[InventoryPanel] PlayerController not found on the Player GameObject.");

        SetVisibility(false);
    }

    private void Update()
    {
        if (isShowing && playerController != null && playerController.Alive)
            playerController.Alive = false;
    }

    public void Show()
    {
        titleText.text = "Inventario";
        levelText.text = "";
        goldText.text = "";
        healthText.text = "";

        if (PlayerStats.Instance == null)
        {
            itemsText.text = "No se han podido cargar los objetos.";
            Debug.LogWarning("[InventoryPanel] PlayerStats instance not found.");
        }
        else
        {
            var inventory = PlayerStats.Instance.Inventory;
            itemsText.text = inventory.Count == 0
                ? "No tienes objetos."
                : string.Join("\n", inventory);
        }

        isShowing = true;
        if (playerController != null)
            playerController.Alive = false;

        SetVisibility(true);
    }

    public void Hide()
    {
        bool wasShowing = isShowing;
        isShowing = false;
        SetVisibility(false);

        if (wasShowing && playerController != null)
            playerController.Alive = true;
    }

    private void SetVisibility(bool visible)
    {
        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }
}
