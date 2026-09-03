using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnGameOver -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        Debug.Log("[GameOverManager] Game Over.");
        LoadSceneWithFade("GameOver");
    }

    public void Retry()
    {
        LoadSceneWithFade("StoryScene");
    }

    public void GoToMainMenu()
    {
        LoadSceneWithFade("Menu inicio");
    }

    private void LoadSceneWithFade(string sceneName)
    {
        if (FadeManager.instance != null)
            FadeManager.instance.LoadScene(sceneName);
        else
            SceneManager.LoadScene(sceneName);
    }
}
