using System.Collections;
using UnityEngine;

public class OutfitSwitcher : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerStats playerStats;

    [Header("Outfits")]
    [SerializeField] private GameObject caterinaVisual;
    [SerializeField] private Animator caterinaAnimator;
    [SerializeField] private GameObject josepVisual;
    [SerializeField] private Animator josepAnimator;

    [Header("Transición")]
    [SerializeField] private GameObject whirlwindMesh;
    [SerializeField] private Animator whirlwindAnimator;
    [SerializeField] private string whirlwindAnimationState = "Whirlwind";
    [SerializeField] private float transitionDuration = 0.5f;

    [Header("Input")]
    [SerializeField] private KeyCode switchKey = KeyCode.O;

    private bool isTransitioning = false;
    private Coroutine transitionCoroutine;

    private void Start()
    {
        ApplyOutfitVisual(playerStats.CurrentOutfit);
    }
    private void Update()
    {
        // Detectar tecla presionada y cambiar outfit
        if (Input.GetKeyDown(switchKey) && !isTransitioning)
        {
            PlayerOutfit targetOutfit = playerStats.CurrentOutfit == PlayerOutfit.Josep 
                ? PlayerOutfit.Caterina 
                : PlayerOutfit.Josep;

            SwitchOutfit(targetOutfit);
        }
    }

    private void OnEnable()
    {
        GameEventsManager.instance.playerEvents.OnOutfitChanged += OnOutfitChanged;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.playerEvents.OnOutfitChanged -= OnOutfitChanged;

        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
            transitionCoroutine = null;
        }

        isTransitioning = false;
        if (whirlwindMesh != null)
            whirlwindMesh.SetActive(false);
    }


    public void SwitchOutfit(PlayerOutfit newOutfit)
    {
        if (!playerStats.CanChangeOutfit)
        {
            Debug.LogWarning("[OutfitSwitcher] No puedes cambiar outfit aún.");
            return;
        }

        playerStats.SetOutfit(newOutfit);
    }

    private void OnOutfitChanged(PlayerOutfit newOutfit)
    {
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(PlayOutfitTransition(newOutfit));
    }

    private IEnumerator PlayOutfitTransition(PlayerOutfit newOutfit)
    {
        isTransitioning = true;

        if (whirlwindMesh != null)
        {
            whirlwindMesh.SetActive(true);

            if (whirlwindAnimator != null)
            {
                whirlwindAnimator.Play(whirlwindAnimationState);
            }
        }

        yield return new WaitForSeconds(transitionDuration);

        ApplyOutfitVisual(newOutfit);

        if (whirlwindMesh != null)
        {
            whirlwindMesh.SetActive(false);
        }

        Debug.Log($"[OutfitSwitcher] Outfit cambiado a: {newOutfit}");

        isTransitioning = false;
        transitionCoroutine = null;
    }

    private void ApplyOutfitVisual(PlayerOutfit outfit)
    {
        bool esJosep = outfit == PlayerOutfit.Josep;

        caterinaVisual.SetActive(!esJosep);
        josepVisual.SetActive(esJosep);

        Animator activeAnimator = esJosep ? josepAnimator : caterinaAnimator;
        playerController.SetActiveAnimator(activeAnimator);
    }
}
