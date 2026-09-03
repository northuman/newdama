using System;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    [Serializable]
    private sealed class QuestTarget
    {
        public QuestInfoSO questInfo;
        public Transform worldTarget;
    }

    private sealed class QuestMarker
    {
        public QuestInfoSO questInfo;
        public Transform worldTarget;
        public RectTransform marker;
        public bool hasCompletedPrimary;
    }

    [Header("World references")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Collider worldBoundsCollider;

    [Header("UI references")]
    [SerializeField] private RectTransform markerLayer;
    [SerializeField] private RectTransform questMarkerTemplate;
    [SerializeField] private RectTransform playerMarker;

    [Header("Primary quests")]
    [SerializeField] private QuestTarget[] questTargets = Array.Empty<QuestTarget>();

    [Tooltip("Añade automáticamente un marcador por cada NPCController con QuestInfoSO en la escena.")]
    [SerializeField] private bool autoDiscoverNpcQuestTargets = true;

    private readonly Dictionary<string, QuestMarker> questMarkers = new();
    private bool hasValidBounds;
    private float worldMinX;
    private float worldMaxX;
    private float worldMinZ;
    private float worldMaxZ;
    private bool subscribedToEvents;

    private void OnEnable()
    {
        TrySubscribeToEvents();
    }

    private void Start()
    {
        ResolveMissingReferences();
        TrySubscribeToEvents();

        hasValidBounds = TryCalculateSquareBounds();
        if (!hasValidBounds)
        {
            Debug.LogWarning("[Minimap] No se pudieron calcular limites validos del mundo.", this);
            SetMarkerVisible(playerMarker, false);
            return;
        }

        BuildQuestMarkers();
        RefreshQuestVisibility();
        UpdatePlayerMarker();

        if (playerMarker != null)
            playerMarker.SetAsLastSibling();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void LateUpdate()
    {
        if (hasValidBounds)
            UpdatePlayerMarker();
    }

    private void ResolveMissingReferences()
    {
        if (playerStats == null)
            playerStats = PlayerStats.Instance != null
                ? PlayerStats.Instance
                : FindObjectOfType<PlayerStats>();

        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
        }
    }

    private bool TryCalculateSquareBounds()
    {
        if (worldBoundsCollider == null ||
            !worldBoundsCollider.enabled ||
            !worldBoundsCollider.gameObject.activeInHierarchy)
        {
            return false;
        }

        Bounds bounds = worldBoundsCollider.bounds;
        float span = Mathf.Max(bounds.size.x, bounds.size.z);
        if (span <= Mathf.Epsilon)
            return false;

        float halfSpan = span * 0.5f;
        worldMinX = bounds.center.x - halfSpan;
        worldMaxX = bounds.center.x + halfSpan;
        worldMinZ = bounds.center.z - halfSpan;
        worldMaxZ = bounds.center.z + halfSpan;
        return true;
    }

    private void BuildQuestMarkers()
    {
        questMarkers.Clear();

        if (markerLayer == null || questMarkerTemplate == null)
        {
            Debug.LogWarning("[Minimap] Faltan MarkerLayer o QuestMarkerTemplate.", this);
            return;
        }

        questMarkerTemplate.gameObject.SetActive(false);

        foreach (QuestTarget target in CollectQuestTargets())
        {
            if (target == null || target.questInfo == null || target.worldTarget == null)
            {
                Debug.LogWarning("[Minimap] Se ha omitido una mision con referencias incompletas.", this);
                continue;
            }

            string questId = target.questInfo.id;
            if (string.IsNullOrWhiteSpace(questId))
            {
                Debug.LogWarning("[Minimap] Se ha omitido una mision sin ID.", target.questInfo);
                continue;
            }

            if (questMarkers.ContainsKey(questId))
            {
                Debug.LogWarning($"[Minimap] ID de mision duplicado: {questId}.", this);
                continue;
            }

            RectTransform marker = Instantiate(questMarkerTemplate, markerLayer);
            marker.name = $"QuestMarker - {questId}";
            marker.gameObject.SetActive(true);
            PlaceMarker(marker, target.worldTarget.position);

            questMarkers.Add(questId, new QuestMarker
            {
                questInfo = target.questInfo,
                worldTarget = target.worldTarget,
                marker = marker,
                hasCompletedPrimary = false
            });
        }
    }

    private List<QuestTarget> CollectQuestTargets()
    {
        List<QuestTarget> targets = new();
        HashSet<string> knownQuestIds = new();

        // Keep explicitly configured targets as overrides. This preserves any
        // hand-picked marker position while allowing new NPCs to be automatic.
        if (questTargets != null)
        {
            foreach (QuestTarget target in questTargets)
            {
                if (target == null || target.questInfo == null || target.worldTarget == null)
                    continue;

                string questId = target.questInfo.id;
                if (string.IsNullOrWhiteSpace(questId) || !knownQuestIds.Add(questId))
                    continue;

                targets.Add(target);
            }
        }

        if (!autoDiscoverNpcQuestTargets)
            return targets;

        NPCController[] npcControllers = FindObjectsOfType<NPCController>(true);
        foreach (NPCController npc in npcControllers)
        {
            if (npc == null || npc.QuestInfo == null)
                continue;

            string questId = npc.QuestInfo.id;
            if (string.IsNullOrWhiteSpace(questId))
                continue;

            if (!knownQuestIds.Add(questId))
            {
                Debug.LogWarning($"[Minimap] Hay más de un NPC para la misión '{questId}'. Se conserva el primer marcador.", npc);
                continue;
            }

            targets.Add(new QuestTarget
            {
                questInfo = npc.QuestInfo,
                worldTarget = npc.transform
            });
        }

        return targets;
    }

    private void UpdatePlayerMarker()
    {
        if (playerMarker == null || playerTransform == null)
        {
            SetMarkerVisible(playerMarker, false);
            return;
        }

        SetMarkerVisible(playerMarker, true);
        PlaceMarker(playerMarker, playerTransform.position);
    }

    private void PlaceMarker(RectTransform marker, Vector3 worldPosition)
    {
        if (marker == null)
            return;

        Vector2 normalizedPosition = new(
            Mathf.Clamp01(Mathf.InverseLerp(worldMinX, worldMaxX, worldPosition.x)),
            Mathf.Clamp01(Mathf.InverseLerp(worldMinZ, worldMaxZ, worldPosition.z))
        );

        marker.anchorMin = normalizedPosition;
        marker.anchorMax = normalizedPosition;
        marker.anchoredPosition = Vector2.zero;
    }

    private void RefreshQuestVisibility()
    {
        foreach (QuestMarker marker in questMarkers.Values)
            RefreshQuestMarker(marker);
    }

    private void RefreshQuestMarker(QuestMarker marker)
    {
        if (marker == null || marker.marker == null || marker.questInfo == null)
            return;

        bool hasRequiredLevel = playerStats != null &&
                                playerStats.Level >= marker.questInfo.levelRequirement;
        bool shouldBeVisible = hasRequiredLevel && !marker.hasCompletedPrimary;

        SetMarkerVisible(marker.marker, shouldBeVisible);
        if (shouldBeVisible && marker.worldTarget != null)
            PlaceMarker(marker.marker, marker.worldTarget.position);
    }

    private void OnPlayerLevelChanged(int _)
    {
        RefreshQuestVisibility();
    }

    private void OnQuestStateChanged(Quest quest)
    {
        if (quest == null || quest.questInfo == null)
            return;

        if (!questMarkers.TryGetValue(quest.questInfo.id, out QuestMarker marker))
            return;

        marker.hasCompletedPrimary = quest.hasCompletedPrimary;
        RefreshQuestMarker(marker);
    }

    private void TrySubscribeToEvents()
    {
        if (subscribedToEvents || GameEventsManager.instance == null)
            return;

        GameEventsManager.instance.playerEvents.OnLevelChanged += OnPlayerLevelChanged;
        GameEventsManager.instance.questEvents.OnQuestStateChanged += OnQuestStateChanged;
        subscribedToEvents = true;
    }

    private void UnsubscribeFromEvents()
    {
        if (!subscribedToEvents)
            return;

        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.playerEvents.OnLevelChanged -= OnPlayerLevelChanged;
            GameEventsManager.instance.questEvents.OnQuestStateChanged -= OnQuestStateChanged;
        }

        subscribedToEvents = false;
    }

    private static void SetMarkerVisible(RectTransform marker, bool visible)
    {
        if (marker != null && marker.gameObject.activeSelf != visible)
            marker.gameObject.SetActive(visible);
    }
}
