using System;
using System.Linq;
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


public static class GameVariables 
{
    public static string curr_level {get; private set;}= "level_test"; //this json file exsists
  
    private static WholeJsonScript wholeJsonScript;

    public static void UpdateLevel(string newLevel){

        //update what our level name is
        //in other words what are json script we are reading
        curr_level = newLevel;

        //update our whole json script class to hold reference to json file
        wholeJsonScript = JsonUtility.FromJson<WholeJsonScript>(LoadResourceTextfile());
    }

    private static string LoadResourceTextfile()
    {

        string filePath = "Levels/" + curr_level;

        TextAsset targetFile = Resources.Load<TextAsset>(filePath);

        return targetFile.text;
    }

    //return what tank we are
    public static string GetTankWorld(){
        return Regex.Match(curr_level, @"\d+").Value;
    }
    //get the level we are
    public static string GetLevel(){
        return Regex.Match(curr_level, @"\d+", RegexOptions.RightToLeft).Value;
    }


    public static Tank_EnemyWaves GetTank_EnemyWaves(){

        Tank_EnemyWaves tank_EnemyWaves = new Tank_EnemyWaves(wholeJsonScript.waves, wholeJsonScript.loop);
        return tank_EnemyWaves;
    }


    public static PetNames GetPetUnlock(){

        //get pet to unlock string name from json file and convert to PetName enum type, 
        Enum.TryParse(wholeJsonScript.petNameString, out PetNames petToUnlock);

        return petToUnlock;
    }
    
    public static string GetPetUnlock_AsString(){
        return wholeJsonScript.petNameString;
    }

    public static int[] GetlevelUnlock(){

        return wholeJsonScript.levelUnlock;
    }

    public static int GetStartMoney(){
        return wholeJsonScript.startMoney;
    }

    public static int[] GetEggPiecesPrices(){
        return wholeJsonScript.eggPiecesPrices;
    }


}
