using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    public GameObject menuPausa;
    public Transicion transicion;
    public string nombreEscenaMenu = "Menu Inicio";

    public void Jugar(){
        SceneManager.LoadScene("Juego", LoadSceneMode.Single);
    }
    public void Salir(){
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isActive = menuPausa.activeSelf;
            menuPausa.SetActive(!isActive);
            Time.timeScale = isActive ? 1f : 0f; // Pausa el juego
        }
    }
    public void SalirDeljuego()
    {
        Time.timeScale = 1f; // Asegura que el tiempo vuelva a la normalidad
        transicion.LoadScene(nombreEscenaMenu); // Llama a la transición

        // En el editor, para simular la salida
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void Reanudar(){
        menuPausa.SetActive(false);
        Time.timeScale = 1f;
    }
}
