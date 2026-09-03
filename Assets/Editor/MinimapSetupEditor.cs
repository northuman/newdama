using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class MinimapSetupEditor
{
    private const string MenuPath = "Tools/Story/Crear o actualizar minimapa";

    private static readonly (string rootName, string assetPath)[] PrimaryQuests =
    {
        ("1_MadreSuperiora", "Assets/Scripts/StoryScripts/QuestSystem/NPCs/Primary NPCs/1_MadreSuperiora/MadreSuperiora.asset"),
        ("2_Guarda", "Assets/Scripts/StoryScripts/QuestSystem/NPCs/Primary NPCs/2_Guarda 14/Guarda.asset"),
        ("3_Molinero", "Assets/Scripts/StoryScripts/QuestSystem/NPCs/Primary NPCs/3_Molinero/Molinero.asset"),
        ("4_Destripat", "Assets/Scripts/StoryScripts/QuestSystem/NPCs/Primary NPCs/4_Destripat/Destripat.asset"),
        ("5_Ermitaño", "Assets/Scripts/StoryScripts/QuestSystem/NPCs/Primary NPCs/5_Ermitaño/Ermitano.asset"),
        ("6_1_GuardaMarqués", "Assets/Scripts/StoryScripts/QuestSystem/NPCs/Primary NPCs/6_1_GuardaMarqués/GuardaMarqués.asset"),
        ("6_2_Marqués", "Assets/Scripts/StoryScripts/QuestSystem/NPCs/Primary NPCs/6_2_Marqués/Marques.asset"),
        ("7_Damasco", "Assets/Scripts/StoryScripts/QuestSystem/NPCs/Primary NPCs/7_Damasco/Sepulturero.asset"),
        ("8_Levante", "Assets/Scripts/StoryScripts/QuestSystem/NPCs/Primary NPCs/8_Levante/GuardaLevante.asset"),
        ("9_DMartin", "Assets/Scripts/StoryScripts/QuestSystem/NPCs/Primary NPCs/9_DMartin/DMartin.asset"),
        ("10_Prostíbulo", "Assets/Scripts/StoryScripts/QuestSystem/NPCs/Primary NPCs/10_Prostíbulo/Paca.asset"),
        ("11_Hospital", "Assets/Scripts/StoryScripts/QuestSystem/NPCs/Primary NPCs/11_Hospital/FrayArsacio.asset"),
        ("12_Terciopelero", "Assets/Scripts/StoryScripts/QuestSystem/NPCs/Primary NPCs/12_Terciopelero/Terciopelero.asset"),
        ("13_NiñoDMartin", "Assets/Scripts/StoryScripts/QuestSystem/NPCs/Primary NPCs/13_NiñoDMartín/DMartinFinal.asset")
    };

    [MenuItem(MenuPath)]
    private static void SetupMinimap()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling)
        {
            EditorUtility.DisplayDialog(
                "Minimapa",
                "Sal de Play Mode y espera a que Unity termine de compilar antes de crear el minimapa.",
                "Aceptar");
            return;
        }

        Scene scene = SceneManager.GetActiveScene();
        if (!scene.IsValid() || !scene.isLoaded)
        {
            Debug.LogError("[Minimap Setup] No hay una escena activa valida.");
            return;
        }

        Canvas hudCanvas = FindSceneComponentByName<Canvas>(scene, "Canvas - HUD");
        if (hudCanvas == null)
        {
            Debug.LogError("[Minimap Setup] No se encontro 'Canvas - HUD' en la escena activa.");
            return;
        }

        GameObject player = FindSceneGameObjectWithTag(scene, "Player");
        PlayerStats playerStats = FindSceneComponent<PlayerStats>(scene);
        TerrainCollider terrainCollider = FindSceneComponentByName<TerrainCollider>(scene, "Zona rural y Ciudad");

        if (player == null || playerStats == null || terrainCollider == null)
        {
            Debug.LogError(
                "[Minimap Setup] Faltan referencias obligatorias: Player, PlayerStats o TerrainCollider de 'Zona rural y Ciudad'.");
            return;
        }

        RectTransform minimap = GetOrCreateUIChild(hudCanvas.transform, "Minimap");
        minimap.anchorMin = Vector2.one;
        minimap.anchorMax = Vector2.one;
        minimap.pivot = Vector2.one;
        minimap.anchoredPosition = new Vector2(-24f, -130f);
        minimap.sizeDelta = new Vector2(260f, 260f);
        minimap.localScale = Vector3.one;
        minimap.gameObject.layer = hudCanvas.gameObject.layer;

        Image background = GetOrAddComponent<Image>(minimap.gameObject);
        background.color = new Color32(45, 37, 31, 217);
        background.raycastTarget = false;

        Outline outline = GetOrAddComponent<Outline>(minimap.gameObject);
        outline.effectColor = new Color32(216, 184, 115, 255);
        outline.effectDistance = new Vector2(2f, -2f);
        outline.useGraphicAlpha = true;

        GetOrAddComponent<RectMask2D>(minimap.gameObject);
        MinimapController controller = GetOrAddComponent<MinimapController>(minimap.gameObject);

        RectTransform markerLayer = GetOrCreateUIChild(minimap, "MarkerLayer");
        markerLayer.anchorMin = Vector2.zero;
        markerLayer.anchorMax = Vector2.one;
        markerLayer.pivot = new Vector2(0.5f, 0.5f);
        markerLayer.offsetMin = new Vector2(12f, 12f);
        markerLayer.offsetMax = new Vector2(-12f, -12f);
        markerLayer.localScale = Vector3.one;
        markerLayer.gameObject.layer = hudCanvas.gameObject.layer;

        RectTransform questTemplate = GetOrCreateUIChild(markerLayer, "QuestMarkerTemplate");
        ConfigureMarker(
            questTemplate,
            new Vector2(14f, 14f),
            new Color32(244, 197, 66, 255));
        questTemplate.gameObject.SetActive(false);

        RectTransform playerMarker = GetOrCreateUIChild(markerLayer, "PlayerMarker");
        ConfigureMarker(
            playerMarker,
            new Vector2(18f, 18f),
            new Color32(77, 220, 255, 255));
        playerMarker.gameObject.SetActive(true);
        playerMarker.SetAsLastSibling();

        List<(QuestInfoSO questInfo, Transform worldTarget)> targets = CollectPrimaryQuestTargets(scene);
        ConfigureQuestManager(scene, targets);
        ConfigureController(
            controller,
            player.transform,
            playerStats,
            terrainCollider,
            markerLayer,
            questTemplate,
            playerMarker,
            targets);

        minimap.SetAsLastSibling();
        EditorSceneManager.MarkSceneDirty(scene);
        Selection.activeGameObject = minimap.gameObject;

        Debug.Log(
            $"[Minimap Setup] Minimap creado o actualizado con {targets.Count} misiones principales. " +
            "Revisa el resultado y guarda StoryScene.",
            minimap.gameObject);
    }

    public static void SetupStorySceneBatch()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/StoryScene.unity", OpenSceneMode.Single);
        SetupMinimap();
        EditorSceneManager.SaveOpenScenes();
    }

    private static List<(QuestInfoSO questInfo, Transform worldTarget)> CollectPrimaryQuestTargets(Scene scene)
    {
        List<(QuestInfoSO questInfo, Transform worldTarget)> targets = new();

        foreach ((string rootName, string assetPath) in PrimaryQuests)
        {
            GameObject root = FindSceneRoot(scene, rootName);
            if (root == null)
            {
                Debug.LogWarning($"[Minimap Setup] No se encontro la raiz '{rootName}'.");
                continue;
            }

            NPCController npc = root.GetComponentInChildren<NPCController>(true);
            if (npc == null)
            {
                Debug.LogWarning($"[Minimap Setup] '{rootName}' no contiene un NPCController.", root);
                continue;
            }

            QuestInfoSO questInfo = AssetDatabase.LoadAssetAtPath<QuestInfoSO>(assetPath);
            if (questInfo == null)
            {
                Debug.LogWarning($"[Minimap Setup] No se pudo cargar el QuestInfoSO de '{rootName}' en '{assetPath}'.", npc);
                continue;
            }

            SerializedObject npcObject = new(npc);
            SerializedProperty questInfoProperty = npcObject.FindProperty("questInfo");
            if (questInfoProperty != null)
            {
                questInfoProperty.objectReferenceValue = questInfo;
                npcObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(npc);
            }

            targets.Add((questInfo, npc.transform));
        }

        return targets;
    }

    private static void ConfigureQuestManager(
        Scene scene,
        List<(QuestInfoSO questInfo, Transform worldTarget)> targets)
    {
        QuestManager questManager = FindSceneComponent<QuestManager>(scene);
        if (questManager == null)
        {
            Debug.LogWarning("[Minimap Setup] No se encontro QuestManager en la escena.");
            return;
        }

        SerializedObject serializedManager = new(questManager);
        SerializedProperty questInfos = serializedManager.FindProperty("questInfos");
        if (questInfos == null)
        {
            Debug.LogWarning("[Minimap Setup] QuestManager no contiene la propiedad 'questInfos'.", questManager);
            return;
        }

        questInfos.arraySize = targets.Count;
        for (int i = 0; i < targets.Count; i++)
            questInfos.GetArrayElementAtIndex(i).objectReferenceValue = targets[i].questInfo;

        serializedManager.ApplyModifiedProperties();
        EditorUtility.SetDirty(questManager);
    }

    private static void ConfigureController(
        MinimapController controller,
        Transform player,
        PlayerStats playerStats,
        Collider worldBounds,
        RectTransform markerLayer,
        RectTransform questTemplate,
        RectTransform playerMarker,
        List<(QuestInfoSO questInfo, Transform worldTarget)> targets)
    {
        SerializedObject serializedController = new(controller);
        serializedController.FindProperty("playerTransform").objectReferenceValue = player;
        serializedController.FindProperty("playerStats").objectReferenceValue = playerStats;
        serializedController.FindProperty("worldBoundsCollider").objectReferenceValue = worldBounds;
        serializedController.FindProperty("markerLayer").objectReferenceValue = markerLayer;
        serializedController.FindProperty("questMarkerTemplate").objectReferenceValue = questTemplate;
        serializedController.FindProperty("playerMarker").objectReferenceValue = playerMarker;

        SerializedProperty targetsProperty = serializedController.FindProperty("questTargets");
        targetsProperty.arraySize = targets.Count;
        for (int i = 0; i < targets.Count; i++)
        {
            SerializedProperty element = targetsProperty.GetArrayElementAtIndex(i);
            element.FindPropertyRelative("questInfo").objectReferenceValue = targets[i].questInfo;
            element.FindPropertyRelative("worldTarget").objectReferenceValue = targets[i].worldTarget;
        }

        serializedController.ApplyModifiedProperties();
        EditorUtility.SetDirty(controller);
    }

    private static void ConfigureMarker(RectTransform marker, Vector2 size, Color32 color)
    {
        marker.gameObject.layer = marker.parent.gameObject.layer;
        marker.anchorMin = new Vector2(0.5f, 0.5f);
        marker.anchorMax = new Vector2(0.5f, 0.5f);
        marker.pivot = new Vector2(0.5f, 0.5f);
        marker.anchoredPosition = Vector2.zero;
        marker.sizeDelta = size;
        marker.localScale = Vector3.one;
        marker.localRotation = Quaternion.Euler(0f, 0f, 45f);

        Image image = GetOrAddComponent<Image>(marker.gameObject);
        image.color = color;
        image.raycastTarget = false;
    }

    private static RectTransform GetOrCreateUIChild(Transform parent, string name)
    {
        Transform existing = parent.Find(name);
        if (existing != null)
        {
            RectTransform existingRect = existing as RectTransform;
            if (existingRect == null)
                Debug.LogError($"[Minimap Setup] '{name}' existe pero no tiene RectTransform.", existing);
            return existingRect;
        }

        GameObject child = new(name, typeof(RectTransform));
        Undo.RegisterCreatedObjectUndo(child, $"Crear {name}");
        RectTransform rect = child.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        return rect;
    }

    private static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        return component != null ? component : Undo.AddComponent<T>(gameObject);
    }

    private static GameObject FindSceneRoot(Scene scene, string name)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            if (root.name == name)
                return root;
        }

        return null;
    }

    private static T FindSceneComponentByName<T>(Scene scene, string gameObjectName) where T : Component
    {
        foreach (T component in Resources.FindObjectsOfTypeAll<T>())
        {
            if (component.gameObject.scene == scene && component.gameObject.name == gameObjectName)
                return component;
        }

        return null;
    }

    private static T FindSceneComponent<T>(Scene scene) where T : Component
    {
        foreach (T component in Resources.FindObjectsOfTypeAll<T>())
        {
            if (component.gameObject.scene == scene)
                return component;
        }

        return null;
    }

    private static GameObject FindSceneGameObjectWithTag(Scene scene, string tag)
    {
        foreach (GameObject gameObject in scene.GetRootGameObjects())
        {
            foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>(true))
            {
                if (transform.CompareTag(tag))
                    return transform.gameObject;
            }
        }

        return null;
    }
}
