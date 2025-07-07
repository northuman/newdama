using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* 
* Controla las transiciones del menú de inicio a la partida.
* Contiene las funciones de los botones.
*/
public class MenuInicio : MonoBehaviour
{
    public Transicion transicion;
    public string nombreEscenaMenu = "Menu Inicio";

    public void Jugar()
    {
        SceneManager.LoadScene("Juego", LoadSceneMode.Single);
    }
    public void Salir()
    {
        Application.Quit();
    }
    public void AbrirEnlace(string url)
    {
        Application.OpenURL("https://www.vecteezy.com/free-vector/backdrop");
    }
}
