using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public  class UI_Levels : MonoBehaviour 
{
    //this class only holds references to all levels on the main menu scene
    [SerializeField] public GameObject[] content_world_ref;


    private void Start() {
        
        //load save file  
        LevelsAccess.LoadLevels_Array();

        //update levels on panels
        UI_LevelsUpdateAccess();


    }

    
    private void UI_LevelsUpdateAccess(){

        for(int i = 1; i < content_world_ref.GetLength(0); i++){
            for(int j = 1; j < content_world_ref[i].transform.childCount; j++) {
                
                if(LevelsAccess.GetLevel_Access(i, j)){

                    //playable level
                    content_world_ref[i].transform.GetChild(j).GetChild(0).gameObject.GetComponent<Button>().interactable = true;

                    //show current saved record

                }
                else{
                    content_world_ref[i].transform.GetChild(j).GetChild(0).gameObject.GetComponent<Button>().interactable = false;
                }
            }
        }
    }








    
}
