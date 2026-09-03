using UnityEngine;
using TMPro;

public class OutfitChangePrompt : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.3f;

    private float fadeTimer = 0f;
    private bool isVisible = false;
    private bool targetVisible = false;

    private void OnEnable()
    {
        GameEventsManager.instance.playerEvents.OnCanChangeOutfitChanged += UpdatePromptVisibility;
        UpdatePromptVisibility(PlayerStats.Instance != null && PlayerStats.Instance.CanChangeOutfit);
    }

    private void OnDisable()
    {
        GameEventsManager.instance.playerEvents.OnCanChangeOutfitChanged -= UpdatePromptVisibility;
    }

    private void Update()
    {
        // Fade in/out suave
        if (targetVisible != isVisible)
        {
            fadeTimer += Time.deltaTime / fadeDuration;
            fadeTimer = Mathf.Clamp01(fadeTimer);

            float targetAlpha = targetVisible ? 1f : 0f;
            canvasGroup.alpha = Mathf.Lerp(isVisible ? 1f : 0f, targetAlpha, fadeTimer);

            if (fadeTimer >= 1f)
            {
                isVisible = targetVisible;
                fadeTimer = 0f;
                canvasGroup.blocksRaycasts = targetVisible;
            }
        }
    }

    private void UpdatePromptVisibility(bool canChange)
    {
        targetVisible = canChange;
        fadeTimer = 0f;

        if (canChange && promptText != null)
            promptText.text = "Pulsa [ O ] para cambiar de disfraz";
    }
}
