using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int level = 1;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 100;
    [SerializeField] private int gold = 0;
    [SerializeField] private int speed = 5;

    public int Level { get => level; set { level = value; GameEventsManager.instance.playerEvents.LevelChanged(level); } }
    public int CurrentHealth { 
        get => currentHealth; 
        set { 
            currentHealth = Mathf.Clamp(value, 0, maxHealth);  // nunca más que el máximo
            GameEventsManager.instance.playerEvents.HealthChanged(currentHealth, maxHealth);
        } 
    }

    public int MaxHealth { 
        get => maxHealth; 
        set { 
            maxHealth = value; 
            GameEventsManager.instance.playerEvents.MaxHealthChanged(currentHealth, maxHealth);
        } 
    }
    public int Gold { get => gold; set { gold = value; GameEventsManager.instance.playerEvents.GoldChanged(gold); } }
    public int Speed { get => speed; set => speed = value; }    [SerializeField] private PlayerOutfit currentOutfit = PlayerOutfit.Caterina;
    private bool canChangeOutfit = false;
    private bool isNunPhase = true;

    private HashSet<string> inventory = new HashSet<string>();

    private Dictionary<string, bool> flags = new Dictionary<string, bool>();

    public PlayerOutfit CurrentOutfit => currentOutfit;
    public bool CanChangeOutfit => canChangeOutfit;
    public bool IsNunPhase => isNunPhase;
    public IReadOnlyList<string> Inventory => inventory.OrderBy(itemId => itemId).ToList();

    // Singleton
    public static PlayerStats Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("[PlayerStats] Multiple instances detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetOutfit(PlayerOutfit outfit, bool force = false)
    {
        if (!force && !canChangeOutfit)
        {
            Debug.LogWarning("[PlayerStats] Intento de cambiar outfit sin permiso.");
            return;
        }
        currentOutfit = outfit;
        GameEventsManager.instance.playerEvents.OutfitChanged(outfit);
    }

    public void UnlockOutfitChange() 
    { 
        canChangeOutfit = true;
        GameEventsManager.instance.playerEvents.CanChangeOutfitChanged(true);
    }
    public void ExitNunPhase()       { isNunPhase = false; }

    public void AddItem(string id)    { inventory.Add(id); }
    public void RemoveItem(string id) { inventory.Remove(id); }
    public bool HasItem(string id)    => inventory.Contains(id);

    public void SpendGold(int amount) { Gold = Mathf.Max(0, gold - amount); }

    public void SetFlag(string id, bool value) { flags[id] = value; }
    public bool GetFlag(string id)             => flags.TryGetValue(id, out bool v) && v;

    private void Start()
    {
        Debug.Log($"PlayerStats.Start — gold={gold}", this);
        Level = level;
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
        Gold = gold; 
        GameEventsManager.instance.playerEvents.StatsInitialized(currentHealth, maxHealth, level, gold);
    }
}
