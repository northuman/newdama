using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/*
* Script con funciones auxiliares para manejar las cartas
*/

static class UtilCartas{

    
    private static System.Random rng = new System.Random();


    //Shuffle, randomizar orden en lista
    public static void Randomizar<T>(this IList<T> list){  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }

    //Swap, intercambio de dos elementos en lista
    public static void Intercambio<T>(this IList<T> list, int indexA, int indexB){
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

    //Int aleatorio de num1 a num2, ellos incluidos
    public static int NumAleatorio(int num1, int num2){
        int res = rng.Next(num1,num2+1);
        return res;
    }

    //Copiar valores de lista A a B: B = A.ToList()
    public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
    {
        return new List<TSource>(source);
    }

    

    //Funcion para cambiar el color de las cartas
    public static void ColorearCarta(GameObject carta)  
    {      
        //guardo el nombre del color de la carta
        string colorCarta = carta.GetComponent<MostrarCarta>().color;

        Color blanco = new Color32(186,178,162,255);
        Color negro = new Color32(41,41,41,255);
        Color rojo = new Color32(128,71,76,255);
        Color verde = new Color32(81,116,78,255);
        Color gris = new Color32(147,147,147,255);

        Sprite simbolo = null;
        //carta.transform.Find("Borde/Simbolo").AddComponent<Image>().sprite=nobleza; -> colocar el simbolo de cada baraja
        
        if(colorCarta.Length==1){
            switch(colorCarta)
            {
                case "B":
                    carta.transform.Find("Borde/Color").GetComponent<Image>().color = blanco;
                    simbolo = Resources.Load<Sprite>("sprites/simbolos/clero");
                    break;

                case "N":
                    carta.transform.Find("Borde/Color").GetComponent<Image>().color = negro;
                    carta.transform.Find("Borde/Color/Lineas/NombreText").GetComponent<TextMeshProUGUI>().color = gris;
                    carta.transform.Find("Borde/Color/Lineas/Coste").GetComponent<TextMeshProUGUI>().color = gris;
                    carta.transform.Find("Borde/Color/Lineas/Flavor").GetComponent<TextMeshProUGUI>().color = gris;
                    carta.transform.Find("Borde/Color/Lineas/Tipo").GetComponent<TextMeshProUGUI>().color = gris;
                    carta.transform.Find("Borde/Color/Lineas/DescripcionText").GetComponent<TextMeshProUGUI>().color=gris;
                    simbolo = Resources.Load<Sprite>("sprites/simbolos/marginados");
                    break;

                case "R":
                    carta.transform.Find("Borde/Color").GetComponent<Image>().color = rojo;
                    simbolo = Resources.Load<Sprite>("sprites/simbolos/nobleza");
                    break;

                case "V":
                    carta.transform.Find("Borde/Color").GetComponent<Image>().color = verde;
                    simbolo = Resources.Load<Sprite>("sprites/simbolos/pueblo");
                    break;

            }

            // Asignar sprite al objeto "Borde/Simbolo"
            if (simbolo != null)
            {
                Image simboloImage = carta.transform.Find("Borde/Color/Lineas/Simbolo").GetComponent<Image>();
                simboloImage.sprite = simbolo;
                simboloImage.enabled = true; // Por si el objeto estaba oculto
            }
        }
    }
}