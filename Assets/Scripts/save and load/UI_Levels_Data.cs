
using UnityEngine;

[System.Serializable]
public class UI_Levels_Data
{
    

    //levels save load stuff:
    public bool[,] levels;

    //constuctor
    public UI_Levels_Data()
    {
        levels = LevelsAccess.levels_access;
    }

    
}
