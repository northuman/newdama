using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance { get; private set; }

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.2f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SetAlpha(0f);
    }


    public void LoadScene(string sceneName)
    {
        StartCoroutine(DoLoadScene(sceneName));
    }
    public Coroutine StartFadeOut() => StartCoroutine(FadeOut());
    public Coroutine StartFadeIn() => StartCoroutine(FadeIn());

    private IEnumerator DoLoadScene(string sceneName)
    {
        yield return FadeOut();
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone) yield return null;
        yield return FadeIn();
    }

    private IEnumerator FadeOut() => Fade(0f, 1f);
    private IEnumerator FadeIn()  => Fade(1f, 0f);

    private IEnumerator Fade(float from, float to)
    {
        SetAlpha(from);
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            SetAlpha(Mathf.Lerp(from, to, elapsed / fadeDuration));
            yield return null;
        }
        SetAlpha(to);
    }

    private void SetAlpha(float alpha)
    {
        if (fadeImage == null) return;
        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
        fadeImage.raycastTarget = alpha > 0f;
    }
}
