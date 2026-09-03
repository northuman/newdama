using System;
public class PlayerEvents
{
    public event Action<int> OnLevelChanged;

    public void LevelChanged(int newLevel)
    {
        OnLevelChanged?.Invoke(newLevel);
    }

    public event Action<int, int> OnHealthChanged;
    public void HealthChanged(int current, int max)
    {
        OnHealthChanged?.Invoke(current, max);
    }

    public event Action<int, int> OnMaxHealthChanged;
    public void MaxHealthChanged(int current, int max)
    {
        OnMaxHealthChanged?.Invoke(current, max);
    }

    public event Action<int> OnGoldChanged;

    public void GoldChanged(int newGold)
    {
        OnGoldChanged?.Invoke(newGold);
    }

    public event Action<int, int, int, int> OnStatsInitialized;

    public void StatsInitialized(int health, int maxHealth, int level, int gold)
    {
        OnStatsInitialized?.Invoke(health, maxHealth, level, gold);
    }

    public event Action<PlayerOutfit> OnOutfitChanged;
    public void OutfitChanged(PlayerOutfit newOutfit)
    {
        OnOutfitChanged?.Invoke(newOutfit);
    }

    public event Action<bool> OnCanChangeOutfitChanged;
    public void CanChangeOutfitChanged(bool canChange)
    {
        OnCanChangeOutfitChanged?.Invoke(canChange);
    }
}