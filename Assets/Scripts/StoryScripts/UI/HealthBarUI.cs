using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;

    private void OnEnable()
    {
        GameEventsManager.instance.playerEvents.OnHealthChanged += UpdateHealthBar;
        GameEventsManager.instance.playerEvents.OnMaxHealthChanged += UpdateHealthBar;
        GameEventsManager.instance.playerEvents.OnStatsInitialized += OnStatsInitialized;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.playerEvents.OnHealthChanged -= UpdateHealthBar;
        GameEventsManager.instance.playerEvents.OnMaxHealthChanged -= UpdateHealthBar;
        GameEventsManager.instance.playerEvents.OnStatsInitialized -= OnStatsInitialized;
    }

    private void OnStatsInitialized(int health, int maxHealth, int level, int gold)
    {
        UpdateHealthBar(health, maxHealth);
    }

    private void UpdateHealthBar(int current, int max)
    {
        healthBarFill.fillAmount = (float)current / max;
    }
}