using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStart : MonoBehaviour
{
    public void LoadGame(){
        SceneManager.LoadScene("Juego"); // Escena del juego
    }
}
