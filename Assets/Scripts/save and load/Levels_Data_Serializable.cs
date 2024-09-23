
using UnityEngine;



/// <summary>
/// wrapper class to hold LevelsAccess as a Serializable non static
/// 
/// Used by the SaveandLoad class
/// </summary>
[System.Serializable]
public class Levels_Data_Serializable
{
    

    //levels save load stuff:
    public bool[,] levels;

    //constuctor
    public Levels_Data_Serializable()
    {
        levels = LevelsAccess.levels_access; //pulls levels access from exisiting level access script
    }

    
}
