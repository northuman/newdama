using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryGameResult : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private GameObject debugCanvas;

    void Start()
    {
        if (debugCanvas != null)
        {
            debugCanvas.SetActive(GameEventsManager.instance.DebugMode);
        }
    }

    public void ReportResult(bool win)
    {
        if (debugCanvas != null)
        {
            debugCanvas.SetActive(false);
        }
        StartCoroutine(ReturnToStoryScene(win));
    }

    private IEnumerator ReturnToStoryScene(bool win)
    {
        if (FadeManager.instance != null)
            yield return FadeManager.instance.StartFadeOut();

        Scene storyGameScene = gameObject.scene;

        GameEventsManager.instance.dialogueEvents.CombatResolved(win);
        
        if(!SceneManager.GetSceneByName("StoryScene").isLoaded)
        {
            AsyncOperation load = SceneManager.LoadSceneAsync("StoryScene", LoadSceneMode.Additive);
            while (!load.isDone) yield return null;
        }

        Scene storyScene = SceneManager.GetSceneByName("StoryScene");
        if(storyScene.IsValid())
        {
            SceneManager.SetActiveScene(storyScene);
            ReactivatePlayer(storyScene);
        }
        SceneManager.UnloadSceneAsync(storyGameScene);

        if (FadeManager.instance != null)
            yield return FadeManager.instance.StartFadeIn();
    }

    private void ReactivatePlayer(Scene storyScene)
    {
        foreach (GameObject obj in storyScene.GetRootGameObjects())
        {
            Camera cam = obj.GetComponentInChildren<Camera>(true);
            if (cam != null) { cam.gameObject.SetActive(true); break; }
        }

        GameEventsManager.instance.ActiveQuestId = string.Empty;
        Debug.Log("[StoryGameResult] Vuelta a StoryScene completada.");
    }
}

