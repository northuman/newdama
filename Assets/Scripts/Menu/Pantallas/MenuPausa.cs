using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Controla el menú de pausa.
* Contiene las funciones de los botones.
* Pausa la partida.
*/

public class MenuPausa : MonoBehaviour
{
    public GameObject menuPausa;
    public Transicion transicion;
    public string nombreEscenaMenu = "Menu Inicio";


    public void SalirDeljuego()
    {
        Time.timeScale = 1f; // Asegura que el tiempo vuelva a la normalidad
        transicion.LoadScene(nombreEscenaMenu); // Llama a la transición
    }
    public void Reanudar()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1f;

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
}
