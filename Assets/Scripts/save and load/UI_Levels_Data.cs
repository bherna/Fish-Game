
using UnityEngine;

[System.Serializable]
public class UI_Levels_Data
{
    

    //levels save load stuff:
    public bool[,] levels;

    //constuctor
    public UI_Levels_Data(UI_Levels ui_Levels)
    {
        levels = ui_Levels.levels_access;
    }

    
}
