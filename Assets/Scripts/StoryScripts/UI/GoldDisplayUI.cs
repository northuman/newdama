using TMPro;
using UnityEngine;

public class GoldDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    private void OnEnable()
    {
        Debug.Log($"GoldDisplayUI.OnEnable — instance null? {GameEventsManager.instance == null}", this);
        GameEventsManager.instance.playerEvents.OnGoldChanged += UpdateGold;
        GameEventsManager.instance.playerEvents.OnStatsInitialized += OnStatsInitialized;
    }

    private void OnDisable()
    {
        
        GameEventsManager.instance.playerEvents.OnGoldChanged -= UpdateGold;
        GameEventsManager.instance.playerEvents.OnStatsInitialized -= OnStatsInitialized;
    }

    private void OnStatsInitialized(int health, int maxHealth, int level, int gold)
    {
        UpdateGold(gold);
    }

    private void UpdateGold(int newGold)
    {
        Debug.Log($"UpdateGold llamado con: {newGold}", this);
        goldText.text = $"{newGold} DINEROS";
    }
}