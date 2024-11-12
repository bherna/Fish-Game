using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class GameVariables 
{
    private static string curr_level;

    public static void UpdateLevel(string newLevel){
        curr_level = newLevel;
    }

    public static string GetLevel_AsString(){
        return curr_level;
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
}
