using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/*
* Controla las animaciones de transición.
* fadeGroup es la imagen de transición.
* sceneName es el nombre de la escena que se carga.
*/

public class Transicion : MonoBehaviour
{
    public CanvasGroup fadeGroup;
    private readonly float fadeDuracion = 0.2f;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    IEnumerator FadeIn()
    {
        fadeGroup.alpha = 1;
        while (fadeGroup.alpha > 0)
        {
            fadeGroup.alpha -= Time.deltaTime / fadeDuracion;
            yield return null;
        }
        fadeGroup.blocksRaycasts = false;
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        fadeGroup.blocksRaycasts = true;
        while (fadeGroup.alpha < 1)
        {
            fadeGroup.alpha += Time.deltaTime / fadeDuracion;
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
}
