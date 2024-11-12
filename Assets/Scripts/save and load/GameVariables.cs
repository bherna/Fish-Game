using UnityEngine;

public static class GameVariables 
{
    private static string curr_level;

    public static void UpdateLevel(string newLevel){
        curr_level = newLevel;
    }

    public static string GetLevel(){
        return curr_level;
    }
}
