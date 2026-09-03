using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PararPartida : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(CerrarEscenaYVolverAStoryScene());
    }

    private IEnumerator CerrarEscenaYVolverAStoryScene()
    {
        yield return new WaitForSeconds(5f);

        Scene escenaActual = SceneManager.GetActiveScene();
        Debug.Log("Cerrando la escena: " + escenaActual.name);

        // Cargar StoryScene si no está cargada
        if (!SceneManager.GetSceneByName("StoryScene").isLoaded)
        {
            AsyncOperation cargarStory = SceneManager.LoadSceneAsync("StoryScene", LoadSceneMode.Additive);
            Debug.Log("Cargando StoryScene...");
            while (!cargarStory.isDone)
                yield return null;
        }

        // Descargar la escena actual
        AsyncOperation descargar = SceneManager.UnloadSceneAsync(escenaActual);
        while (!descargar.isDone)
            yield return null;

        // Establecer StoryScene como la escena activa
        Scene storyScene = SceneManager.GetSceneByName("StoryScene");
        if (storyScene.IsValid())
        {
            SceneManager.SetActiveScene(storyScene);
            ActivarCamaraPrincipal(storyScene);
        }
        else
        {
            Debug.LogWarning("No se pudo establecer StoryScene como escena activa.");
        }
    }

    // Método para activar la cámara principal en la escena especificada
    private void ActivarCamaraPrincipal(Scene scene)
    {
        GameObject[] rootObjects = scene.GetRootGameObjects();
        foreach (GameObject go in rootObjects)
        {
            Camera cam = go.GetComponentInChildren<Camera>();
            if (cam != null)
            {
                cam.gameObject.SetActive(true);
                Debug.Log("Cámara activada: " + cam.gameObject.name);
                // Desactivar otras cámaras si es necesario
                DesactivarOtrasCamaras(cam);
                return;
            }
        }
        Debug.LogWarning("No se encontró una cámara en StoryScene.");
    }

    // Método para desactivar todas las cámaras excepto la especificada
    private void DesactivarOtrasCamaras(Camera camPrincipal)
    {
        Camera[] todasLasCamaras = Camera.allCameras;
        foreach (Camera cam in todasLasCamaras)
        {
            if (cam != camPrincipal)
            {
                cam.gameObject.SetActive(false);
                Debug.Log("Cámara desactivada: " + cam.gameObject.name);
            }
        }
    }
}