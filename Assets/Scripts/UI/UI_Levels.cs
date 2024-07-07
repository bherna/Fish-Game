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


    private void Start() {
        
        //load save file  
        LevelsAccess.LoadLevels();

        //update levels on panels
        UI_LevelsUpdateAccess();
    }

    
    private void UI_LevelsUpdateAccess(){

        for(int i = 0; i < levels_reference.GetLength(0); i++){
            for(int j = 0; j < levels_reference[i].world_level.GetLength(0); j++) {
                
                if(LevelsAccess.GetLevelAccess(i, j)){
                    levels_reference[i].world_level[j].GetComponent<Button>().interactable = true;
                }
                else{
                    levels_reference[i].world_level[j].GetComponent<Button>().interactable = false;
                }
            }
        }
    }



    





    
}
