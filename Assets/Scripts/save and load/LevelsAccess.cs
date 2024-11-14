using UnityEngine;

public static class LevelsAccess
{
    //holds all the boolean values for each level from levels reference
    public static bool[,] levels_access;

    
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
                NewGame();
            }
            
        }
        
    }

    //creates a new list for level access, making the first level the only one accessable
    public static void NewGame(){
        
        //Debug.Log("New Game.");

        //new [all false] 2d arary 
        levels_access = new bool[3,7];//final index doesn't exist for us, since we start at 1

        //set tank 0 (tutorial) to true only
        levels_access[1,1] = true;

    }
    

    //Updateing level access array (for other scripts to acces really)
    /// <summary>
    /// Gets current tank - level access. 
    /// For each parameter: tank 1 starts at 0, and level 1 starts at 0. 
    /// </summary>
    /// <param name="tank">sub 1 for correct tank, in array</param>
    /// <param name="level">sub 1 for correct level, in array</param>
    /// <returns></returns>
    public static bool GetLevel_Access(int tank, int level) {
        return levels_access[tank, level];
    }


    /// <summary>
    /// Set a current tank - level access 
    /// tank 1 starts at index 1 
    /// (tank 0 is tutorial tank)
    /// Accesstype: true to play level, false to not be able to play level yet.
    /// </summary>
    /// <param name="tank"></param>
    /// <param name="level"></param>
    /// <param name="accessType"></param>
    public static void UnlockLevel_Access(int tank, int level){
        levels_access[tank, level] = true;
    }





}
