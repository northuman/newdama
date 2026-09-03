using UnityEngine;
using UnityEngine.UI;

public class OutfitIndicatorUI : MonoBehaviour
{
    [SerializeField] private Image outfitIndicator;
    [SerializeField] private Sprite caterineSprite;
    [SerializeField] private Sprite josepSprite;

    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("[OutfitIndicatorUI] PlayerStats no encontrado en la escena");
            return;
        }
    }

    private void OnEnable()
    {
        if (playerStats != null)
        {
            GameEventsManager.instance.playerEvents.OnOutfitChanged += UpdateOutfitDisplay;
            UpdateOutfitDisplay(playerStats.CurrentOutfit);
        }
    }

    private void OnDisable()
    {
        if (playerStats != null)
        {
            GameEventsManager.instance.playerEvents.OnOutfitChanged -= UpdateOutfitDisplay;
        }
    }

    private void UpdateOutfitDisplay(PlayerOutfit outfit)
    {
        if (outfitIndicator == null)
            return;

        switch (outfit)
        {
            case PlayerOutfit.Caterina:
                outfitIndicator.sprite = caterineSprite;
                break;
            case PlayerOutfit.Josep:
                outfitIndicator.sprite = josepSprite;
                break;
            case PlayerOutfit.Any:
                // Mostrar Caterina por defecto si está en estado Any
                outfitIndicator.sprite = caterineSprite;
                break;
        }
    }
}
