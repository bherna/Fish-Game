using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class LevelsAccess
{
    //holds all the boolean values for each level from levels reference
    public static bool[,] levels_access;



    //save current levels access for game 
    public static void SaveLevels(){
        SaveLoad.SaveGame(); //don't need to call self else 'this'
    }

    
    
    
    //pull any saved data loaded within the save path for level access, if non, create new game level access
    public static void LoadLevels_Array(){
        

        //make sure we only run this once per game start up
        if(levels_access == null){

            Levels_Data_Serializable data = SaveLoad.Load_Levels();
            
            if(data != null){
                //then load levels
                levels_access = data.levels;
            }
            else{
                //then we are a new game
                NewLevels_Array();
            }
            
        }
        
    }

    //creates a new list for level access, making the first level the only one accessable
    public static void NewLevels_Array(){
        
        Debug.Log("New Game.");

        //new [all false] 2d arary 
        levels_access = new bool[2,6];

        //set level 1-1
        levels_access[0,0] = true;

    }
    

    //Updateing level access array (for other scripts to acces really)
    /// <summary>
    /// Gets current world - level access. 
    /// For each parameter: world 1 starts at 0, and level 1 starts at 0. 
    /// </summary>
    /// <param name="world">sub 1 for correct world, in array</param>
    /// <param name="level">sub 1 for correct level, in array</param>
    /// <returns></returns>
    public static bool GetLevel_Access(int world, int level) {
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
    public static void UnlockLevel_Access(int world, int level, bool accessType){
        levels_access[world, level] = accessType;
    }





}
