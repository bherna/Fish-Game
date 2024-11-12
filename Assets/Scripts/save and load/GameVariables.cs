using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class GameVariables 
{
    private static string curr_level = "";
    private static Tank_EnemyWaves tank_EnemyWaves;

    public static void UpdateLevel(string newLevel){
        curr_level = newLevel;
    }

    public static int[] GetLevel_AsArray(){

        //regex to parse level_#-#
        //we want these two # values
        //first # is the tank
        //sec # is the level
        string pattern = "[0-9]+";
        Regex rg = new Regex(pattern);
        MatchCollection nums = rg.Matches(curr_level);

        int[] result = new int[]{
            int.Parse(nums[0].Value), int.Parse(nums[1].Value)
        };

        return result;
    }


    public static string LoadResourceTextfile()
    {

        //first check if our get level is null
        if(curr_level == ""){
            Debug.Log("Our json reference is missing . . .");
            Debug.Log("Now using level_test.json as reference");
            curr_level = "level_test";
        }

        string filePath = "Levels/" + curr_level;

        TextAsset targetFile = Resources.Load<TextAsset>(filePath);

        return targetFile.text;
    }


}
