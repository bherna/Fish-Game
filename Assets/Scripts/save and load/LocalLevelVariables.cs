using System;
using System.Text.RegularExpressions;
using UnityEngine;






[System.Serializable]
public class WholeJsonScript{

    //
    //MAKE SURE THAT THE NAMES OF THE VARIABLES HERE ARE SAME AS JSON SCRIPT NAMES
    //

    //all waves associated with this level
    public Single_EnemyWave[] waves;
    //should we loop the waves
    public bool loop;
    //what pet that will be unlocked
    public string petNameString;
    //name of next level to unlock (should be the json file name ie: level_#1-#2) -> int[#1, #2]
    public int[] levelUnlock;

    public int startMoney;

    public int[] eggPiecesPrices;
}









public static class LocalLevelVariables 
{
    private static bool flag = true; //this will be true on start, I dont want errors showing up in console
                                                //it gets annoying
    public static string curr_level {get; private set;}= "level_test"; //this json file exsists
  
    private static WholeJsonScript wholeJsonScript;

    public static void UpdateLevel(string newLevel){

        //update what our level name is
        //in other words what are json script we are reading
        curr_level = newLevel;

        //update our whole json script class to hold reference to json file
        wholeJsonScript = JsonUtility.FromJson<WholeJsonScript>(LoadResourceTextfile());

        flag = false;
    }

    private static string LoadResourceTextfile()
    {

        string filePath = "Levels/" + curr_level;

        TextAsset targetFile = Resources.Load<TextAsset>(filePath);

        return targetFile.text;
    }

    //return what tank we are in
    public static string GetTankWorld(){
        if(flag){return "0";}
        return Regex.Match(curr_level, @"\d+").Value;
    }
    //get the level we are in
    public static string GetLevel(){
        if(flag){return "0";}
        return Regex.Match(curr_level, @"\d+", RegexOptions.RightToLeft).Value;
    }


    public static Tank_EnemyWaves GetTank_EnemyWaves(){
        if(flag){return new Tank_EnemyWaves();};

        Tank_EnemyWaves tank_EnemyWaves = new Tank_EnemyWaves(wholeJsonScript.waves, wholeJsonScript.loop);
        return tank_EnemyWaves;
    }


    public static PetNames GetPetUnlock(){

        if(flag){return PetNames.SchoolTeacher;}

        //get pet to unlock string name from json file and convert to PetName enum type, 
        Enum.TryParse(wholeJsonScript.petNameString, out PetNames petToUnlock);

        return petToUnlock;
    }
    
    public static string GetPetUnlock_AsString(){
        if(flag){return null;}

        return wholeJsonScript.petNameString;
    }

    public static int[] GetlevelUnlock(){
        if(flag){return null;}
        return wholeJsonScript.levelUnlock;
    }

    public static int GetStartMoney(){
        if(flag){return 0;}
        return wholeJsonScript.startMoney;
    }

    public static int[] GetEggPiecesPrices(){
        if(flag){return null;}
        return wholeJsonScript.eggPiecesPrices;
    }


}
