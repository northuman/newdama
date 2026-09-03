using TMPro;
using UnityEngine;

public class LevelDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;

    private void OnEnable()
    {
        GameEventsManager.instance.playerEvents.OnLevelChanged += UpdateLevelDisplay;
        GameEventsManager.instance.playerEvents.OnStatsInitialized += OnStatsInitialized;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.playerEvents.OnLevelChanged -= UpdateLevelDisplay;
        GameEventsManager.instance.playerEvents.OnStatsInitialized -= OnStatsInitialized;
    }

    private void OnStatsInitialized(int health, int maxHealth, int level, int gold)
    {
        UpdateLevelDisplay(level);
    }

    private void UpdateLevelDisplay(int newLevel)
    {
        levelText.text = $"Nivel {newLevel}";
    }
}