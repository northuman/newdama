using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    
    void Start() {
        
    }

    void Update() {
        
    }

    public void EscenaArena() {

        SceneManager.LoadScene("Arena");
    }

    public void EscenaEditor() {

        SceneManager.LoadScene("EditorBarajas");
    }
}
