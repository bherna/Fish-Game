using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelsAccess
{
    //holds all the boolean values for each level from levels reference
    public static bool[,] levels_access;



    //save and load levels for game functions
    public static void SaveLevels(){
        SaveLoad.SaveLevels(); //don't need to call self else 'this'
    }
    public static void LoadLevels(){
        try{

            UI_Levels_Data data = SaveLoad.LoadLevels();

            levels_access = data.levels;
        }
        catch(NullReferenceException ){

            //then we are a new game

            //new [all false] 2d arary 
            levels_access = new bool[2,6];

            //set level 1-1
            levels_access[0,0] = true;
        }
        
    }


    //Updateing level access array (for other scripts to acces really)
    /// <summary>
    /// Gets current world - level access. 
    /// For each parameter: world 1 starts at 0, and level 1 starts at 0. 
    /// </summary>
    /// <param name="world">sub 1 for correct world, in array</param>
    /// <param name="level">sub 1 for correct level, in array</param>
    /// <returns></returns>
    public static bool GetLevelAccess(int world, int level) {
        return levels_access[world, level];
    }


    /// <summary>
    /// Set a current world - level access 
    /// World 1 starts at 0, and level 1 starts at 0.
    /// Accesstype: true to play level, false to not be able to play level yet.
    /// </summary>
    /// <param name="world"></param>
    /// <param name="level"></param>
    /// <param name="accessType"></param>
    public static void SetLevelAccess(int world, int level, bool accessType){
        levels_access[world, level] = accessType;
    }
}
