using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public delegate void OnLevelChange(int level);
    public event OnLevelChange onLevelChange;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void levelUp(int level)
    {
        onLevelChange?.Invoke(level);
    }
}
