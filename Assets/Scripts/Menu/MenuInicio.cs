using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    public void Jugar(){
        SceneManager.LoadScene("Juego", LoadSceneMode.Single);
    }
    public void Salir(){
        Application.Quit();
    }
}
