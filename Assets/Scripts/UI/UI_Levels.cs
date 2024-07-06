using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Levels2d
{
    //list that holds one world's set of levels
    [SerializeField] public GameObject[] world_level;
}

public  class UI_Levels : MonoBehaviour 
{
    //this class only holds references to all levels on the main menu scene
    [SerializeField] public Levels2d[] levels_reference;

    //holds all the boolean values for each level from levels reference
    public bool[,] levels_access;


    private void Start() {
        
        //load save file  
        LoadLevels();

        //update levels on panels
        UI_LevelsUpdateAccess();
    }

    
    private void UI_LevelsUpdateAccess(){

        for(int i = 0; i < levels_reference.GetLength(0); i++){
            for(int j = 0; j < levels_reference[i].world_level.GetLength(0); j++) {
                
                if(levels_access[i,j]){
                    levels_reference[i].world_level[j].GetComponent<Button>().interactable = true;
                }
                else{
                    levels_reference[i].world_level[j].GetComponent<Button>().interactable = false;
                }
            }
        }
    }



    //Updateing level access array (for other scripts to acces really)
    public bool GetLevelAccess(int world, int level) {
        return levels_access[world-1, level-1];
    }
        
    public void SetLevelAccess(int world, int level, bool accessType){
        levels_access[world-1, level-1] = accessType;
    }





    //save and load levels for game functions
    public void SaveLevels(){
        SaveLoad.SaveLevels(this);
    }
    public void LoadLevels(){
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
}
