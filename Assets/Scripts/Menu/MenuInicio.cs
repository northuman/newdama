using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    public void Jugar(){
        Debug.Log("Pulso jugar");
        SceneManager.LoadScene("Juego", LoadSceneMode.Single);
    }
    public void Salir(){
        Debug.Log("Salir del juego");
        Application.Quit();
    }
}
