using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public  class UI_Levels : MonoBehaviour 
{
    //this class only holds references to all levels on the main menu scene
    [SerializeField] public GameObject[] content_Tanks;


    private void Start() {
        
        //load save file  
        LevelsAccess.LoadLevels_Array();

        //update levels on panels
        UI_LevelsUpdateAccess();


    }

    
    private void UI_LevelsUpdateAccess(){

        int tankWorld = 1; //we start at 1, so increment this last
        int level = 1;      //same thing as above

        //for each tank/world in our game
        //we for each here to avoid skipping index 0 (since tankworld starts at 1)
        foreach(GameObject tank_ in content_Tanks)
        {
            //for each of the levels associated to that tank/world
            //for each here to avoid skipping index 0
            foreach(Transform levelChild in tank_.transform) 
            {
                if(LevelsAccess.GetLevel_Access(tankWorld, level)){

                    //playable level
                    levelChild.GetChild(0).gameObject.GetComponent<Button>().interactable = true;

                    //show current saved record

                }
                else{
                    levelChild.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
                }

                level++;
            }

            tankWorld++;
        }

    }








    
}
