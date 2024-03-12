using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Fish_Age : MonoBehaviour
{

    //fish stage
    public int current_age_stage {get; private set; } = 0;

    //fish current age until next fish_stage
    [SerializeField] float curr_sec_age = 0;

    //takes how many seconds till next fish stage
    [SerializeField] int until_next_stage = 15;

    //bool if we should keep age-ing
    private bool updateAge = true;


    

    // Update is called once per frame
    void Update()
    {

        if(updateAge){

            //update age of fish
            curr_sec_age += Time.deltaTime;

            if(curr_sec_age > until_next_stage){
                UpdateFishStage();
            }
        }
        
           
    }

    private void UpdateFishStage(){

        //if current age is not final stage
        if(current_age_stage < Controller_Fish.instance.GetFishStages().Count-1){
            
            //update 
            current_age_stage += 1;

            //reset
            curr_sec_age = 0;
            
        }
        else{
            //we are done aging
            updateAge = false;
        }
    }
}
