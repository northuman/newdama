using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIElements : MonoBehaviour
{
    public int vida1;
    public TMP_Text vida1Text;
    public GameObject jugador1;
    // Start is called before the first frame update
    void Start()
    {
        jugador1 = GameObject.Find("Jugador");
    }

    // Update is called once per frame
    void Update()
    {
        vida1 = jugador1.GetComponent<Jugador>().vida;


        vida1Text.text = "" + vida1;
    }
}
